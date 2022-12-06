using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MI_PR_Data_Entry
{
    public static class SheetSettings
    {
        #region Defaults
        public const string DefaultRecordsSheetDataColumnStart = "C";
        public const string DefaultRecordsSheetPlayerNameColumn = "A";
        public const int DefaultRecordsSheetTournamentNameRow = 1;
        public const int DefaultRecordsSheetDataRowStart = 4;
        public const int DefaultRecordsSheetColumnsToSkip = 2;

        public const int DefaultPlacementsSheetRowsToSkip = 3;
        public const int DefaultPlacementsColumnsToSkip = 3;

        public const string DefaultRecordsSheetName = "Records";
        public const string DefaultPlacementsSheetName = "Placements";

        public const int DefaultSpreadsheetPlayersNumOfColumnsToSkip = 1;
        public const int DefaultSpreadsheetPlayersNumOfRowsToSkip = 1;

        public const string DefaultUserSlugsColumn = "D";
        public const string DefaultStartggPlayerIdsColumn = "E";
        public const string DefaultSpreadsheetPlayerNameColumn = "B";
        #endregion

        public static string recordsSheetDataColumnStart;


        private static TextBox mainFormTargetRecordsColumnTextBox;

        public static void Init(TextBox mainFormTargetRecordsColumnTextBox)
        {
            SheetSettings.mainFormTargetRecordsColumnTextBox = mainFormTargetRecordsColumnTextBox;
        }
    }
}
