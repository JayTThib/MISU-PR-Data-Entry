using MI_PR_Data_Entry.StartggObjects.Other;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MI_PR_Data_Entry.StartggObjects
{//Alias for "event" since that's a reserved keyword.
    public class TournamentEvent
    {
        public double startAt { get; set; }
        public int numEntrants { get; set; }
        public Sets sets { get; set; }
        public Tournament tournament { get; set; }
    }
}
