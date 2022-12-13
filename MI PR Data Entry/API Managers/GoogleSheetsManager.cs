using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Google.Apis.Sheets.v4.Data;
using System.Linq;
using System.Windows.Forms;

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

        private enum CellSplitType { UserSlug, StartggPlayerID }

        //REORGANIZE LATER, AND MOVE SOME TO SETTINGS
        #region Fields / properties / constants 

        private static string recordsTargetColumn
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
        private const int recordsSheetColumnsToSkip = 2;
        private const int placementsSheetRowsToSkip = 3;
        private const int placementsColumnsToSkip = 3;
        private const string recordsSheetName = "Records";
        private const string placementsSheetName = "Placements";

        private const string entrantsLabelStarter = "Entrants: ";

        #endregion


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
            try
            {
                trackedPlayers = new List<TrackedPlayer>();
                Dictionary<string, IList<object>> playerDataDict = new Dictionary<string, IList<object>>()
                {
                    { SheetSettings.trackedPlayersPlayerNames, null },
                    { SheetSettings.userSlugs, null },
                    { SheetSettings.playerIds, null }
                };
                string[] indexesOfIdentifierTypes = playerDataDict.Keys.ToArray();

                //Grab tracked player data from the Google API.
                SpreadsheetsResource.ValuesResource.BatchGetRequest request = service.Spreadsheets.Values.BatchGet(spreadsheetId);
                request.Ranges = playerDataDict.Keys.ToArray();
                BatchGetValuesResponse response = request.Execute();

                if (response.ValueRanges == null || response.ValueRanges.Count == 0)
                {
                    throw new Exception("This program received a null response when trying to get named ranges for tracked players.");
                }

                int targetRowCount = response.ValueRanges[0].Values.Count;
                if (response.ValueRanges.Any(range => range.Values.Count != targetRowCount))
                {
                    throw new Exception("A named range on the " + SheetSettings.trackedPlayersSheetName + " sheet was found to have a row count that didn't match the other named ranges. Are there any rows with empty cells? Do any of the named ranges not extend far enough? On the sheet, click Data -> Named Ranges and check.\n\nNamed ranges involved: " + String.Join(", ", playerDataDict.Keys.ToArray()));
                }

                for (int i = 0; i < targetRowCount; i++)
                {
                    //Temporarily store specific player data from the response to the playerDataDict
                    for (int copyIndex = 0; copyIndex < indexesOfIdentifierTypes.Length; copyIndex++)
                    {
                        playerDataDict[indexesOfIdentifierTypes[copyIndex]] = response.ValueRanges[copyIndex].Values[i];
                    }
                    //Error check for merged or empty cells.
                    foreach (KeyValuePair<string, IList<object>> kvp in playerDataDict)
                    {
                        if (kvp.Value.Count == 0)
                        {
                            throw new Exception("A cell for the named range " + kvp.Key + " was empty. All of the tracked player info named ranges need to have the same amount of rows.\n\nNamed ranges involved: " + String.Join(", ", playerDataDict.Keys.ToArray()));
                        }
                        else if (kvp.Value.Count > 1)
                        {
                            throw new Exception("The named range " + kvp.Key + " probably includes a merged cell, when there shouldn't be any.");
                        }
                    }

                    const int FirstIndex = 0;
                    string playerName = playerDataDict[SheetSettings.trackedPlayersPlayerNames][FirstIndex].ToString();
                    string userSlugsCellValue = playerDataDict[SheetSettings.userSlugs][FirstIndex].ToString();
                    string playerIdsCellValue = playerDataDict[SheetSettings.playerIds][FirstIndex].ToString();

                    List<string> userSlugs = CellValueToList(userSlugsCellValue, CellSplitType.UserSlug);
                    List<string> playerIds = CellValueToList(playerIdsCellValue, CellSplitType.StartggPlayerID);

                    if (userSlugs.Count == playerIds.Count)
                    {
                        trackedPlayers.Add(new TrackedPlayer(playerName, userSlugs, playerIds));
                    }
                    else
                    {
                        throw new Exception("A player on the " + SheetSettings.trackedPlayersSheetName + " sheet was found to not have a matching amount of user slugs and player IDs. \n\nPlayer: " + playerName + "\nUser slug count: " + userSlugs.Count + "\nPlayer ID count: " + playerIds.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
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

        public static Task UploadGeneralTournamentInfo()
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

        public static Task SetTargetRecordsColumn()
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
                return Task.FromException(ex);
            }

            recordsTargetColumn = SheetRange.NumToLetters(targetColumn);
            placementsSheetTargetColumn = SheetRange.NumToLetters(SheetRange.LettersToNum(recordsTargetColumn) - recordsSheetColumnsToSkip + placementsColumnsToSkip);
            return Task.CompletedTask;
        }

        public static Task UploadRecords()
        {
            try
            {
                ValueRange targetValueRange = new ValueRange();
                targetValueRange.Values = new List<IList<object>>();
                targetValueRange.Range = recordsSheetName + "!" + recordsTargetColumn + recordsSheetDataRowStart.ToString() + ":" + recordsTargetColumn;

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
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }

            return Task.CompletedTask;
        }

        #region Filters

        private static List<string> CellValueToList(string cellValue, CellSplitType cellSplitType)
        {
            List<string> returnedList = new List<string>();
            string tempStor = string.Empty;

            for (int i = 0; i < cellValue.Length; i++)
            {
                if (Char.IsDigit(cellValue[i])
                 || (cellSplitType == CellSplitType.UserSlug && Char.IsLetter(cellValue[i])))
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

            string returnedString = string.Empty;
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
