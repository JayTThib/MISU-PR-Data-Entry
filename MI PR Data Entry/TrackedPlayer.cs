using System.Collections.Generic;

namespace MI_PR_Data_Entry
{
    public class TrackedPlayer
    {
        //userSlugs and startggPlayerIds are meant to match.

        public string playerName { get; private set; }
        public List<string> userSlugs { get; private set; }
        public List<string> startggPlayerIds { get; private set; }

        public TournamentResult tournamentResult { get; set; }

        public TrackedPlayer(string playerName, List<string> userSlugs, List<string> startggPlayerIds)
        {
            this.playerName = playerName;
            this.userSlugs = userSlugs;
            this.startggPlayerIds = startggPlayerIds;
        }
    }
}
