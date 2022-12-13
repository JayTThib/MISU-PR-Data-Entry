using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MI_PR_Data_Entry
{
    public partial class MainForm : Form
    {
        /// <summary>The operation that the program is attempting to accomplish. This is an enum instead of a bool just in case I decide to add new modes.</summary>
        private enum OperationMode { PlayerId, PlacementsAndRecords }

        #region Fields / properties / constants
        public static TextBox targetRecordsColumnTB;
        public static string errorMessage;

        private SheetSettingsForm sheetSettingsForm;
        private InstructionsForm instructionsForm;
        private SpreadsheetSettingsForm spreadsheetSettingsForm;

        /// <summary></summary>
        private bool isProcessing
        {
            get { return _isProcessing; }

            set
            {
                if (value)
                {
                    foreach (Control con in controlsDisabledWhileProcessing)
                    {
                        con.Enabled = false;
                    }

                    if (sheetSettingsForm != null)
                    {
                        sheetSettingsForm.SetControlsEnabledStatus(false);
                    }
                }
                else
                {
                    foreach (Control con in controlsDisabledWhileProcessing)
                    {
                        con.Enabled = true;
                    }

                    if (sheetSettingsForm != null)
                    {
                        sheetSettingsForm.SetControlsEnabledStatus(true);
                    }
                }

                _isProcessing = value;
            }
        }
        private bool _isProcessing = false;

        private OperationMode currentOperationMode
        {
            get { return _currentOperationMode; }

            set 
            {
                List<Control> tempControlsList;

                #region Hide controls related to the previous mode.
                switch (_currentOperationMode)
                {
                    case OperationMode.PlayerId:
                        tempControlsList = playerIdModeControls;
                        break;

                    case OperationMode.PlacementsAndRecords:
                        tempControlsList = placementsAndRecordsModeControls;
                        break;

                    default:
                        MessageBox.Show("ERROR: Unaccounted for " + nameof(OperationMode) + ": " + _currentOperationMode);
                        return;
                }

                foreach (Control con in tempControlsList)
                {
                    con.Hide();
                }
                #endregion

                #region Show controls related to the new mode.
                switch (value)
                {
                    case OperationMode.PlayerId:
                        tempControlsList = playerIdModeControls;
                        break;

                    case OperationMode.PlacementsAndRecords:
                        tempControlsList = placementsAndRecordsModeControls;
                        break;

                    default:
                        MessageBox.Show("ERROR: Unaccounted for " + nameof(OperationMode) + ": " + value);
                        return;
                }

                foreach (Control con in tempControlsList)
                {
                    if (con.GetType() == typeof(TextBox))
                    {
                        con.Text = string.Empty;
                    }

                    con.Show();
                }
                #endregion

                _currentOperationMode = value;
            }
        }
        private OperationMode _currentOperationMode;

        private readonly List<Control> playerIdModeControls;
        private readonly List<Control> placementsAndRecordsModeControls;
        private readonly List<Control> controlsDisabledWhileProcessing;

        private const string StatusLabelOpener = "STATUS: ";
        private const string StatusLabelSuccess = "Operation successfully finished! ";
        private const string StatusLabelWaiting = "Waiting for user input.";

        #endregion


        public MainForm()
        {
            InitializeComponent();
            SheetSettings.LoadSavedSettings();

            #region Init readonly controls lists
            playerIdModeControls = new List<Control>()
            {
                userSlugLabel,
                userSlugTextBox,
                outputPlayerIdLabel,
                outputPlayerIdTextBox,
                playerIdExecuteButton
            };

            placementsAndRecordsModeControls = new List<Control>()
            {
                eventSlugLabel,
                eventSlugTextBox,
                placementsAndRecordsExecuteButton,
                targetRecordsColumnLabel,
                targetRecordsColumnTextBox
            };

            controlsDisabledWhileProcessing = new List<Control>()
            {
                appNameLabel,
                appNameTextBox,
                sheetIdLabel,
                sheetIdTextBox,
                apiKeyLabel,
                apiKeyTextBox,
                clientSecretsLabel,
                clientSecretsPathTextBox,
                chooseClientSecretPathButton,
                modeGroupBox,
                userSlugLabel,
                userSlugTextBox,
                outputPlayerIdLabel,
                outputPlayerIdTextBox,
                playerIdExecuteButton,
                eventSlugLabel,
                eventSlugTextBox,
                placementsAndRecordsExecuteButton,
                targetRecordsColumnLabel,
                targetRecordsColumnTextBox
            };
            #endregion

            placementsModeRadioButton.Checked = true;
            _currentOperationMode = OperationMode.PlayerId;
            currentOperationMode = OperationMode.PlacementsAndRecords;
            statusLabel.Text = StatusLabelOpener + StatusLabelWaiting;
            targetRecordsColumnTB = targetRecordsColumnTextBox;
        }


        #region Button click functions

        private async void playerIdExecuteButton_Click(object sender, EventArgs e)
        {
            GeneralProcessingStart();

            if (Validator.InvalidStartggApiKey(apiKeyTextBox) || Validator.UserSlugIsInvalid(userSlugTextBox))
            {
                ErrorHandler();
                return;
            }

            try
            {
                statusLabel.Text = StatusLabelOpener + "Getting player ID from Start.gg...";
                await StartggManager.QueryForPlayerId(userSlugTextBox.Text);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                ErrorHandler();
                return;
            }
            
            outputPlayerIdTextBox.Text = StartggManager.outputPlayerId;
            SuccessfullyFinishedProcessing(outputPlayerIdTextBox);
        }
        

        private async void placementsAndRecordsExecuteButton_Click(object sender, EventArgs e)
        {
            GeneralProcessingStart();

            if (Validator.InvalidPrSheetLink(sheetIdTextBox)
                || Validator.InvalidStartggApiKey(apiKeyTextBox)
                || Validator.EventSlugIsInvalid(eventSlugTextBox.Text)
                || Validator.ClientSecretFileDoesntExist(clientSecretsPathTextBox.Text) 
                || Validator.AppNameIsInvalid(appNameTextBox.Text))
            {
                ErrorHandler();
                return;
            }

            #region Try to call the APIs and process data (step by step, not simultaneously); if it fails at any point then abort doing the rest of the steps.
            Dictionary<Func<Task>, string> funcAndStatusDict = new Dictionary<Func<Task>, string>()
            {
                { async() => { await GoogleSheetsManager.SetupService(); }, "Setting up connection to Google Sheets"},
                { async() => { await GoogleSheetsManager.GetTrackedPlayerIdentifiers(); }, "Getting tracked player identifiers from Google Sheets"},
                { async() => { await StartggManager.SetTournamentResultForTrackedPlayers(); }, "Getting tournament results from Start.gg"},
                { async() => { await GoogleSheetsManager.SetTargetRecordsColumn(); }, "Getting target spreadsheet column"},
                { async() => { await GoogleSheetsManager.UploadGeneralTournamentInfo(); }, "Uploading general event info to Google Sheets" },
                { async() => { await GoogleSheetsManager.UploadRecords(); }, "Uploading records to Google Sheets" },
                { async() => { await GoogleSheetsManager.UploadPlacements(); }, "Uploading placements to Google Sheets"}
            };
            
            foreach (KeyValuePair<Func<Task>, string> kvp in funcAndStatusDict)
            {
                try
                {
                    statusLabel.Text = kvp.Value;
                    await kvp.Key.Invoke();
                }
                catch (Exception ex)
                {
                    errorMessage = ex.ToString();
                    ErrorHandler();
                    return;
                }
            }
            #endregion

            targetRecordsColumnTextBox.Text = string.Empty;
            SuccessfullyFinishedProcessing(eventSlugTextBox);
        }

        private void chooseClientSecretPathButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    GoogleSheetsManager.clientSecretsPath = openFileDialog.FileName;
                    Properties.Settings.Default.clientSecretPath = openFileDialog.FileName;
                    clientSecretsPathTextBox.Text = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR\n\n" + ex.Message);
                }
            }
        }

        private void viewSheetRequirementsButton_Click(object sender, EventArgs e)
        {
            spreadsheetSettingsForm = new SpreadsheetSettingsForm();
            spreadsheetSettingsForm.Show();
        }

        private void instructionsButton_Click(object sender, EventArgs e)
        {
            instructionsForm = new InstructionsForm();
            instructionsForm.Show();
        }

        private void sheetSettingsButton_Click(object sender, EventArgs e)
        {
            sheetSettingsForm = new SheetSettingsForm();
            sheetSettingsForm.Show();
        }

        #endregion


        #region Misc MainForm methods

        /// <summary>Clear temporary data (such as error info), and set isProcessing to true. This will disable all controls related to settings.</summary>
        private void GeneralProcessingStart()
        {
            isProcessing = true;
            errorMessage = string.Empty;
        }

        private void placementsModeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            currentOperationMode = OperationMode.PlacementsAndRecords;
        }

        private void playerIdModeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            currentOperationMode = OperationMode.PlayerId;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Load all saved settings. Use the default app name just to make things easier for the MI PR panel.
            if (Properties.Settings.Default.appName == string.Empty 
                || Properties.Settings.Default.appName == GoogleSheetsManager.defaultAppName)
            {
                appNameTextBox.Text = GoogleSheetsManager.defaultAppName;
                GoogleSheetsManager.appName = GoogleSheetsManager.defaultAppName;
            }
            else
            {
                appNameTextBox.Text = Properties.Settings.Default.appName;
                GoogleSheetsManager.appName = Properties.Settings.Default.appName;
            }

            sheetIdTextBox.Text = Properties.Settings.Default.spreadsheetId;
            apiKeyTextBox.Text = Properties.Settings.Default.startggAPIKey;
            clientSecretsPathTextBox.Text = Properties.Settings.Default.clientSecretPath;
        }

        private void ErrorHandler()
        {
            this.ActiveControl = null;
            isProcessing = false;
            statusLabel.Text = StatusLabelOpener + "Operation failed. " + StatusLabelWaiting;
            MessageBox.Show(errorMessage);
        }

        private void SuccessfullyFinishedProcessing(Control controlToFocusOn)
        {
            controlToFocusOn.Focus();
            statusLabel.Text = StatusLabelOpener + StatusLabelSuccess + StatusLabelWaiting;
            isProcessing = false;
        }

        #endregion

        private void clearSavedInfoButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.appName = GoogleSheetsManager.defaultAppName;
            Properties.Settings.Default.spreadsheetId = string.Empty;
            Properties.Settings.Default.startggAPIKey = string.Empty;
            Properties.Settings.Default.clientSecretPath = string.Empty;

            appNameTextBox.Text = GoogleSheetsManager.defaultAppName;
            sheetIdTextBox.Text = string.Empty;
            apiKeyTextBox.Text = string.Empty;
            clientSecretsPathTextBox.Text = string.Empty;

            Properties.Settings.Default.Save();
            MessageBox.Show("Cleared saved info.");
        }
    }
}
