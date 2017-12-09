namespace SimpleBookmaker.Services.Infrastructure.EventStats
{
    using System.Collections.Generic;

    public class GameStats
    {
        public GameStats(int gameId,
            int homeCorners,
            int awayCorners,
            int homeFreeKicks,
            int awayFreeKicks,
            int homePossession,
            int awayPossession,
            IEnumerable<int> homeGoalscorers,
            IEnumerable<int> awayGoalscorers)
        {
            this.HomeCorners = homeCorners;
            this.AwayCorners = awayCorners;

            this.HomeFreeKicks = homeFreeKicks;
            this.AwayFreeKicks = awayFreeKicks;

            this.HomePossession = homePossession;
            this.AwayPossession = awayPossession;

            this.HomeGoalscorers = homeGoalscorers;
            this.AwayGoalscorers = awayGoalscorers;
        }

        public int GameId{ get; set; }

        public int HomeCorners { get; private set; }
        public int AwayCorners { get; private set; }

        public int HomeFreeKicks { get; private set; }
        public int AwayFreeKicks { get; private set; }

        public int HomePossession { get; private set; }
        public int AwayPossession { get; private set; }

        public IEnumerable<int> HomeGoalscorers { get; private set; }
        public IEnumerable<int> AwayGoalscorers { get; private set; }
    }
}
