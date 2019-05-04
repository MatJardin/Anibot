using Miki.GraphQL.Queries;
using Newtonsoft.Json;

namespace Anibot.Anilist.Internal
{
    public interface IUserSearchResult
    {
        string Name { get; }
    }

    public interface IUser : IUserSearchResult
    {
        string About { get; }

        object Favourites { get; }

        object Stats { get; }

        string SiteUrl { get; }

        string Avatar { get; }
    }
    [GraphQLSchema("User")]
    public class AnilistUser : IUser
    {
        [JsonProperty("name")]
        internal string name;

        [JsonProperty("avatar")]
        internal string avatar;

        [JsonProperty("about")]
        internal string about;

        [JsonProperty("favourites")]
        internal object favourites;

        [JsonProperty("stats")]
        internal object stats;

        [JsonProperty("siteUrl")]
        internal string siteUrl;
        public string user => name;
        public string About => about;

        public object Favourites => favourites;

        public object Stats => stats;

        public string Avatar => avatar;

        public string Name => name;

        public string SiteUrl => siteUrl;
    }   
}