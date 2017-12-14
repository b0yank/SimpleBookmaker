namespace SimpleBookmaker.Web.Infrastructure
{
    using System.Collections.Generic;

    public class BetSlipAdditionResult
    {
        private ICollection<string> errors = new List<string>();

        public bool Success { get { return this.errors.Count == 0; } }

        public void AddError(string error) => this.errors.Add(error);

        public IEnumerable<string> GetErrors() => this.errors;
    }
}
