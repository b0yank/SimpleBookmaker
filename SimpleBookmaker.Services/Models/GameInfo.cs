namespace SimpleBookmaker.Services.Models
{
    using System.Collections.Generic;

    public class GameInfo
    {
        public int GameId { get; set; }

        public ICollection<int> HomeGoalscorers = new List<int>();
        public ICollection<int> AwayGoalscorers = new List<int>();

        public int HomeCorners { get; set; }
        public int AwayCorners { get; set; }

        public double HomePossession { get; set; }
        public double AwayPossession { get; set; }

        public int HomeFreeKicks { get; set; }
        public int AwayFreeKicks { get; set; }
    }
}
