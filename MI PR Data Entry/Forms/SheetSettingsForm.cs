using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MI_PR_Data_Entry
{
    public partial class SheetSettingsForm : Form
    {
        public SheetSettingsForm()
        {
            InitializeComponent();
        }

        private void recordsSheetDataColStartUseDefaultButton_Click(object sender, EventArgs e)
        {
            recordsSheetDataColStartTextBox.Text = SheetSettings.DefaultRecordsSheetDataColumnStart;

        }

        private void recordsSheetPlayerNameColUseDefaultButton_Click(object sender, EventArgs e)
        {

        }

        private void recordsSheetTournNameRowUseDefaultButton_Click(object sender, EventArgs e)
        {

        }

        public void SetControlsEnabledStatus(bool status)
        {

        }
    }
}
