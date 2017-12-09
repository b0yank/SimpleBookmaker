namespace SimpleBookmaker.Services.BetResolvers
{
    using Contracts;
    using Data.Core.Enums;
    using System;
    using System.Linq;
    using System.Reflection;

    public class TournamentBetResolverFactory : ITournamentBetResolverFactory
    {
        public ITournamentBetResolver GetResolver(TournamentBetType betType)
        {
            var resolverType = Assembly.GetAssembly(typeof(TournamentBetResolverFactory))
                .GetTypes()
                .FirstOrDefault(t => t.IsClass
                    && (typeof(ITournamentBetResolver).IsAssignableFrom(t))
                    && t.Name == betType.ToString());

            if (resolverType == null)
            {
                return null;
            }

            return (ITournamentBetResolver)Activator.CreateInstance(resolverType);
        }
    }
}
