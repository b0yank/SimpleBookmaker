namespace SimpleBookmaker.Data.Models.Bets
{
    using Core.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class Bet
    {
        public int Id { get; set; }

        public double Coefficient { get; set; }

        public bool IsSuccess { get; set; }

        public bool IsEvaluated { get; set; }
    }
}
