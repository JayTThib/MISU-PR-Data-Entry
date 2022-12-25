using System;

namespace MI_PR_Data_Entry
{
    public struct SheetRange
    {
        public Cell rangeStart { get; set; }
        public Cell rangeEnd { get; set; }

        public string sheetName { get; set; }

        private const char RangeStartMarker = '!';
        private const char RangeMiddleMarker = ':';

        public SheetRange(string sheetName, Cell rangeStart, Cell rangeEnd)
        {
            this.sheetName = sheetName;
            this.rangeStart = rangeStart;
            this.rangeEnd = rangeEnd;
        }

        public SheetRange(string range)
        {
            this.sheetName = GetSheetNameFromRange(range);
            this.rangeStart = GetStartCellFromRange(range);
            this.rangeEnd = GetEndCellFromRange(range);
        }

        public static string GetSheetNameFromRange(string range)
        {
            return range.Substring(0, range.LastIndexOf(RangeStartMarker));
        }

        public static Cell GetStartCellFromRange(string range)
        {
            int cellStartIndex = range.LastIndexOf(RangeStartMarker) + 1;
            Cell cell = new Cell();
            string cellRow = string.Empty;

            for (int i = cellStartIndex; i < range.Length; i++)
            {
                if (Char.IsLetter(range[i]))
                {
                    cell.column += range[i];
                }
                else if (char.IsDigit(range[i]))
                {
                    cellRow += range[i];
                }
                else if (range[i] == RangeMiddleMarker)
                {
                    break;
                }
                else
                {
                    throw new Exception("Invalid char " + range[i] + " found inside of range: " + range);
                }
            }

            cell.row = Int32.Parse(cellRow);
            return cell;
        }

        public static Cell GetEndCellFromRange(string range)
        {
            int cellStartIndex = range.LastIndexOf(RangeMiddleMarker) + 1;
            Cell cell = new Cell();
            string cellRow = string.Empty;

            for (int i = cellStartIndex; i < range.Length; i++)
            {
                if (char.IsLetter(range[i]))
                {
                    cell.column += range[i];
                }
                else if (char.IsDigit(range[i]))
                {
                    cellRow += range[i];
                }
                else
                {
                    throw new Exception();
                }
            }

            cell.row = Int32.Parse(cellRow);
            return cell;
        }

        /// <summary>Formats the sheetName, rangeStart and rangeEnd into one string. Used specifically for requests to Google Sheets.</summary>
        public string GetFormattedRange()
        {
            return $"{sheetName}!{rangeStart}:{rangeEnd}";
        }

        public static string GetFormattedRange(string sheetName, string rangeStart, string rangeEnd)
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

        public static string ModifyColumnAsString(string column, int modifier)
        {
            return NumToLetters(LettersToNum(column) + modifier);
        }

        public static int ModifyColumnAsInt(string column, int modifier)
        {
            return LettersToNum(column) + modifier;
        }
        
        #endregion
    }
}
