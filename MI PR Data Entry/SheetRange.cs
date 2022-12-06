using System;

namespace MI_PR_Data_Entry
{
    public struct SheetRange
    {
        public string rangeStart { get; set; }
        public string rangeEnd { get; set; }
        public string sheetName { get; set; }


        public SheetRange(string sheetName, string rangeStart, string rangeEnd)
        {
            this.sheetName = sheetName;
            this.rangeStart = rangeStart;
            this.rangeEnd = rangeEnd;
        }


        /// <summary>Formats the sheetName, rangeStart and rangeEnd into one string. Used specifically for requests to Google Sheets.</summary>
        public string GetFormattedRange()
        {
            return $"{sheetName}!{rangeStart}:{rangeEnd}";
        }

        public static string GetColumnFromCell(string cellRange)
        {
            if (string.IsNullOrEmpty(cellRange))
            {
                return "A";
            }

            string temp = string.Empty;
            for (int i = 0; i < cellRange.Length; i++)
            {
                if (Char.IsLetter(cellRange[i]))
                {
                    temp += cellRange[i];
                }
            }

            return temp;
        }

        public static string GetRowFromCell(string cellRange)
        {
            if (string.IsNullOrEmpty(cellRange))
            {
                return "1";
            }

            string temp = string.Empty;
            for (int i = 0; i < cellRange.Length; i++)
            {
                if (!Char.IsLetter(cellRange[i]))
                {
                    temp += cellRange[i];
                }
            }

            return temp;
        }

        #region Character conversion

        public static string NumToLetters(int index) 
        {
            index -= 1;
            const byte baseByte = 'Z' - 'A' + 1;
            string letters = string.Empty; 

            do 
            {
                letters = Convert.ToChar('A' + index % baseByte) + letters; 
                index = index / baseByte - 1; 
            } while (index >= 0); 
            
            return letters; 
        } 

        public static int LettersToNum(string letters)
        {
            letters = letters.ToUpper();
            int column = 0;

            for (int i = letters.Length - 1; i >= 0; i--)
            {

                column += (letters[i] - 64) * (int)Math.Pow(26, letters.Length - (i + 1));
            }

            return column;
        }

        #endregion

        #region Column math

        public static string ColumnAddAsStr(string column, int addition)
        {
            return NumToLetters(LettersToNum(column) + addition);
        }

        public static string ColumnSubAsStr(string column, int subtraction)
        {
            return NumToLetters(LettersToNum(column) - subtraction);
        }

        public static int ColumnAddAsInt(string column, int addition)
        {
            return LettersToNum(column) + addition;
        }

        public static int ColumnSubAsInt(string column, int subtraction)
        {
            return LettersToNum(column) - subtraction;
        }

        #endregion
    }
}
