namespace SimpleBookmaker.Data.Models.Coefficients
{
    using System.ComponentModel.DataAnnotations;


    public abstract class BetCoefficient
    {
        public int Id { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Coefficient must be larger than zero")]
        [Required]
        public double Coefficient { get; set; }
    }
}
