using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using MI_PR_Data_Entry.StartggObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MI_PR_Data_Entry
{
    public static class StartggManager
    {
        #region Fields / properties / constants

        public const string disqualifiedDisplayScore = "DQ";

        public static TournamentResult tournamentResult;
        public static string outputPlayerId;
        public static int? targetPlayerId;
        public static string eventSlug;
        public static string startggApiKey;
        public static Dictionary<int, TournamentResult> trackedPlayerResults;

        private static GraphQLHttpClient graphQLClient;

        #endregion


        #region Queries

        public static async Task QueryForPlayerId(string targetUserSlug)
        {
            GraphQLClientSetup();

            GraphQLRequest query = new GraphQLRequest
            {
                Query = @"
                    query GetPlayerIdFromSlug($usrSlg:String!)
                    {
	                    user(slug:$usrSlg) 
                        {
                            player
                            {
                                gamerTag
                                id
                            }
                        }
                    }",
                Variables = new
                {
                    usrSlg = targetUserSlug
                }
            };

            GraphQLResponse<RootObjPlayerId> response = await graphQLClient.SendQueryAsync<RootObjPlayerId>(query);

            try
            {
                outputPlayerId = response.Data.user.player.id.ToString();
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    throw ex;
                }

                throw new Exception("GENERIC ERROR\n\n" + ex.ToString());
            }
        }

        private static async Task QueryForPlayerResult(string targetPlayerId, TrackedPlayer trackedPlayer)
        {
            int targetIdAsInt = Int32.Parse(targetPlayerId);

            GraphQLRequest query = new GraphQLRequest
            {
                Query = @"
                        query Test($plyrId:ID!, $evntSlg:String!)
                        {
	                        tournamentEvent:event(slug:$evntSlg)
                            {
                                startAt
                                numEntrants
                                tournament
                                {
                                    name
                                }
                                sets(page:1, perPage:500, sortType:RECENT, filters:{playerIds:[$plyrId]})
                                {
                                    nodes
                                    {
                                        displayScore
                                        slots
                                        {
                                            entrant
                                            {
                                                id
                                                standing
                                                {
                                                    player
                                                    {
                                                        gamerTag
                                                        id
                                                    }
                                                    placement
                                                }
                                            }
                                        }
                                        winnerId
                                    }
                                }
                            }
                        }",

                Variables = new
                {
                    plyrId = targetPlayerId,
                    evntSlg = eventSlug
                }
            };

            GraphQLResponse<RootObjPlacement> response = await graphQLClient.SendQueryAsync<RootObjPlacement>(query);

            try
            {
                if (response.Data != null)
                {//If the player didn't enter, nodes.Length will equal 0.
                    if (response.Data.tournamentEvent.sets.nodes.Length == 0)
                    {
                        return;
                    }

                    trackedPlayer.tournamentResult = new TournamentResult();
                    TournamentResult tResult = trackedPlayer.tournamentResult;

                    tResult.numEntrants = response.Data.tournamentEvent.numEntrants;
                    tResult.eventDate = GetTournamentDate(response.Data.tournamentEvent.startAt);
                    tResult.tournamentName = response.Data.tournamentEvent.tournament.name;

                    List<string> winsList = new List<string>();
                    List<string> lossesList = new List<string>();

                    #region Set the player's final placement for the event, and startggPlayerId used for that event
                    Node tempNode = response.Data.tournamentEvent.sets.nodes[0];
                    if (tempNode.slots[0].entrant.standing.player.id == targetIdAsInt)
                    {
                        tResult.placement = tempNode.slots[0].entrant.standing.placement;
                        tResult.startggPlayerId = tempNode.slots[0].entrant.standing.player.id.ToString();
                    }
                    else
                    {
                        tResult.placement = tempNode.slots[1].entrant.standing.placement;
                        tResult.startggPlayerId = tempNode.slots[1].entrant.standing.player.id.ToString();
                    }
                    #endregion

                    #region Get win / loss data for each set 
                    foreach (Node eventSet in response.Data.tournamentEvent.sets.nodes)
                    {
                        Slot targetPlayerSlot, otherSlot;
                        if (eventSet.slots[0].entrant.standing.player.id == targetIdAsInt)
                        {
                            targetPlayerSlot = eventSet.slots[0];
                            otherSlot = eventSet.slots[1];
                        }
                        else
                        {
                            targetPlayerSlot = eventSet.slots[1];
                            otherSlot = eventSet.slots[0];
                        }

                        if (eventSet.winnerId == targetPlayerSlot.entrant.id)
                        {
                            if (!eventSet.SomeoneWasDQd())
                            {
                                winsList.Add(otherSlot.entrant.standing.player.gamerTag);
                            }
                        }
                        else
                        {
                            if (eventSet.SomeoneWasDQd())
                            {
                                lossesList.Add(disqualifiedDisplayScore);
                            }
                            else
                            {
                                lossesList.Add(otherSlot.entrant.standing.player.gamerTag);
                            }
                        }
                    }
                    #endregion

                    tResult.wins = string.Join(", ", winsList);
                    tResult.losses = string.Join(", ", lossesList);
                }
                else
                {//finish this later
                    throw new Exception("undetermined error");
                }
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    throw ex;
                }

                throw new Exception("GENERIC ERROR\n\n" + ex.ToString());
            }
        }

        #endregion

        #region Misc functions

        public static void GraphQLClientSetup()
        {
            const string authScheme = "Bearer";
            const string startggEndPoint = "https://api.smash.gg/gql/alpha";

            graphQLClient = new GraphQLHttpClient(startggEndPoint, new NewtonsoftJsonSerializer());
            graphQLClient.HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authScheme, startggApiKey);
        }

        private static string GetTournamentDate(double unixTimeStamp)//Not my code, taken from: https://stackoverflow.com/a/250400
        {// Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime.ToShortDateString();
        }

        public static async Task SetTournamentResultForTrackedPlayers()
        {
            GraphQLClientSetup();

            bool setTournamentInfo = false;
            foreach (TrackedPlayer trackedPlayer in GoogleSheetsManager.trackedPlayers)
            {
                foreach (string pId in trackedPlayer.startggPlayerIds)
                {
                    await QueryForPlayerResult(pId, trackedPlayer);

                    if (trackedPlayer.tournamentResult != null)
                    {
                        break;
                    }
                }

                if (trackedPlayer.tournamentResult != null && !setTournamentInfo)
                {
                    GoogleSheetsManager.SetGeneralTournamentInfo(trackedPlayer.tournamentResult);
                    setTournamentInfo = true;
                }
            }
        }

        #endregion
    }
}
