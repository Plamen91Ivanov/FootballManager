using AngleSharp.Html.Parser;
using FootballManager.Data.Common.Repositories;
using FootballManager.Data.Models;
using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FootballManager.Services.Data
{
    public class AddGames : IAddGames
    {
        private readonly IDeletableEntityRepository<FootballManager.Data.Models.Match> gameRepository;
        private readonly IDeletableEntityRepository<MatchesInfo> matchRepository;

        public AddGames(
                        IDeletableEntityRepository<FootballManager.Data.Models.Match> gameRepository,
                        IDeletableEntityRepository<MatchesInfo> matchRepository)
        {
            this.gameRepository = gameRepository;
            this.matchRepository = matchRepository;
        }

        public async Task<int> AddMatchesInfoAsync()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var parser = new HtmlParser();
            var webClient = new WebClient { Encoding = Encoding.GetEncoding("windows-1251") };

            for (var i = 1; i <= 2; i++)
            {
                var url = $"https://www.futbol24.com/national/Bulgaria/A-Grupa/2020-2021/results/?statLR-Page={i}";
                string html = null;
                for (var j = 0; j < 20; j++)
                {
                    try
                    {
                        html = webClient.DownloadString(url);
                        break;
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(1000);
                    }
                }

                if (string.IsNullOrWhiteSpace(html))
                {
                    continue;
                }

                var document = parser.ParseDocument(html);
                var ResultTable = document.GetElementsByClassName("stat2");
                var test = ResultTable.Length;

                foreach (var tr in ResultTable)
                {
                    var DateTime = tr.GetElementsByClassName("timezonebar").Length;
                    var MatchCount = tr.GetElementsByClassName("status5").Length;

                    for (int n = 0; n < MatchCount; n++)
                    {
                        var Match = tr.GetElementsByClassName("status5")[n];
                        var MatchDate = Match.GetElementsByClassName("timezone")[0].OuterHtml.Split("title");

                        var DateSplit = MatchDate[1].Split("=");
                        var DateSplitBySpace = DateSplit[1].Split(' ');
                        var DateReplace = Regex.Replace(DateSplitBySpace[0], @"[^0-9:,]+", " ");
                        var Date = Convert.ToDateTime(DateReplace, new CultureInfo("fr-FR"));
                        var League = Match.GetElementsByClassName("comp")[0].TextContent;
                        var HomeTeam = Match.GetElementsByClassName("team4")[0].TextContent;
                        var Result = Match.GetElementsByClassName("dash")[0].TextContent.Split('-');
                        var HomeTeamResult = int.Parse(Result[0]);
                        var AwayTeamResult = int.Parse(Result[1]);
                        var AwayTeam = Match.GetElementsByClassName("team5")[0].TextContent;
                        var winner = "draw";

                        if (HomeTeamResult > AwayTeamResult)
                        {
                            winner = HomeTeam;
                        }
                        else if (HomeTeamResult < AwayTeamResult)
                        {
                            winner = AwayTeam;
                        }

                        var addMatchesInfo = new MatchesInfo
                        {
                            HomeTeam = HomeTeam,
                            AwayTeam = AwayTeam,
                            HomeTeamResult = HomeTeamResult,
                            AwayTeamResult = AwayTeamResult,
                            Winner = winner,
                            Date = Date,
                        };

                        await this.matchRepository.AddAsync(addMatchesInfo);
                    }
                }
            }

            await this.gameRepository.SaveChangesAsync();

            return 1;
        }

        public async Task<int> AddAsync()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var parser = new HtmlParser();
            var webClient = new WebClient { Encoding = Encoding.GetEncoding("windows-1251") };

            for (var i = 1; i <= 1; i++)
            {
                var url = $"https://www.futbol24.com/national/Bulgaria/A-Grupa/2020-2021/results/?statLR-Page=1";
                string html = null;
                for (var j = 0; j < 20; j++)
                {
                    try
                    {
                        html = webClient.DownloadString(url);
                        break;
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(1000);
                    }
                }

                if (string.IsNullOrWhiteSpace(html))
                {
                    continue;
                }

                var document = parser.ParseDocument(html);
                var ResultTable = document.GetElementsByClassName("stat2");
                var test = ResultTable.Length;

                foreach (var tr in ResultTable)
                {
                    var DateTime = tr.GetElementsByClassName("timezonebar").Length;
                    var MatchCount = tr.GetElementsByClassName("status5").Length;

                    for (int n = 0; n < MatchCount; n++)
                    {
                        var Match = tr.GetElementsByClassName("status5")[n];
                        var MatchDate = Match.GetElementsByClassName("timezone")[0].OuterHtml.Split("title");

                        var DateSplit = MatchDate[1].Split("=");
                        var DateSplitBySpace = DateSplit[1].Split(' ');
                        var DateReplace = Regex.Replace(DateSplitBySpace[0], @"[^0-9:,]+", " ");
                        var Date = Convert.ToDateTime(DateReplace, new CultureInfo("fr-FR"));
                        var League = Match.GetElementsByClassName("comp")[0].TextContent;
                        var HomeTeam = Match.GetElementsByClassName("team4")[0].TextContent;
                        var Result = Match.GetElementsByClassName("dash")[0].TextContent.Split('-');
                        var HomeTeamResult = int.Parse(Result[0]);
                        var AwayTeamResult = int.Parse(Result[1]);
                        var AwayTeam = Match.GetElementsByClassName("team5")[0].TextContent;

                        var addMatch = new FootballManager.Data.Models.Match
                        {
                            HomeTeam = HomeTeam,
                            AwayTeam = AwayTeam,
                            HomeTeamResult = HomeTeamResult,
                            AwayTeamResult = AwayTeamResult,
                            Date = Date,
                        };

                        await this.gameRepository.AddAsync(addMatch);
                    }
                }

                await this.gameRepository.SaveChangesAsync();
            }

            return 1;
        }
    }
}
