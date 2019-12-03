using System;
using MingweiSamuel.Camille;
using MingweiSamuel.Camille.Enums;
using MingweiSamuel;
using System.Linq;
using System.Threading.Tasks;

namespace RiotAPIDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var riotApi = RiotApi.NewInstance("RGAPI-1936e2bc-7df2-44d3-bb23-64391ee10177");

            Console.WriteLine("Enter username: ");
            var summonerNameQuery = Console.ReadLine();


            var summonerData = await riotApi.SummonerV4.GetBySummonerNameAsync(Region.NA, summonerNameQuery);
            if (null == summonerData)
            {
                Console.WriteLine($"Summoner '{summonerNameQuery}' not found.");
                return;
            }

            Console.WriteLine($"Match history for {summonerData.Name}:");

            var matchlist = await riotApi.MatchV4.GetMatchlistAsync(
               Region.NA, summonerData.AccountId, queue: new[] { 420 }, endIndex: 5);

            var matchDataTasks = matchlist.Matches.Select(
                   matchMetadata => riotApi.MatchV4.GetMatchAsync(Region.NA, matchMetadata.GameId)
               ).ToArray();

            var matchDatas = await Task.WhenAll(matchDataTasks);

            for (var i = 0; i < matchDatas.Count(); i++)
            {
                var matchData = matchDatas[i];

                var participantIdData = matchData.ParticipantIdentities.First(pi => summonerData.Id.Equals(pi.Player.SummonerId));

                var participant = matchData.Participants.First(p => p.ParticipantId == participantIdData.ParticipantId);

                var win = participant.Stats.Win;
                var champ = (Champion)participant.ChampionId;
                var k = participant.Stats.Kills;
                var d = participant.Stats.Deaths;
                var a = participant.Stats.Assists;
                var c = (participant.Stats.TotalMinionsKilled + participant.Stats.NeutralMinionsKilled);
                var g = matchData.GameDuration;
                TimeSpan t = TimeSpan.FromSeconds(matchData.GameDuration);
                string gametime = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
                var kda = (k + a) / (float)d;
                var cspm = (c/(float)g)*60;
                

                Console.WriteLine("{0,3}) {1,-4} ({2}) Game Time: {3}", i + 1, win ? "Win" : "Loss", champ.Name(), gametime);

                Console.WriteLine("     K/D/A {0}/{1}/{2} ({3:0.00}) CS: {4} CSPM: {5:0.00}\n", k, d, a, kda, c, cspm);
            }
        }
    }
}

