using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MI_PR_Data_Entry
{
    public partial class SheetSettingsForm : Form
    {
        /* 
         * 
         * 
         */

        /// <summary></summary>
        private static class MessageSections
        {
            //Sheet name starters
            public const string UpdateSheetMessageStart = "Updated the saved sheet name for: ";
            public const string DefaultedSheetMessageStart = "Reset the saved sheet name for: ";
            //Sheet name enders
            public const string RecordsSheetEnd = "Records";
            public const string PlacementsSheetEnd = "Placements";
            public const string TrackedPlayersSheetEnd = "Tracked Players";
            //Named range starters
            public const string UpdateNamedRangeMessageStart = "Updated the saved name for the named range: ";
            public const string DefaultedNamedRangeMessageStart = "Reset the saved name for the named range: ";
            //Named range enders
            public const string RecordsPlayerNamesEnd = "Records sheet, player names starting range";
            public const string RecordsPlayerDataEnd = "Records sheet, player data starting range";
            public const string EventGeneralInfoEnd = "Records sheet, general event info starting range";
            public const string TrackedPlayerPlayerNamesEnd = "Tracked players sheet, player names starting range";
            public const string UserSlugsEnd = "Tracked players sheet, user slugs starting range";
            public const string PlayerIdsEnd = "Tracked players sheet, player IDs starting range";
            public const string PlacementsDataEnd = "Placements sheet, placement data starting range";
        }

        public SheetSettingsForm()
        {
            InitializeComponent();
            InitTextBoxes();
        }

        #region Misc functions
        /// <summary>Match every text box to the data from Properties.Settings.Default.</summary>
        private void InitTextBoxes()
        {
            //Sheet names
            recordsSheetNameTextBox.Text = Properties.Settings.Default.recordsSheetName;
            placementsSheetNameTextBox.Text = Properties.Settings.Default.placementsSheetName;
            trackedPlayersSheetNameTextBox.Text = Properties.Settings.Default.trackedPlayersSheetName;
            //Named ranges
            recordsPlayerNamesTextBox.Text = Properties.Settings.Default.recordsPlayerNames;
            recordsPlayerDataTextBox.Text = Properties.Settings.Default.recordsPlayerData;
            eventGeneralInfoTextBox.Text = Properties.Settings.Default.eventGeneralInfo;
            trackedPlayersPlayerNamesTextBox.Text = Properties.Settings.Default.trackedPlayersPlayerNames;
            userSlugsTextBox.Text = Properties.Settings.Default.userSlugs;
            playerIdsTextBox.Text = Properties.Settings.Default.playerIds;
            placementsDataTextBox.Text = Properties.Settings.Default.placementsData;
        }

        
        public void SetControlsEnabledStatus(bool status)
        {
            throw new Exception("NOT DONE");
        }

        #endregion

        #region Sheet name related functions
        private void updateRecordsSheetNameButton_Click(object sender, EventArgs e)
        {
            SheetSettings.recordsSheetName = recordsSheetNameTextBox.Text;
            MessageBox.Show(MessageSections.UpdateSheetMessageStart + MessageSections.RecordsSheetEnd);
        }
        private void useDefaultRecordsSheetNameButton_Click(object sender, EventArgs e)
        {
            SheetSettings.recordsSheetName = SheetSettings.DefaultSheetSettings.RecordsSheetName;
            recordsSheetNameTextBox.Text = SheetSettings.DefaultSheetSettings.RecordsSheetName;
            MessageBox.Show(MessageSections.DefaultedNamedRangeMessageStart + MessageSections.RecordsSheetEnd);
        }

        private void updatePlacementsSheetNameButton_Click(object sender, EventArgs e)
        {
            SheetSettings.placementsSheetName = placementsSheetNameTextBox.Text;
            MessageBox.Show(MessageSections.UpdateSheetMessageStart + MessageSections.PlacementsSheetEnd);
        }
        private void useDefaultPlacementsSheetNameButton_Click(object sender, EventArgs e)
        {
            SheetSettings.placementsSheetName = SheetSettings.DefaultSheetSettings.PlacementsSheetName;
            placementsSheetNameTextBox.Text = SheetSettings.DefaultSheetSettings.PlacementsSheetName;
            MessageBox.Show(MessageSections.DefaultedSheetMessageStart + MessageSections.PlacementsSheetEnd);
        }

        private void updateTrackedPlayersSheetNameButton_Click(object sender, EventArgs e)
        {
            SheetSettings.trackedPlayersSheetName = trackedPlayersSheetNameTextBox.Text;
            MessageBox.Show(MessageSections.UpdateSheetMessageStart + MessageSections.TrackedPlayersSheetEnd);
        }
        private void useDefaultTrackedPlayersSheetNameButton_Click(object sender, EventArgs e)
        {
            SheetSettings.trackedPlayersSheetName = SheetSettings.DefaultSheetSettings.TrackedPlayersSheetName;
            trackedPlayersSheetNameTextBox.Text = SheetSettings.DefaultSheetSettings.TrackedPlayersSheetName;
            MessageBox.Show(MessageSections.DefaultedSheetMessageStart + MessageSections.TrackedPlayersSheetEnd);
        }
        #endregion

        #region Named ranges related functions
        private void updateRecordsPlayerNamesButton_Click(object sender, EventArgs e)
        {
            SheetSettings.recordsPlayerNames = recordsPlayerNamesTextBox.Text;
            MessageBox.Show(MessageSections.UpdateNamedRangeMessageStart + MessageSections.RecordsPlayerNamesEnd);
        }
        private void useDefaultRecordsPlayerNamesButton_Click(object sender, EventArgs e)
        {
            SheetSettings.recordsPlayerNames = SheetSettings.DefaultSheetSettings.RecordsPlayerNames;
            recordsPlayerNamesTextBox.Text = SheetSettings.DefaultSheetSettings.RecordsPlayerNames;
            MessageBox.Show(MessageSections.DefaultedNamedRangeMessageStart + MessageSections.RecordsPlayerNamesEnd);
        }

        private void updateRecordsPlayerDataButton_Click(object sender, EventArgs e)
        {
            SheetSettings.recordsPlayerData = recordsPlayerDataTextBox.Text;
            MessageBox.Show(MessageSections.UpdateNamedRangeMessageStart + MessageSections.RecordsPlayerDataEnd);
        }
        private void useDefaultRecordsPlayerDataButton_Click(object sender, EventArgs e)
        {
            SheetSettings.recordsPlayerData = SheetSettings.DefaultSheetSettings.RecordsPlayerData;
            recordsPlayerDataTextBox.Text = SheetSettings.DefaultSheetSettings.RecordsPlayerData;
            MessageBox.Show(MessageSections.DefaultedNamedRangeMessageStart + MessageSections.RecordsPlayerDataEnd);
        }

        private void updateEventGeneralInfoButton_Click(object sender, EventArgs e)
        {
            SheetSettings.eventGeneralInfo = eventGeneralInfoTextBox.Text;
            MessageBox.Show(MessageSections.UpdateNamedRangeMessageStart + MessageSections.EventGeneralInfoEnd);
        }
        private void useDefaultEventGeneralInfoButton_Click(object sender, EventArgs e)
        {
            SheetSettings.eventGeneralInfo = SheetSettings.DefaultSheetSettings.EventGeneralInfo;
            eventGeneralInfoTextBox.Text = SheetSettings.DefaultSheetSettings.EventGeneralInfo;
            MessageBox.Show(MessageSections.DefaultedNamedRangeMessageStart + MessageSections.EventGeneralInfoEnd);
        }

        private void updateTrackedPlayersPlayerNamesButton_Click(object sender, EventArgs e)
        {
            SheetSettings.trackedPlayersPlayerNames = trackedPlayersPlayerNamesTextBox.Text;
            MessageBox.Show(MessageSections.UpdateNamedRangeMessageStart + MessageSections.TrackedPlayerPlayerNamesEnd);
        }
        private void useDefaultTrackedPlayersPlayerNamesButton_Click(object sender, EventArgs e)
        {
            SheetSettings.trackedPlayersPlayerNames = SheetSettings.DefaultSheetSettings.TrackedPlayersPlayerNames;
            trackedPlayersPlayerNamesTextBox.Text = SheetSettings.DefaultSheetSettings.TrackedPlayersPlayerNames;
            MessageBox.Show(MessageSections.DefaultedNamedRangeMessageStart + MessageSections.TrackedPlayerPlayerNamesEnd);
        }

        private void updateUserSlugsButton_Click(object sender, EventArgs e)
        {
            SheetSettings.userSlugs = userSlugsTextBox.Text;
            MessageBox.Show(MessageSections.UpdateNamedRangeMessageStart + MessageSections.UserSlugsEnd);
        }
        private void useDefaultUserSlugsButton_Click(object sender, EventArgs e)
        {
            SheetSettings.userSlugs = SheetSettings.DefaultSheetSettings.UserSlugs;
            userSlugsTextBox.Text = SheetSettings.DefaultSheetSettings.UserSlugs;
            MessageBox.Show(MessageSections.DefaultedNamedRangeMessageStart + MessageSections.UserSlugsEnd);
        }

        private void updatePlayerIdsButton_Click(object sender, EventArgs e)
        {
            SheetSettings.playerIds = playerIdsTextBox.Text;
            MessageBox.Show(MessageSections.UpdateNamedRangeMessageStart + MessageSections.PlayerIdsEnd);
        }
        private void useDefaultPlayerIdsButton_Click(object sender, EventArgs e)
        {
            SheetSettings.playerIds = SheetSettings.DefaultSheetSettings.PlayerIds;
            playerIdsTextBox.Text = SheetSettings.DefaultSheetSettings.PlayerIds;
            MessageBox.Show(MessageSections.DefaultedNamedRangeMessageStart + MessageSections.PlayerIdsEnd);
        }

        private void updatePlacementsDataButton_Click(object sender, EventArgs e)
        {
            SheetSettings.placementsData = placementsDataTextBox.Text;
            MessageBox.Show(MessageSections.UpdateNamedRangeMessageStart + MessageSections.PlacementsDataEnd);
        }
        private void useDefaultPlacementsDataButton_Click(object sender, EventArgs e)
        {
            SheetSettings.placementsData = SheetSettings.DefaultSheetSettings.PlacementsData;
            placementsDataTextBox.Text = SheetSettings.DefaultSheetSettings.PlacementsData;
            MessageBox.Show(MessageSections.DefaultedNamedRangeMessageStart + MessageSections.PlacementsDataEnd);
        }
        #endregion
    }
}
