namespace MI_PR_Data_Entry.StartggObjects
{
    public class Node
    {
        /// <summary>
        /// This string gets formatted as "winnerSponsor | winnerGamerTag gamesWon - loserSponsor | loserGamerTag gamesLost"
        /// <para> If a player wins by DQ, then it'll just literally be "DQ"</para>
        /// </summary>
        public string displayScore { get; set; }

        public Slot[] slots { get; set; }
        public int winnerId { get; set; }


        public bool SomeoneWasDQd()
        {
            return displayScore == StartggManager.disqualifiedDisplayScore ? true : false;
        }
    }
}
