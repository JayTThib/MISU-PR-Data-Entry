namespace MI_PR_Data_Entry.Forms
{
    partial class OperationInstructionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperationInstructionsForm));
            this.operationInstructionsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // operationInstructionsLabel
            // 
            this.operationInstructionsLabel.Location = new System.Drawing.Point(12, 9);
            this.operationInstructionsLabel.Name = "operationInstructionsLabel";
            this.operationInstructionsLabel.Size = new System.Drawing.Size(643, 317);
            this.operationInstructionsLabel.TabIndex = 0;
            this.operationInstructionsLabel.Text = resources.GetString("operationInstructionsLabel.Text");
            // 
            // OperationInstructionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 326);
            this.Controls.Add(this.operationInstructionsLabel);
            this.Name = "OperationInstructionsForm";
            this.Text = "Operation Instructions";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label operationInstructionsLabel;
    }
}