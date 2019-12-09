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
            var riotApi = RiotApi.NewInstance("RGAPI-393e05a1-d9ac-4320-8bfa-afd80a4d68d9");

            Console.WriteLine("Enter username: ");
            var summonerNameQuery = Console.ReadLine();


            // Get summoners data (blocking).
            var summonerData = await riotApi.SummonerV4.GetBySummonerNameAsync(Region.NA, summonerNameQuery);
            if (null == summonerData)
            {
                // If a summoner is not found, the response will be null.
                Console.WriteLine($"Summoner '{summonerNameQuery}' not found.");
                return;
            }

            Console.WriteLine($"Ranked Match history for {summonerData.Name}:");

            // Get 10 most recent matches (blocking).
            // Queue ID 420 is RANKED_SOLO_5v5 (TODO)
            var matchlist = await riotApi.MatchV4.GetMatchlistAsync(
               Region.NA, summonerData.AccountId, queue: new[] { 420 }, endIndex: 5);
            // Get match results (done asynchronously -> not blocking -> fast).
            var matchDataTasks = matchlist.Matches.Select(
                   matchMetadata => riotApi.MatchV4.GetMatchAsync(Region.NA, matchMetadata.GameId)
               ).ToArray();
            // Wait for all task requests to complete asynchronously.
            var matchDatas = await Task.WhenAll(matchDataTasks);

            for (var i = 0; i < matchDatas.Count(); i++)
            {
                var matchData = matchDatas[i];
                // Get this summoner's participant ID info.
                var participantIdData = matchData.ParticipantIdentities
                    .First(pi => summonerData.Id.Equals(pi.Player.SummonerId));
                // Find the corresponding participant.
                var participant = matchData.Participants
                    .First(p => p.ParticipantId == participantIdData.ParticipantId);

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
                

                // Print #, win/loss, champion.
                Console.WriteLine("{0,3}) {1,-4} ({2}) Game Time: {3}", i + 1, win ? "Win" : "Loss", champ.Name(), gametime);
                // Print champion, K/D/A
                Console.WriteLine("     K/D/A {0}/{1}/{2} ({3:0.00}) CS: {4} CSPM: {5:0.00}\n", k, d, a, kda, c, cspm);
            }
        }
        }
    }

            //string userinput;
            //Console.WriteLine("Enter a name: ");
            //userinput = Console.ReadLine();
            //var summoner = riotApi.SummonerV4.GetBySummonerName(Region.NA, $"{userinput}");
            //Console.WriteLine($"Name: {summoner.Name} Level: {summoner.SummonerLevel} AccountID: {summoner.AccountId}");

            //string userinput;
            //SummonerV4 summonerApi = new SummonerV4();
            //Console.WriteLine("Enter a name: ");
            //userinput = Console.ReadLine();
            //var summoner = summonerApi.GetSummonerByName($"{userinput}");
            //Console.WriteLine($"Name: {summoner.Name} Level: {summoner.Level}");