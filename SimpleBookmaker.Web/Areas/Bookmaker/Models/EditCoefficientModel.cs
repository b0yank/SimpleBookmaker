namespace SimpleBookmaker.Web.Areas.Bookmaker.Models
{
    using System.ComponentModel.DataAnnotations;

    public class EditCoefficientModel
    {
        public int CoefficientId { get; set; }

        public int SubjectId { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Coefficient must be larger than zero")]
        public double NewCoefficient { get; set; }
    }
}
