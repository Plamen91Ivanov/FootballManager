namespace FootballManager.Data.Models
{
    using FootballManager.Data.Common.Models;

    public class Games : BaseDeletableModel<int>
    {
        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public int HomeTeamResult { get; set; }

        public int AwayTeamResult { get; set; }

        public double HomeTeamCoefficient { get; set; }

        public double AwayTeamCoefficient { get; set; }
    }
}
