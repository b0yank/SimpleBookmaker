﻿namespace SimpleBookmaker.Web.Infrastructure
{
    public static class ErrorMessages
    {
        public const string InvalidUser = "User was not found.";
        public const string InvalidTeam = "Team was not found.";
        public const string InvalidRole = "Invalid role {0}.";
        public const string InvalidPlayer = "Player was not found.";
        public const string InvalidTournament = "Tournament was not found.";
        public const string InvalidGame = "Game was not found.";
        public const string GameCreateFailed = "Could not create game - teams or tournament could not be found.";
        public const string TeamTournamentAddFailed = "Could not add team(s) to tournament. Tournament and/or one or more teams are invalid.";
        public const string TeamAlreadyInTournament = "The chosen team is already signed up for this tournament.";
        public const string TournamentRemoveFailed = "Failed to remove tournament. Perhaps there are still unresolved bets for one or more games?";
    }
}