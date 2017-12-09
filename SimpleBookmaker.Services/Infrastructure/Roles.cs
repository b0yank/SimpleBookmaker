namespace SimpleBookmaker.Services.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Roles
    {
        public const string Administrator = "Administrator";
        public const string Bookmaker = "Bookmaker";
        public const string Statistician = "Statistician";

        public static IEnumerable<string> All()
        {
            var roleFields = typeof(Roles)
                                .GetFields()
                                .Where(f => f.IsLiteral
                                    && f.GetRawConstantValue().GetType() == typeof(string));

           return roleFields.Select(f => f.GetRawConstantValue().ToString());
        }
    }
}
