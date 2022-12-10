using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Google.Apis.Sheets.v4.Data;
using System.Runtime.CompilerServices;

namespace MI_PR_Data_Entry
{
    public static class GoogleSheetsManager 
    {
        /* TODO
         * -Reorganize / remove some constants. Move stuff to settings class.
         * -Finish writing try catch statements and error checks / user feedback.
         * -For sheet requirements, write down that player names on the records sheet have to be merged cells (vertical size of 2)
         * -some functions assume that 5 is used for the start row (or... basically that it uses an odd num)
         */

        private enum FilterType { UserSlug, StartggPlayerID }

        //REORGANIZE LATER, AND MOVE SOME TO SETTINGS
        #region Fields / properties / constants 

        public static Dictionary<string, int> trackedPlayersRowOnRecordsSheet;

        public static string recordsTargetColumn
        {
            get { return MainForm.targetRecordsColumnTB.Text; }
            set { MainForm.targetRecordsColumnTB.Text = value; }
        }

        ///<summary>The part of the link to the spreadsheet between spreadsheets/d/ and /edit</summary>
        public static string spreadsheetId;
        public static List<TrackedPlayer> trackedPlayers;
        public static List<string> errorsList;
        public static string appName;
        public static string clientSecretsPath;

        private static int recordsSheetTournamentEntrantCountRow = 3;
        private static string placementsSheetTargetColumn;
        private static IEnumerable<string> Scopes = new string[] { SheetsService.Scope.Spreadsheets };
        private static SheetsService service;

        private static string tournamentName;
        private static string eventDate;
        private static int numEntrants;

        public const string defaultAppName = "MISU PR Data Entry";
        private const string recordsSheetDataColumnStart = "C";
        private const int recordsSheetTournamentNameRow = 1;
        private const int recordsSheetDataRowStart = 4;
        private const string recordsSheetPlayerNameColumn = "A";
        private const int recordsSheetColumnsToSkip = 2;
        private const int placementsSheetRowsToSkip = 3;
        private const int placementsColumnsToSkip = 3;
        private const string playersSheetName = "Spreadsheet Players";
        private const string recordsSheetName = "Records";
        private const string placementsSheetName = "Placements";
        private const int spreadsheetPlayersNumOfColumnsToSkip = 1;
        private const int spreadsheetPlayersNumOfRowsToSkip = 1;
        private const string userSlugsColumn = "D";
        private const string startggPlayerIdsColumn = "E";
        private const string spreadsheetPlayerNameColumn = "B";
        private const string entrantsLabelStarter = "Entrants: ";

        #endregion



