namespace FootballManager.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using FootballManager.Data.Models;
    using FootballManager.Services.Data;
    using FootballManager.Web.ViewModels;
    using FootballManager.Web.ViewModels.Football;
    using FootballManager.Web.ViewModels.Teams;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IAddGames addGames;
        private readonly MatchService matchService;

        public HomeController(
                              IAddGames addGames,
                              MatchService matchService)
        {
            this.addGames = addGames;
            this.matchService = matchService;
        }

        public IActionResult Index()
        {
           // var addMatchesInfo = this.addGames.AddMatchesInfoAsync();
            //var addGame = this.addGames.AddAsync();
            return this.View();
        }

        public IActionResult Football()
        {
            var findedMatch = new MatchesViewModel
            {
                Matches = this.matchService.GetAllMatches<MatchViewModel>(),
            };
            return this.View(findedMatch);

        }

        public IActionResult BulgarianTeams()
        {
            var foundTeams = new BulgarianTeamsViewModel
            {
                BulgarianTeam = this.matchService.GetAllBulgarianTeams<BulgarianTeamViewModel>(),
            };

            return this.View(foundTeams);
        }

        public IActionResult SearchTeam(string name, int numberOfMatches)
        {
            this.ViewBag.Team = name;
            var foundTeam = new MatchesViewModel
            {
                Matches = this.matchService.SearchTeam<MatchViewModel>(name, numberOfMatches),
            };

            this.ViewBag.HomeWins = foundTeam.Matches.Where(x => x.HomeTeam == name && x.HomeTeamResult > x.AwayTeamResult).Count();
            this.ViewBag.AwayWins = foundTeam.Matches.Where(x => x.AwayTeam == name && x.HomeTeamResult < x.AwayTeamResult).Count();
            this.ViewBag.Draw = foundTeam.Matches.Where(x => x.HomeTeamResult == x.AwayTeamResult).Count();
            this.ViewBag.HomeLoose = foundTeam.Matches.Where(x => x.HomeTeam == name && x.HomeTeamResult < x.AwayTeamResult).Count();
            this.ViewBag.AwayLoose = foundTeam.Matches.Where(x => x.AwayTeam == name && x.HomeTeamResult > x.AwayTeamResult).Count();

            var test = 0;

            foreach (var item in foundTeam.Matches)
            {
                test += item.AwayTeamResult;
            }

            if (numberOfMatches != 0)
            {
                this.ViewBag.AwayTeamGoals = test / numberOfMatches;
            }

            return this.View(foundTeam);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
