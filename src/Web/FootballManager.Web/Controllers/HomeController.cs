namespace FootballManager.Web.Controllers
{
    using System.Diagnostics;
    using FootballManager.Services.Data;
    using FootballManager.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IAddGames addGames;

        public HomeController(IAddGames addGames)
        {
            this.addGames = addGames;
        }

        public IActionResult Index()
        {
            //var addGame = this.addGames.AddAsync();
            return this.View();
        }

        public IActionResult Football()
        {
            return this.View();
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
