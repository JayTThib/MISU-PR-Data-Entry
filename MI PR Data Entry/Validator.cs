using System;
using System.IO;
using System.Windows.Forms;

namespace MI_PR_Data_Entry
{/// <summary></summary>
    public static class Validator
    {
        public static bool InvalidPrSheetLink(TextBox sheetLinkTextBox)
        {
            const string TargetD = "/d/";
            const char TargetSlash = '/';

            if (sheetLinkTextBox.Text == string.Empty)
            {
                if (Properties.Settings.Default.spreadsheetId == string.Empty)
                {
                    MainForm.errorMessage = "Enter the full link to the current PR sheet";
                    return true;
                }

                sheetLinkTextBox.Text = Properties.Settings.Default.spreadsheetId;
            }

            string tempString = sheetLinkTextBox.Text;
            int strIndex = tempString.LastIndexOf(TargetD) + TargetD.Length;

            if (strIndex < 0)
            {
                MainForm.errorMessage = "Invalid link entered for the PR sheet. Enter the full link.\n\nCouldn't find '" + TargetD + "'";
                return true;
            }

            tempString = tempString.Substring(strIndex);
            strIndex = tempString.IndexOf(TargetSlash);

            if (strIndex < 0)
            {
                MainForm.errorMessage = "Invalid link entered for the PR sheet. Enter the full link.\n\nCouldn't find another '" + TargetSlash + "' after '" + TargetD + "'";
                return true;
            }
            
            GoogleSheetsManager.spreadsheetId = tempString.Split(TargetSlash)[0];
            Properties.Settings.Default.spreadsheetId = sheetLinkTextBox.Text;
            return false;
        }

        public static bool InvalidStartggApiKey(TextBox apiKeyTextBox)
        {
            if (apiKeyTextBox.Text == string.Empty)
            {
                if (Properties.Settings.Default.startggAPIKey == string.Empty)
                {
                    MainForm.errorMessage = "Blank text entered for the API key, and no API key was previously saved.\n\nGet your key by following the steps at- https://developer.start.gg/docs/authentication";
                    return true;
                }

                apiKeyTextBox.Text = Properties.Settings.Default.startggAPIKey;
                StartggManager.startggApiKey = Properties.Settings.Default.startggAPIKey;

                if (FoundCharThatWasntDigitOrLetter(apiKeyTextBox.Text))
                {
                    MainForm.errorMessage = "Blank text entered for the API key, so a previously saved key was used instead. However, an invalid character was found. Re-enter your Start.gg API key and make sure it's only letters and numbers.";
                    return true;
                }

                return false;
            }
            
            if (FoundCharThatWasntDigitOrLetter(apiKeyTextBox.Text))
            {
                MainForm.errorMessage = "An invalid character was found within the Start.gg API key. Re-enter your Start.gg API key, and make sure that it only contains letters and numbers.";
                return true;
            }

            Properties.Settings.Default.startggAPIKey = apiKeyTextBox.Text;
            StartggManager.startggApiKey = apiKeyTextBox.Text;
            return false;
        }

        private static bool FoundCharThatWasntDigitOrLetter(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!char.IsLetterOrDigit(str[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool UserSlugIsInvalid(TextBox userSlugTextBox)
        {
            if (userSlugTextBox.Text == string.Empty)
            {
                MainForm.errorMessage = "Empty text entered for the user slug. Enter a valid slug.";
                return true;
            }

            const string UserTarget = "/user/";
            int userStartIndex = userSlugTextBox.Text.IndexOf(UserTarget);

            if (userStartIndex < 0)
            {
                for (int i = 0; i < userSlugTextBox.Text.Length; i++)
                {
                    if (!Char.IsLetterOrDigit(userSlugTextBox.Text[i]))
                    {
                        MainForm.errorMessage = "Couldn't determine if the user slug was posted by itself or if the full profile link was posted. Just post the full profile link instead.";
                        return true;
                    }
                }

                return false;
            }
            else
            {
                userStartIndex += UserTarget.Length;
                int endIndex = userSlugTextBox.Text.IndexOf("/", userStartIndex);

                if (endIndex < 0)
                {
                    for (int i = 0; i < userSlugTextBox.Text.Length - userStartIndex; i++)
                    {
                        if (!Char.IsLetterOrDigit(userSlugTextBox.Text[userStartIndex + i]))
                        {
                            MainForm.errorMessage = "Found an invalid character in the user slug after /user/. Either post the user slug by itself, or the full profile link.";
                            return true;
                        }
                    }

                    userSlugTextBox.Text = userSlugTextBox.Text.Substring(userStartIndex);
                    return false;
                }
                else
                {
                    userSlugTextBox.Text = userSlugTextBox.Text.Substring(userStartIndex, endIndex - userStartIndex);
                    return false;
                }
            }
        }

        public static bool EventSlugIsInvalid(string eventSlugText)
        {
            const char TargetSlash = '/';
            const string TournamentSubStr = "tournament/";
            const string EventSubStr = "/event/";
            const string EventLinkExample = "https://www.start.gg/tournament/bahamut-ann-arbor-regional-1/event/ultimate-singles/standings";

            if (eventSlugText == string.Empty)
            {
                MessageBox.Show("ERROR\n\nThe event link text box is blank.");
                return true;
            }

            int tournamentStartIndex = eventSlugText.IndexOf(TournamentSubStr);

            if (tournamentStartIndex < 0)
            {
                MainForm.errorMessage = "ERROR: Couldn't find '" + TournamentSubStr + "' inside of the event link.\n\nEnter a full link to the event, such as: " + EventLinkExample;
                return true;
            }

            int eventEndIndex = eventSlugText.IndexOf(EventSubStr) + EventSubStr.Length;

            if (eventEndIndex - EventSubStr.Length < 0)
            {
                MainForm.errorMessage = "ERROR: Couldn't find '" + EventSubStr + "' inside of the event link.\n\nEnter a full link to the event, such as: " + EventLinkExample;
                return true;
            }

            int eventNameLength = 0;
            for (int i = eventEndIndex; i < eventSlugText.Length; i++)
            {
                if (eventSlugText[i] == TargetSlash)
                {
                    break;
                }

                eventNameLength++;
            }

            StartggManager.eventSlug = eventSlugText.Substring(tournamentStartIndex, eventEndIndex - tournamentStartIndex + eventNameLength);
            return false;
        }

        public static bool ClientSecretFileDoesntExist(string path)
        {
            if (path == string.Empty)
            {
                if (Properties.Settings.Default.clientSecretPath == string.Empty)
                {
                    MainForm.errorMessage = "Provide a file path for client_secret.json";
                    return true;
                }
            }

            if (File.Exists(path))
            {
                GoogleSheetsManager.clientSecretsPath = path;
                Properties.Settings.Default.clientSecretPath = path;
                return false;
            }

            MainForm.errorMessage = "client_secret.json was unable to be found at: \" + path";
            return true;
        }

        public static bool PlayerIdIsInvalid(string targetPlayerIdText)
        {
            if (targetPlayerIdText != string.Empty)
            {
                try
                {
                    StartggManager.targetPlayerId = Int32.Parse(targetPlayerIdText);
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: Something went wrong with the player ID.\n\nError details: " + ex.Message);
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Please enter a Start.gg player ID.");
                return true;
            }
        }
    }
}
