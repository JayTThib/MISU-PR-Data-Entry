namespace MI_PR_Data_Entry
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.playerIdExecuteButton = new System.Windows.Forms.Button();
            this.eventSlugTextBox = new System.Windows.Forms.TextBox();
            this.eventSlugLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.userSlugTextBox = new System.Windows.Forms.TextBox();
            this.userSlugLabel = new System.Windows.Forms.Label();
            this.outputPlayerIdTextBox = new System.Windows.Forms.TextBox();
            this.outputPlayerIdLabel = new System.Windows.Forms.Label();
            this.modeGroupBox = new System.Windows.Forms.GroupBox();
            this.placementsModeRadioButton = new System.Windows.Forms.RadioButton();
            this.playerIdModeRadioButton = new System.Windows.Forms.RadioButton();
            this.apiKeyLabel = new System.Windows.Forms.Label();
            this.apiKeyTextBox = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.placementsAndRecordsExecuteButton = new System.Windows.Forms.Button();
            this.targetRecordsColumnLabel = new System.Windows.Forms.Label();
            this.targetRecordsColumnTextBox = new System.Windows.Forms.TextBox();
            this.clientSecretsPathTextBox = new System.Windows.Forms.TextBox();
            this.clientSecretsLabel = new System.Windows.Forms.Label();
            this.chooseClientSecretPathButton = new System.Windows.Forms.Button();
            this.viewInitialSetupInstructionsButton = new System.Windows.Forms.Button();
            this.sheetIdTextBox = new System.Windows.Forms.TextBox();
            this.sheetIdLabel = new System.Windows.Forms.Label();
            this.clearSavedInfoButton = new System.Windows.Forms.Button();
            this.viewOperationInstructionsButton = new System.Windows.Forms.Button();
            this.modeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // playerIdExecuteButton
            // 
            this.playerIdExecuteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.playerIdExecuteButton.Location = new System.Drawing.Point(374, 236);
            this.playerIdExecuteButton.Name = "playerIdExecuteButton";
            this.playerIdExecuteButton.Size = new System.Drawing.Size(75, 24);
            this.playerIdExecuteButton.TabIndex = 0;
            this.playerIdExecuteButton.Text = "Execute";
            this.playerIdExecuteButton.UseVisualStyleBackColor = true;
            this.playerIdExecuteButton.Click += new System.EventHandler(this.playerIdExecuteButton_Click);
            // 
            // eventSlugTextBox
            // 
            this.eventSlugTextBox.Location = new System.Drawing.Point(74, 237);
            this.eventSlugTextBox.Name = "eventSlugTextBox";
            this.eventSlugTextBox.Size = new System.Drawing.Size(294, 20);
            this.eventSlugTextBox.TabIndex = 1;
            // 
            // eventSlugLabel
            // 
            this.eventSlugLabel.AutoSize = true;
            this.eventSlugLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.eventSlugLabel.Location = new System.Drawing.Point(5, 240);
            this.eventSlugLabel.Name = "eventSlugLabel";
            this.eventSlugLabel.Size = new System.Drawing.Size(60, 13);
            this.eventSlugLabel.TabIndex = 2;
            this.eventSlugLabel.Text = "Event slug:";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.statusLabel.Location = new System.Drawing.Point(5, 216);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(51, 13);
            this.statusLabel.TabIndex = 3;
            this.statusLabel.Text = "Status: ";
            // 
            // userSlugTextBox
            // 
            this.userSlugTextBox.Location = new System.Drawing.Point(74, 237);
            this.userSlugTextBox.Name = "userSlugTextBox";
            this.userSlugTextBox.Size = new System.Drawing.Size(294, 20);
            this.userSlugTextBox.TabIndex = 7;
            // 
            // userSlugLabel
            // 
            this.userSlugLabel.AutoSize = true;
            this.userSlugLabel.Location = new System.Drawing.Point(9, 240);
            this.userSlugLabel.Name = "userSlugLabel";
            this.userSlugLabel.Size = new System.Drawing.Size(57, 13);
            this.userSlugLabel.TabIndex = 8;
            this.userSlugLabel.Text = "User slug: ";
            // 
            // outputPlayerIdTextBox
            // 
            this.outputPlayerIdTextBox.Location = new System.Drawing.Point(140, 263);
            this.outputPlayerIdTextBox.Name = "outputPlayerIdTextBox";
            this.outputPlayerIdTextBox.Size = new System.Drawing.Size(309, 20);
            this.outputPlayerIdTextBox.TabIndex = 9;
            // 
            // outputPlayerIdLabel
            // 
            this.outputPlayerIdLabel.AutoSize = true;
            this.outputPlayerIdLabel.Location = new System.Drawing.Point(9, 264);
            this.outputPlayerIdLabel.Name = "outputPlayerIdLabel";
            this.outputPlayerIdLabel.Size = new System.Drawing.Size(128, 13);
            this.outputPlayerIdLabel.TabIndex = 10;
            this.outputPlayerIdLabel.Text = "Output Start.gg Player ID:";
            // 
            // modeGroupBox
            // 
            this.modeGroupBox.Controls.Add(this.placementsModeRadioButton);
            this.modeGroupBox.Controls.Add(this.playerIdModeRadioButton);
            this.modeGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.modeGroupBox.Location = new System.Drawing.Point(9, 141);
            this.modeGroupBox.Name = "modeGroupBox";
            this.modeGroupBox.Size = new System.Drawing.Size(211, 72);
            this.modeGroupBox.TabIndex = 11;
            this.modeGroupBox.TabStop = false;
            this.modeGroupBox.Text = "Mode";
            // 
            // placementsModeRadioButton
            // 
            this.placementsModeRadioButton.AutoSize = true;
            this.placementsModeRadioButton.Location = new System.Drawing.Point(6, 42);
            this.placementsModeRadioButton.Name = "placementsModeRadioButton";
            this.placementsModeRadioButton.Size = new System.Drawing.Size(173, 17);
            this.placementsModeRadioButton.TabIndex = 2;
            this.placementsModeRadioButton.TabStop = true;
            this.placementsModeRadioButton.Text = "Output placements and records";
            this.placementsModeRadioButton.UseVisualStyleBackColor = true;
            this.placementsModeRadioButton.CheckedChanged += new System.EventHandler(this.placementsModeRadioButton_CheckedChanged);
            // 
            // playerIdModeRadioButton
            // 
            this.playerIdModeRadioButton.AutoSize = true;
            this.playerIdModeRadioButton.Location = new System.Drawing.Point(6, 19);
            this.playerIdModeRadioButton.Name = "playerIdModeRadioButton";
            this.playerIdModeRadioButton.Size = new System.Drawing.Size(128, 17);
            this.playerIdModeRadioButton.TabIndex = 0;
            this.playerIdModeRadioButton.TabStop = true;
            this.playerIdModeRadioButton.Text = "Get Start.gg Player ID";
            this.playerIdModeRadioButton.UseVisualStyleBackColor = true;
            this.playerIdModeRadioButton.CheckedChanged += new System.EventHandler(this.playerIdModeRadioButton_CheckedChanged);
            // 
            // apiKeyLabel
            // 
            this.apiKeyLabel.AutoSize = true;
            this.apiKeyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.apiKeyLabel.Location = new System.Drawing.Point(5, 95);
            this.apiKeyLabel.Name = "apiKeyLabel";
            this.apiKeyLabel.Size = new System.Drawing.Size(88, 13);
            this.apiKeyLabel.TabIndex = 13;
            this.apiKeyLabel.Text = "Start.gg API Key:";
            this.toolTip1.SetToolTip(this.apiKeyLabel, "Testing tool tip");
            // 
            // apiKeyTextBox
            // 
            this.apiKeyTextBox.Location = new System.Drawing.Point(99, 92);
            this.apiKeyTextBox.Name = "apiKeyTextBox";
            this.apiKeyTextBox.Size = new System.Drawing.Size(350, 20);
            this.apiKeyTextBox.TabIndex = 14;
            // 
            // placementsAndRecordsExecuteButton
            // 
            this.placementsAndRecordsExecuteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.placementsAndRecordsExecuteButton.Location = new System.Drawing.Point(374, 236);
            this.placementsAndRecordsExecuteButton.Name = "placementsAndRecordsExecuteButton";
            this.placementsAndRecordsExecuteButton.Size = new System.Drawing.Size(75, 24);
            this.placementsAndRecordsExecuteButton.TabIndex = 19;
            this.placementsAndRecordsExecuteButton.Text = "Execute";
            this.placementsAndRecordsExecuteButton.UseVisualStyleBackColor = true;
            this.placementsAndRecordsExecuteButton.Click += new System.EventHandler(this.placementsAndRecordsExecuteButton_Click);
            // 
            // targetRecordsColumnLabel
            // 
            this.targetRecordsColumnLabel.AutoSize = true;
            this.targetRecordsColumnLabel.Location = new System.Drawing.Point(5, 266);
            this.targetRecordsColumnLabel.Name = "targetRecordsColumnLabel";
            this.targetRecordsColumnLabel.Size = new System.Drawing.Size(333, 13);
            this.targetRecordsColumnLabel.TabIndex = 20;
            this.targetRecordsColumnLabel.Text = "Target Records sheet column (leave blank to use first empty column):";
            // 
            // targetRecordsColumnTextBox
            // 
            this.targetRecordsColumnTextBox.Location = new System.Drawing.Point(337, 263);
            this.targetRecordsColumnTextBox.Name = "targetRecordsColumnTextBox";
            this.targetRecordsColumnTextBox.Size = new System.Drawing.Size(112, 20);
            this.targetRecordsColumnTextBox.TabIndex = 21;
            // 
            // clientSecretsPathTextBox
            // 
            this.clientSecretsPathTextBox.Location = new System.Drawing.Point(126, 115);
            this.clientSecretsPathTextBox.Name = "clientSecretsPathTextBox";
            this.clientSecretsPathTextBox.Size = new System.Drawing.Size(242, 20);
            this.clientSecretsPathTextBox.TabIndex = 22;
            // 
            // clientSecretsLabel
            // 
            this.clientSecretsLabel.AutoSize = true;
            this.clientSecretsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.clientSecretsLabel.Location = new System.Drawing.Point(5, 118);
            this.clientSecretsLabel.Name = "clientSecretsLabel";
            this.clientSecretsLabel.Size = new System.Drawing.Size(115, 13);
            this.clientSecretsLabel.TabIndex = 23;
            this.clientSecretsLabel.Text = "client_secrets file path:";
            // 
            // chooseClientSecretPathButton
            // 
            this.chooseClientSecretPathButton.Location = new System.Drawing.Point(374, 114);
            this.chooseClientSecretPathButton.Name = "chooseClientSecretPathButton";
            this.chooseClientSecretPathButton.Size = new System.Drawing.Size(75, 24);
            this.chooseClientSecretPathButton.TabIndex = 24;
            this.chooseClientSecretPathButton.Text = "Select file";
            this.chooseClientSecretPathButton.UseVisualStyleBackColor = true;
            this.chooseClientSecretPathButton.Click += new System.EventHandler(this.chooseClientSecretPathButton_Click);
            // 
            // viewInitialSetupInstructionsButton
            // 
            this.viewInitialSetupInstructionsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.viewInitialSetupInstructionsButton.Location = new System.Drawing.Point(8, 2);
            this.viewInitialSetupInstructionsButton.Name = "viewInitialSetupInstructionsButton";
            this.viewInitialSetupInstructionsButton.Size = new System.Drawing.Size(180, 25);
            this.viewInitialSetupInstructionsButton.TabIndex = 25;
            this.viewInitialSetupInstructionsButton.Text = "View initial setup instructions";
            this.viewInitialSetupInstructionsButton.UseVisualStyleBackColor = true;
            this.viewInitialSetupInstructionsButton.Click += new System.EventHandler(this.viewInitialSetupInstructionsButton_Click);
            // 
            // sheetIdTextBox
            // 
            this.sheetIdTextBox.Location = new System.Drawing.Point(88, 70);
            this.sheetIdTextBox.Name = "sheetIdTextBox";
            this.sheetIdTextBox.Size = new System.Drawing.Size(361, 20);
            this.sheetIdTextBox.TabIndex = 26;
            // 
            // sheetIdLabel
            // 
            this.sheetIdLabel.AutoSize = true;
            this.sheetIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.sheetIdLabel.Location = new System.Drawing.Point(6, 70);
            this.sheetIdLabel.Name = "sheetIdLabel";
            this.sheetIdLabel.Size = new System.Drawing.Size(76, 13);
            this.sheetIdLabel.TabIndex = 27;
            this.sheetIdLabel.Text = "PR sheet link: ";
            // 
            // clearSavedInfoButton
            // 
            this.clearSavedInfoButton.Location = new System.Drawing.Point(279, 41);
            this.clearSavedInfoButton.Name = "clearSavedInfoButton";
            this.clearSavedInfoButton.Size = new System.Drawing.Size(108, 23);
            this.clearSavedInfoButton.TabIndex = 32;
            this.clearSavedInfoButton.Text = "Clear saved info";
            this.clearSavedInfoButton.UseVisualStyleBackColor = true;
            this.clearSavedInfoButton.Click += new System.EventHandler(this.clearSavedInfoButton_Click);
            // 
            // viewOperationInstructionsButton
            // 
            this.viewOperationInstructionsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.viewOperationInstructionsButton.Location = new System.Drawing.Point(8, 33);
            this.viewOperationInstructionsButton.Name = "viewOperationInstructionsButton";
            this.viewOperationInstructionsButton.Size = new System.Drawing.Size(180, 25);
            this.viewOperationInstructionsButton.TabIndex = 33;
            this.viewOperationInstructionsButton.Text = "View operation instructions";
            this.viewOperationInstructionsButton.UseVisualStyleBackColor = true;
            this.viewOperationInstructionsButton.Click += new System.EventHandler(this.viewOperationInstructionsButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 289);
            this.Controls.Add(this.viewOperationInstructionsButton);
            this.Controls.Add(this.clearSavedInfoButton);
            this.Controls.Add(this.sheetIdLabel);
            this.Controls.Add(this.sheetIdTextBox);
            this.Controls.Add(this.viewInitialSetupInstructionsButton);
            this.Controls.Add(this.chooseClientSecretPathButton);
            this.Controls.Add(this.clientSecretsLabel);
            this.Controls.Add(this.clientSecretsPathTextBox);
            this.Controls.Add(this.targetRecordsColumnTextBox);
            this.Controls.Add(this.targetRecordsColumnLabel);
            this.Controls.Add(this.placementsAndRecordsExecuteButton);
            this.Controls.Add(this.apiKeyTextBox);
            this.Controls.Add(this.apiKeyLabel);
            this.Controls.Add(this.modeGroupBox);
            this.Controls.Add(this.outputPlayerIdLabel);
            this.Controls.Add(this.outputPlayerIdTextBox);
            this.Controls.Add(this.userSlugLabel);
            this.Controls.Add(this.userSlugTextBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.eventSlugLabel);
            this.Controls.Add(this.eventSlugTextBox);
            this.Controls.Add(this.playerIdExecuteButton);
            this.Name = "MainForm";
            this.Text = "MISU PR Data Entry";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.modeGroupBox.ResumeLayout(false);
            this.modeGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button playerIdExecuteButton;
        private System.Windows.Forms.TextBox eventSlugTextBox;
        private System.Windows.Forms.Label eventSlugLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.TextBox userSlugTextBox;
        private System.Windows.Forms.Label userSlugLabel;
        private System.Windows.Forms.TextBox outputPlayerIdTextBox;
        private System.Windows.Forms.Label outputPlayerIdLabel;
        private System.Windows.Forms.GroupBox modeGroupBox;
        private System.Windows.Forms.RadioButton playerIdModeRadioButton;
        private System.Windows.Forms.RadioButton placementsModeRadioButton;
        private System.Windows.Forms.Label apiKeyLabel;
        private System.Windows.Forms.TextBox apiKeyTextBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button placementsAndRecordsExecuteButton;
        private System.Windows.Forms.Label targetRecordsColumnLabel;
        private System.Windows.Forms.TextBox targetRecordsColumnTextBox;
        private System.Windows.Forms.TextBox clientSecretsPathTextBox;
        private System.Windows.Forms.Label clientSecretsLabel;
        private System.Windows.Forms.Button chooseClientSecretPathButton;
        private System.Windows.Forms.Button viewInitialSetupInstructionsButton;
        private System.Windows.Forms.TextBox sheetIdTextBox;
        private System.Windows.Forms.Label sheetIdLabel;
        private System.Windows.Forms.Button clearSavedInfoButton;
        private System.Windows.Forms.Button viewOperationInstructionsButton;
    }
}