        public static Task TestingNamedRanges()
        {
            try
            {
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, "RecordsEventNameStart");

                ValueRange response = request.Execute();

                IList<IList<object>> values = response.Values;

                if (values != null)
                {
                    if (values.Count > 0)
                    {
                        foreach (IList<object> objj in values)
                        {
                            Console.WriteLine("OBJJ - " + objj.ToString());
                            foreach (object temppp in objj)
                            {
                                Console.WriteLine("tempp - " + temppp.ToString());
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("count was 0");
                    }
                }
                else
                {
                    Console.WriteLine("values is null");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return Task.CompletedTask;
        }


        public static void SetGeneralTournamentInfo(TournamentResult tournamentResult)//rename? or rename the other method that sounds similar 
        {
            tournamentName = tournamentResult.tournamentName;
            eventDate = tournamentResult.eventDate;
            numEntrants = tournamentResult.numEntrants;
        }

        public static async Task SetupService()
        {
            try
            {
                UserCredential credential;
                using (var stream = new FileStream(clientSecretsPath, FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None);
                }

                service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = appName
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Task GetTrackedPlayerIdentifiers()
        {
            trackedPlayers = new List<TrackedPlayer>();
            SheetRange sheetRange = new SheetRange(playersSheetName,
                spreadsheetPlayerNameColumn + (spreadsheetPlayersNumOfRowsToSkip + 1),//Add 1 since spreadsheet rows start at 1 instead of 0
                startggPlayerIdsColumn);

            try
            {
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, sheetRange.GetFormattedRange());
                ValueRange response = request.Execute();
                IList<IList<object>> values = response.Values;

                if (values != null && values.Count > 0)
                {
                    foreach (IList<object> rowData in values)
                    {
                        const int indexMod = 1;//Use this to start at the first index (zero), since no extra data was grabbed.
                        string playerName = (string)rowData[SheetRange.ColumnSubAsInt(spreadsheetPlayerNameColumn, spreadsheetPlayersNumOfColumnsToSkip) - indexMod];
                        string userSlugsCellValue = (string)rowData[SheetRange.ColumnSubAsInt(userSlugsColumn, spreadsheetPlayersNumOfColumnsToSkip) - indexMod];
                        string playerIdsCellValue = (string)rowData[SheetRange.ColumnSubAsInt(startggPlayerIdsColumn, spreadsheetPlayersNumOfColumnsToSkip) - indexMod];

                        List<string> userSlugs = IdentifierFilter(userSlugsCellValue, FilterType.UserSlug);
                        List<string> playerIds = IdentifierFilter(playerIdsCellValue, FilterType.StartggPlayerID);

                        if (userSlugs.Count == playerIds.Count)
                        {
                            trackedPlayers.Add(new TrackedPlayer(playerName, userSlugs, playerIds));
                        }
                        else
                        {//TEMPORARY - WRITE WHY THIS FAILED
                            throw new Exception("");
                        }
                    }
                }
                else
                {//TEMPORARY - WRITE WHY THIS FAILED
                    throw new Exception("");
                }
            }
            catch (Exception ex) 
            {
                if (ex != null)
                {
                    throw ex;
                }

                throw new Exception("GENERIC ERROR\n\n" + ex.ToString());
            }

            return Task.CompletedTask;
        }

        public static Task UploadPlacements()
        {
            SheetRange sheetRange = new SheetRange(placementsSheetName,
                placementsSheetTargetColumn + (placementsSheetRowsToSkip + 1).ToString(),//Add one since spreadsheets start their rows at 1 instead of 0.
                placementsSheetTargetColumn + (trackedPlayers.Count + placementsSheetRowsToSkip).ToString());

            ValueRange targetRange = new ValueRange();
            targetRange.Range = sheetRange.GetFormattedRange();
            targetRange.Values = new List<IList<object>>();
            
            for (int i = 0; i < trackedPlayers.Count; i++)
            {
                if (trackedPlayers[i].tournamentResult != null)
                {
                    targetRange.Values.Add(new List<object>() { trackedPlayers[i].tournamentResult.placement.ToString() });
                }
                else
                {
                    targetRange.Values.Add(new List<object>() { string.Empty });
                }
            }

            SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest = service.Spreadsheets.Values.Update(targetRange, spreadsheetId, targetRange.Range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            UpdateValuesResponse updateResponse = updateRequest.Execute();
            return Task.CompletedTask;
        }

        public static async Task UploadRecords()//TEMP- just call each function from MainForm.placementsAndRecordsExecuteButton_Click()
        {
            if (recordsTargetColumn == string.Empty)
            {//TEMP- WRITE ERROR DETAILS
                return;
            }

            await UploadGeneralTournamentInfo();
            await UploadRecordsHelper_SetDict();

            if (trackedPlayersRowOnRecordsSheet == null)
            {//TEMP- WRITE ERROR DETAILS
                return;
            }

            UploadRecordsHelper_SetSheetValues(recordsTargetColumn, trackedPlayersRowOnRecordsSheet);
        }

        private static Task UploadGeneralTournamentInfo()
        {
            SheetRange sheetRange = new SheetRange(recordsSheetName,
                recordsTargetColumn + recordsSheetTournamentNameRow,
                recordsTargetColumn + recordsSheetTournamentEntrantCountRow);

            ValueRange targetRange = new ValueRange();
            targetRange.Range = sheetRange.GetFormattedRange();
            targetRange.Values = new List<IList<object>>();

            targetRange.Values.Add(new List<object>() { tournamentName });
            targetRange.Values.Add(new List<object>() { eventDate });
            targetRange.Values.Add(new List<object>() { entrantsLabelStarter + numEntrants.ToString() });

            SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest = service.Spreadsheets.Values.Update(targetRange, spreadsheetId, targetRange.Range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            UpdateValuesResponse updateResponse = updateRequest.Execute();
            return Task.CompletedTask;
        }

        public static Task UploadRecordsHelper_GetTargetColumn()
        {
            MainForm.targetRecordsColumnTB.Text = FilterTargetRecordsColumn(MainForm.targetRecordsColumnTB.Text);

            if (MainForm.targetRecordsColumnTB.Text != string.Empty)
            {
                recordsTargetColumn = MainForm.targetRecordsColumnTB.Text;
                placementsSheetTargetColumn = SheetRange.NumToLetters(SheetRange.LettersToNum(recordsTargetColumn) - recordsSheetColumnsToSkip + placementsColumnsToSkip);
                return Task.CompletedTask;
            }

            //Get the entire row containing tournament names on the Records sheet
            SheetRange sheetRange = new SheetRange(recordsSheetName,
                recordsSheetDataColumnStart + recordsSheetTournamentNameRow.ToString(),
                recordsSheetTournamentNameRow.ToString());

            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, sheetRange.GetFormattedRange());

            ValueRange response = request.Execute();
            IList<IList<object>> values = response.Values;

            int recordsSheetStart = SheetRange.LettersToNum(recordsSheetDataColumnStart);
            int targetColumn = recordsSheetStart;

            try
            {
                if (values != null)
                {//Set targetColumn to the first column that doesn't have a tournament name
                    for (int i = 0; i < values[0].Count; i++)
                    {
                        string cellValue = (string)values[0][i];

                        if (cellValue == string.Empty)
                        {
                            targetColumn = recordsSheetStart + i;
                            break;
                        }
                        else
                        {
                            targetColumn++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    throw ex;
                }

                throw new Exception("GENERIC ERROR\n\n" + ex.ToString());
            }

            recordsTargetColumn = SheetRange.NumToLetters(targetColumn);
            placementsSheetTargetColumn = SheetRange.NumToLetters(SheetRange.LettersToNum(recordsTargetColumn) - recordsSheetColumnsToSkip + placementsColumnsToSkip);
            return Task.CompletedTask;
        }

        private static Task UploadRecordsHelper_SetDict()
        {
            trackedPlayersRowOnRecordsSheet = new Dictionary<string, int>();

            //Get entire player name column on the Records sheet
            SheetRange sheetRange = new SheetRange(recordsSheetName,
                recordsSheetPlayerNameColumn + recordsSheetDataRowStart.ToString(),
                recordsSheetPlayerNameColumn);
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, sheetRange.GetFormattedRange());
            ValueRange response = request.Execute();
            
            IList<IList<object>> values = response.Values;
            
            if (values != null)
            {
                if (values.Count > 0)
                {
                    for (int i = 0; i < values.Count; i++)
                    {
                        if (values[i].Count == 1)
                        {
                            trackedPlayersRowOnRecordsSheet.Add(values[i][0].ToString(), recordsSheetDataRowStart + i);
                        }
                    }
                }
                else
                {
                    //TEMP- WRITE ERROR DETAILS
                    throw new Exception();
                }
            }
            else
            {
                //TEMP- WRITE ERROR DETAILS
                throw new Exception();
            }

            return Task.CompletedTask;
        }

        private static void UploadRecordsHelper_SetSheetValues(string targetTournamentColumn, Dictionary<string, int> kvp)
        {
            ValueRange targetValueRange = new ValueRange();
            targetValueRange.Values = new List<IList<object>>();
            targetValueRange.Range = recordsSheetName + "!" + targetTournamentColumn + recordsSheetDataRowStart.ToString() + ":" + targetTournamentColumn;

            int loopIndex = 0;
            foreach (TrackedPlayer trackedPlayer in trackedPlayers)
            {
                targetValueRange.Values.Add(new List<object>());
                targetValueRange.Values.Add(new List<object>());

                if (trackedPlayer.tournamentResult == null)
                {
                    targetValueRange.Values[loopIndex].Add(string.Empty);
                    targetValueRange.Values[loopIndex + 1].Add(string.Empty);
                }
                else
                {
                    targetValueRange.Values[loopIndex].Add(trackedPlayer.tournamentResult.wins);
                    targetValueRange.Values[loopIndex + 1].Add(trackedPlayer.tournamentResult.losses);
                }

                loopIndex += 2;
            }

            SpreadsheetsResource.ValuesResource.UpdateRequest request = service.Spreadsheets.Values.Update(targetValueRange, spreadsheetId, targetValueRange.Range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            UpdateValuesResponse response = request.Execute();
        }

        #region Filters

        private static List<string> IdentifierFilter(string cellValue, FilterType filterType)
        {
            List<string> returnedList = new List<string>();
            string tempStor = string.Empty;

            for (int i = 0; i < cellValue.Length; i++)
            {
                if (Char.IsDigit(cellValue[i])
                 || (filterType == FilterType.UserSlug && Char.IsLetter(cellValue[i])))
                {
                    tempStor += cellValue[i];
                }
                else if (tempStor != string.Empty)
                {
                    returnedList.Add(tempStor);
                    tempStor = string.Empty;
                }
            }

            if (tempStor != string.Empty)
            {
                returnedList.Add(tempStor);
            }

            return returnedList;
        }

        private static string FilterTargetRecordsColumn(string target)
        {
            if (target == string.Empty)
            {
                return string.Empty;
            }

            string returnedString = "";
            for (int i = 0; i < target.Length; i++)
            {
                if (Char.IsLetter(target[i]))
                {
                    returnedString += target[i];
                }
            }

            return target.ToUpper();
        }

        #endregion
    }
}
