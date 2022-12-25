using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Google.Apis.Sheets.v4.Data;
using System.Linq;

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

        private static class SheetNames
        {
            public const string TrackedPlayers = "Tracked Players";
            public const string Placements = "Placements";
            public const string Records = "Records";
        }

        private enum NamedRangeType
        {
            RecordsPlayerNames,
            PlayerRecords,
            EventGeneralInfo,
            TrackedPlayersPlayerNames,
            UserSlugs,
            PlayerIds,
            PlacementsData
        }

        private enum CellSplitType { UserSlug, StartggPlayerID }

        /// <summary></summary>
        private static readonly Dictionary<NamedRangeType, string> rangeNames = new Dictionary<NamedRangeType, string>()
        {
            { NamedRangeType.RecordsPlayerNames, "DATAAPP_RecordsPlayerNames"},
            { NamedRangeType.PlayerRecords, "DATAAPP_PlayerRecords"},
            { NamedRangeType.EventGeneralInfo, "DATAAPP_EventGeneralInfo"},
            { NamedRangeType.TrackedPlayersPlayerNames, "DATAAPP_TrackedPlayersPlayerNames"},
            { NamedRangeType.UserSlugs, "DATAAPP_UserSlugs"},
            { NamedRangeType.PlayerIds, "DATAAPP_PlayerIDs"},
            { NamedRangeType.PlacementsData, "DATAAPP_PlacementsData"}
        };

        private static Dictionary<NamedRangeType, ValueRange> retrievedSheetValueRanges;

        #region Fields / properties / constants 
        ///<summary>The part of the link to the spreadsheet between spreadsheets/d/ and /edit</summary>
        public static string spreadsheetId;
        private static SheetsService service;
        public static List<TrackedPlayer> trackedPlayers;
        public static string clientSecretsPath;

        private static string recordsTargetColumn
        {
            get { return MainForm.targetRecordsColumnTB.Text; }
            set { MainForm.targetRecordsColumnTB.Text = value; }
        }
        
        private static string placementsSheetTargetColumn;
        private static string tournamentName;
        private static string eventDate;
        private static int numEntrants;
        #endregion


        public static void SetGeneralTournamentInfo(TournamentResult tournamentResult)//rename? or rename the other method that sounds similar 
        {
            tournamentName = tournamentResult.tournamentName;
            eventDate = tournamentResult.eventDate;
            numEntrants = tournamentResult.numEntrants;
        }

        private static async Task SetupService()
        {
            try
            {
                UserCredential credential;
                using (var stream = new FileStream(clientSecretsPath, FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        new string[] { SheetsService.Scope.Spreadsheets },
                        "user",
                        CancellationToken.None);
                }

                service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "MISU PR Data Entry"
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static Task SheetDataErrorCheck()
        {
            try
            {
                int playerNamesRowCount = retrievedSheetValueRanges[NamedRangeType.TrackedPlayersPlayerNames].Values.Count;
                int userSlugsRowCount = retrievedSheetValueRanges[NamedRangeType.UserSlugs].Values.Count;
                int playerIdsRowCount = retrievedSheetValueRanges[NamedRangeType.PlayerIds].Values.Count;

                if (playerNamesRowCount != userSlugsRowCount || playerNamesRowCount != playerIdsRowCount)
                {
                    throw new Exception("The named ranges on the " + SheetNames.TrackedPlayers + " sheet don't have a matching amount of data. Make sure that there isn't a blank cell inside of a named range, when the other named ranges on the same row are filled out. Also, none of the cells for the named ranges are supposed to be merged." +
                        "\n\nNamed ranges involved:\n" + rangeNames[NamedRangeType.TrackedPlayersPlayerNames] + " row count: " + playerNamesRowCount + "\n" + rangeNames[NamedRangeType.UserSlugs] + " row count: " + userSlugsRowCount + "\n" + rangeNames[NamedRangeType.PlayerIds] + " row count: " + playerIdsRowCount);
                }

                //need more stuff here, like checking for merged cells or blank spaces? idk
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }

            return Task.CompletedTask;
        }

        private static Task SetTrackedPlayers()
        {
            try
            {
                trackedPlayers = new List<TrackedPlayer>();

                for (int i = 0; i < retrievedSheetValueRanges[NamedRangeType.TrackedPlayersPlayerNames].Values.Count; i++)
                {
                    const int FirstIndex = 0;
                    string pNameCellValue = retrievedSheetValueRanges[NamedRangeType.TrackedPlayersPlayerNames].Values[i][FirstIndex].ToString();
                    List<string> userSlugs = CellValueToList(retrievedSheetValueRanges[NamedRangeType.UserSlugs].Values[i][FirstIndex].ToString(), CellSplitType.UserSlug);
                    List<string> playerIds = CellValueToList(retrievedSheetValueRanges[NamedRangeType.PlayerIds].Values[i][FirstIndex].ToString(), CellSplitType.StartggPlayerID);
                    
                    if (userSlugs.Count == playerIds.Count)
                    {
                        trackedPlayers.Add(new TrackedPlayer(pNameCellValue, userSlugs, playerIds));
                    }
                    else
                    {
                        throw new Exception("A player on the " + SheetNames.TrackedPlayers + " sheet was found to not have a matching amount of user slugs and player IDs.\n\nPlayer: " + pNameCellValue + "\nUser slugs count: " + userSlugs.Count + "\nPlayer IDs count: " + playerIds.Count);
                    }
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        public static async Task<Task> ProcessEventData()
        {
            Dictionary<Func<Task>, string> funcAndStatusDict = new Dictionary<Func<Task>, string>()
            {
                { async() => { await SetupService(); }, "Setting up connection to Google Sheets"},
                { async() => { await GetNamedRanges(); }, "Getting named ranges from Google Sheets" },
                { async() => { await SheetDataErrorCheck(); }, "Checking named ranges for errors" },
                { async() => { await SetTrackedPlayers(); }, "Getting tracked player identifiers from Google Sheets"},
                { async() => { await StartggManager.SetTournamentResultForTrackedPlayers(); }, "Getting tournament results from Start.gg"},
                { async() => { await SetTargetRecordsColumn(); }, "Getting target spreadsheet column"},
                { async() => { await SetPlacementsTargetColumn(); }, "Calculating column offset between " + SheetNames.Records + " and " + SheetNames.Placements },
                { async() => { await UploadGeneralTournamentInfo(); }, "Uploading general event info to Google Sheets" },
                { async() => { await UploadRecords(); }, "Uploading records to Google Sheets" },
                { async() => { await UploadPlacements(); }, "Uploading placements to Google Sheets"}
            };

            foreach (KeyValuePair<Func<Task>, string> kvp in funcAndStatusDict)
            {
                MainForm.status.Text = kvp.Value;
                await kvp.Key.Invoke();
            }

            return Task.CompletedTask;
        }

        private static Task GetNamedRanges()
        {
            try
            {
                SpreadsheetsResource.ValuesResource.BatchGetRequest request = service.Spreadsheets.Values.BatchGet(spreadsheetId);
                request.Ranges = rangeNames.Values.ToArray();
                BatchGetValuesResponse response = request.Execute();
                retrievedSheetValueRanges = new Dictionary<NamedRangeType, ValueRange>();

                int responseIndex = 0;
                foreach (KeyValuePair<NamedRangeType, string> kvp in rangeNames)
                {
                    retrievedSheetValueRanges.Add(kvp.Key, response.ValueRanges[responseIndex]);
                    responseIndex++;
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        private static Task SetPlacementsTargetColumn()
        {
            try
            {
                Cell recordsDataStartRange = SheetRange.GetStartCellFromRange(retrievedSheetValueRanges[NamedRangeType.EventGeneralInfo].Range);
                Cell placementsDataRange = SheetRange.GetStartCellFromRange(retrievedSheetValueRanges[NamedRangeType.PlacementsData].Range);
                int placementsDataColumnOffset = SheetRange.LettersToNum(placementsDataRange.column) - SheetRange.LettersToNum(recordsDataStartRange.column);
                placementsSheetTargetColumn = SheetRange.ModifyColumnAsString(recordsTargetColumn, placementsDataColumnOffset);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        private static Task SetTargetRecordsColumn()
        {
            try
            {
                recordsTargetColumn = FilterTargetRecordsColumn(recordsTargetColumn);

                if (recordsTargetColumn != string.Empty)
                {
                    return Task.CompletedTask;
                }
                else if (retrievedSheetValueRanges[NamedRangeType.EventGeneralInfo].Values == null)
                {
                    recordsTargetColumn = SheetRange.GetStartCellFromRange(retrievedSheetValueRanges[NamedRangeType.EventGeneralInfo].Range).column;
                    return Task.CompletedTask;
                }
                else
                {
                    int targetIndex = 0;
                    IList<object> eventGeneralInfoRange = retrievedSheetValueRanges[NamedRangeType.EventGeneralInfo].Values[0];

                    while (targetIndex < eventGeneralInfoRange.Count)
                    {
                        if ((string)eventGeneralInfoRange[targetIndex] == string.Empty)
                        {
                            break;
                        }

                        targetIndex++;
                    }

                    Cell startCell = SheetRange.GetStartCellFromRange(retrievedSheetValueRanges[NamedRangeType.EventGeneralInfo].Range);
                    recordsTargetColumn = SheetRange.ModifyColumnAsString(startCell.column, targetIndex);
                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        #region Functions for uploading data
        private static Task UploadGeneralTournamentInfo()
        {
            try
            {
                const string EntrantsLabelStart = "Entrants: ";
                const int RowOffset = 2;//Skip down two rows since the event date and entrant count are right below the event name.

                Cell targetStartCell = new Cell();
                targetStartCell.column = recordsTargetColumn;
                targetStartCell.row = SheetRange.GetStartCellFromRange(retrievedSheetValueRanges[NamedRangeType.EventGeneralInfo].Range).row;

                ValueRange targetRange = new ValueRange();
                targetRange.Range = new SheetRange(SheetNames.Records, targetStartCell, new Cell(targetStartCell.column, targetStartCell.row + RowOffset)).GetFormattedRange();
                targetRange.Values = new List<IList<object>>()
                {
                    { new List<object>(){ tournamentName } },
                    { new List<object>(){ eventDate } },
                    { new List<object>(){ EntrantsLabelStart + numEntrants.ToString() } }
                };

                SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest = service.Spreadsheets.Values.Update(targetRange, spreadsheetId, targetRange.Range);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                updateRequest.Execute();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        private static Task UploadRecords()
        {
            try
            {
                ValueRange targetValueRange = new ValueRange();
                targetValueRange.Values = new List<IList<object>>();
                int recordsSheetDataRowStart = SheetRange.GetStartCellFromRange(retrievedSheetValueRanges[NamedRangeType.PlayerRecords].Range).row;
                targetValueRange.Range = SheetRange.GetFormattedRange(SheetNames.Records, recordsTargetColumn + recordsSheetDataRowStart.ToString(), recordsTargetColumn);

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
                request.Execute();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        private static Task UploadPlacements()
        {
            try
            {
                int startRow = SheetRange.GetStartCellFromRange(retrievedSheetValueRanges[NamedRangeType.PlacementsData].Range).row;
                SheetRange sheetRange = new SheetRange(SheetNames.Placements,
                    new Cell(placementsSheetTargetColumn, startRow),
                    new Cell(placementsSheetTargetColumn, trackedPlayers.Count + startRow - 1));

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
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }
        #endregion

        #region Filter functions
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
