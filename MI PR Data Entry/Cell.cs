namespace MI_PR_Data_Entry
{
    public struct Cell
    {
        public string column { get; set; }
        public int row { get; set; }


        public Cell(string column, int row)
        {
            this.column = column;
            this.row = row;
        }

        public override string ToString()
        {
            return column + row.ToString();
        }
    }
}
