using System;
using System.Windows.Forms;

namespace MI_PR_Data_Entry
{
    public static class SheetSettings
    {
        public static class DefaultSheetSettings
        {
            public const string RecordsSheetName = "Records";
            public const string PlacementsSheetName = "Placements";
            public const string TrackedPlayersSheetName = "Tracked Players";

            public const string RecordsPlayerNames = "DATAAPP_RecordsPlayerNames";
            public const string RecordsPlayerData = "DATAAPP_PlayerData";
            public const string EventGeneralInfo = "DATAAPP_EventGeneralInfo";

            public const string TrackedPlayersPlayerNames = "DATAAPP_TrackedPlayersPlayerNames";
            public const string UserSlugs = "DATAAPP_UserSlugs";
            public const string PlayerIds = "DATAAPP_PlayerIDs";

            public const string PlacementsData = "DATAAPP_PlacementsData";
        }

        #region Sheet name fields
        public static string recordsSheetName
        {
            get { return _recordsSheetName; }
            set
            {
                if (InputIsValid(value))
                {
                    _recordsSheetName = value;
                    Properties.Settings.Default.recordsSheetName = value;
                }
            }
        }
        private static string _recordsSheetName;

        public static string placementsSheetName
        {
            get { return _placementsSheetName; }
            set
            {
                if (InputIsValid(value))
                {
                    _placementsSheetName = value;
                    Properties.Settings.Default.placementsSheetName = value;
                }
            }
        }
        private static string _placementsSheetName;


        public static string trackedPlayersSheetName
        {
            get { return _spreadsheetPlayersSheetName; }
            set
            {
                if (InputIsValid(value))
                {
                    _spreadsheetPlayersSheetName = value;
                    Properties.Settings.Default.trackedPlayersSheetName = value;
                }
            }
        }
        private static string _spreadsheetPlayersSheetName;
        #endregion

        #region Named range fields
        public static string recordsPlayerNames
        {
            get { return _recordsPlayerNames; }
            set
            {
                if (InputIsValid(value))
                {
                    _recordsPlayerNames = value;
                    Properties.Settings.Default.recordsPlayerNames = value;
                }
            }
        }
        private static string _recordsPlayerNames;

        public static string recordsPlayerData
        {
            get { return _recordsPlayerData; }
            set
            {
                if (InputIsValid(value))
                {
                    _recordsPlayerData = value;
                    Properties.Settings.Default.recordsPlayerData = value;
                }
            }
        }
        private static string _recordsPlayerData;

        public static string eventGeneralInfo
        {
            get { return _eventGeneralInfo; }
            set
            {
                if (InputIsValid(value))
                {
                    _eventGeneralInfo = value;
                    Properties.Settings.Default.eventGeneralInfo = value;
                }
            }
        }
        private static string _eventGeneralInfo;

        public static string trackedPlayersPlayerNames
        {
            get { return _trackedPlayersPlayerNames; }
            set
            {
                if (InputIsValid(value))
                {
                    _trackedPlayersPlayerNames = value;
                    Properties.Settings.Default.trackedPlayersPlayerNames = value;
                }
            }
        }
        private static string _trackedPlayersPlayerNames;

        public static string userSlugs
        {
            get { return _userSlugs; }
            set
            {
                if (InputIsValid(value))
                {
                    _userSlugs = value;
                    Properties.Settings.Default.userSlugs = value;
                }
            }
        }
        private static string _userSlugs;

        public static string playerIds
        {
            get { return _userIds; }
            set
            {
                if (InputIsValid(value))
                {
                    _userIds = value;
                    Properties.Settings.Default.playerIds = value;
                }
            }
        }
        private static string _userIds;

        public static string placementsData
        {
            get { return _placementsData; }
            set
            {
                if (InputIsValid(value))
                {
                    _placementsData = value;
                    Properties.Settings.Default.placementsData = value;
                }
            }
        }
        private static string _placementsData;
        #endregion

        
        private static bool InputIsValid(string input)
        {
            if (input == string.Empty)
            {
                MessageBox.Show("A blank space isn't allowed as input.");
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void LoadSavedSettings()
        {
            //Sheet names
            recordsSheetName = Properties.Settings.Default.recordsSheetName == string.Empty ? DefaultSheetSettings.RecordsSheetName : Properties.Settings.Default.recordsSheetName;
            placementsSheetName = Properties.Settings.Default.placementsSheetName == string.Empty ? DefaultSheetSettings.PlacementsSheetName : Properties.Settings.Default.placementsSheetName;
            trackedPlayersSheetName = Properties.Settings.Default.trackedPlayersSheetName == string.Empty ? DefaultSheetSettings.TrackedPlayersSheetName : Properties.Settings.Default.trackedPlayersSheetName;
            //Named ranges
            recordsPlayerNames = Properties.Settings.Default.recordsPlayerNames == string.Empty ? DefaultSheetSettings.RecordsPlayerNames : Properties.Settings.Default.recordsPlayerNames;
            recordsPlayerData = Properties.Settings.Default.recordsPlayerData == string.Empty ? DefaultSheetSettings.RecordsPlayerData : Properties.Settings.Default.recordsPlayerData;
            eventGeneralInfo = Properties.Settings.Default.eventGeneralInfo == string.Empty ? DefaultSheetSettings.EventGeneralInfo : Properties.Settings.Default.eventGeneralInfo;
            trackedPlayersPlayerNames = Properties.Settings.Default.trackedPlayersPlayerNames == string.Empty ? DefaultSheetSettings.TrackedPlayersPlayerNames : Properties.Settings.Default.trackedPlayersPlayerNames;
            userSlugs = Properties.Settings.Default.userSlugs == string.Empty ? DefaultSheetSettings.UserSlugs : Properties.Settings.Default.userSlugs;
            playerIds = Properties.Settings.Default.playerIds == string.Empty ? DefaultSheetSettings.PlayerIds : Properties.Settings.Default.playerIds;
            placementsData = Properties.Settings.Default.placementsData == string.Empty ? DefaultSheetSettings.PlacementsData : Properties.Settings.Default.placementsData;
        }
    }
}
