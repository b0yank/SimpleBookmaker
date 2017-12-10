namespace SimpleBookmaker.Web.Models
{
    public abstract class PageModel
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public string RequestPath { get; set; }
    }
}
