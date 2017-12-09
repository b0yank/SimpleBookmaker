namespace SimpleBookmaker.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Bets;
    using Models.BetSlips;
    using Models.Coefficients;

    public class SimpleBookmakerDbContext : IdentityDbContext<User>
    {
        public SimpleBookmakerDbContext(DbContextOptions<SimpleBookmakerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<GameBet> GameBets { get; set; }

        public DbSet<GameBetCoefficient> GameBetCoefficients { get; set; }

        public DbSet<GameBetSlip> GameBetSlips { get; set; }

        public DbSet<PlayerGameBet> PlayerGameBets { get; set; }

        public DbSet<PlayerGameBetCoefficient> PlayerGameBetCoefficients { get; set; }

        public DbSet<TournamentBet> TournamentBets { get; set; }

        public DbSet<TournamentBetSlip> TournamentBetSlips { get; set; }

        public DbSet<TournamentBetCoefficient> TournamentBetCoefficients { get; set; }

        public DbSet<TournamentPlayer> TournamentPlayers { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<TournamentTeam> TournamentsTeams { get; set; }

        public DbSet<BetSlipHistory> BetSlipHistories { get; set; }

        public DbSet<BetHistory> BetHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(u => u.BetSlipHistories)
                .WithOne(bsh => bsh.User)
                .HasForeignKey(bsh => bsh.UserId);

            builder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId);

            builder.Entity<Team>()
                .HasMany(te => te.Tournaments)
                .WithOne(to => to.Team)
                .HasForeignKey(to => to.TeamId);

            builder.Entity<Tournament>()
                .HasMany(to => to.Teams)
                .WithOne(te => te.Tournament)
                .HasForeignKey(to => to.TournamentId);

            builder.Entity<Tournament>()
                .HasMany(t => t.Players)
                .WithOne(tp => tp.Tournament)
                .HasForeignKey(tp => tp.TournamentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Team>()
                .HasIndex(t => t.Name)
                .IsUnique(true);

            builder.Entity<Tournament>()
                .HasIndex(t => t.Name)
                .IsUnique(true);

            builder.Entity<Player>()
                .HasMany(p => p.Tournaments)
                .WithOne(t => t.Player)
                .HasForeignKey(t => t.PlayerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Game>()
                .HasOne(g => g.HomeTeam)
                .WithMany(tt => tt.HomeGames)
                .HasForeignKey(g => g.HomeTeamId);

            builder.Entity<Game>()
                .HasOne(g => g.AwayTeam)
                .WithMany(tt => tt.AwayGames)
                .HasForeignKey(g => g.AwayTeamId);

            builder.Entity<Game>()
                .HasMany(g => g.GameCoefficients)
                .WithOne(gc => gc.Game)
                .HasForeignKey(gc => gc.GameId);

            builder.Entity<GameBet>()
                .HasOne(gb => gb.BetSlip)
                .WithMany(gbs => gbs.GameBets)
                .HasForeignKey(gb => gb.BetSlipId);

            builder.Entity<TournamentPlayer>()
                .HasKey(tp => new { tp.TournamentId, tp.PlayerId });

            builder.Entity<TournamentTeam>()
                .HasIndex(tt => new { tt.TeamId, tt.TournamentId })
                .IsUnique(true);

            builder.Entity<BetSlipHistory>()
                .HasMany(bsh => bsh.BetHistories)
                .WithOne(bh => bh.BetSlipHistory)
                .HasForeignKey(bh => bh.BetSlipHistoryId);

            builder.Entity<PlayerGameBetCoefficient>()
                .HasMany(pgbc => pgbc.Bets)
                .WithOne(pgb => pgb.BetCoefficient)
                .HasForeignKey(pgb => pgb.PlayerGameBetCoefficientId);

            builder.Entity<TournamentBetCoefficient>()
                .HasMany(tbc => tbc.Bets)
                .WithOne(tb => tb.BetCoefficient)
                .HasForeignKey(tb => tb.BetCoefficientId);

            builder.Entity<PlayerGameBetCoefficient>()
                .HasOne(pgbc => pgbc.Player)
                .WithMany(p => p.GameCoefficients)
                .HasForeignKey(pgbc => pgbc.PlayerId);

            builder.Entity<PlayerGameBetCoefficient>()
                .HasOne(pgbc => pgbc.Game)
                .WithMany(g => g.PlayerCoefficients)
                .HasForeignKey(pgbc => pgbc.GameId);

            builder.Entity<TournamentBet>()
                .HasOne(tb => tb.BetSlip)
                .WithMany(tbs => tbs.Bets)
                .HasForeignKey(tb => tb.BetSlipId);

            builder.Entity<PlayerGameBet>()
                .HasOne(pgb => pgb.BetSlip)
                .WithMany(pgbs => pgbs.PlayerBets)
                .HasForeignKey(pgb => pgb.BetSlipId);

            base.OnModelCreating(builder);
        }
    }
}
