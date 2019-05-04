using System;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using Miki.Anilist;
using System.Linq;
using Miki.GraphQL;
using Miki.GraphQL.Queries;
using Anibot.Anilist.Internal;

namespace Anibot.Modules.Anilist
{
    static class Extensions
    {

        public static bool In<T>(this T item, params T[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            return items.Contains(item);
        }

    }
    public class AnilistModule : ModuleBase<SocketCommandContext>
    {
        [Command("anime")]
        [Alias("a")]
        public async Task UserAsync([Remainder]string query)
        {
            AnilistClient client = new AnilistClient();
            var ch = await client.GetMediaAsync(query,
                MediaFormat.MANGA, MediaFormat.NOVEL, MediaFormat.ONE_SHOT);
            int count = 0;
            string Genres = "Genres: ";
            string Score = ch.Score.ToString();

            while (count < ch.Genres.Count())
            {
                Genres = Genres + ch.Genres[count] + ", ";
                count++;
            }
            Genres = Genres.Remove(Genres.Length - 2);
            var embed = new EmbedBuilder()
            {
                Title = ch.DefaultTitle,
                Url = ch.Url,
                Description = Genres + "\n" + "\n" + ch.Episodes + " épisodes"
                + "\n" + "\n" + ch.Description,
                ThumbnailUrl = ch.CoverImage,
                Color = Discord.Color.Red,
                Footer = new EmbedFooterBuilder()
                {
                    IconUrl = ch.Url,
                    Text = "AL Score: " + Score + "/100",
                }
            }.Build();

            await ReplyAsync(Context.User.Mention);
            await ReplyAsync(embed: embed);
        }

        private bool AliasAttribute(string v)
        {
            throw new NotImplementedException();
        }

        [Command("manga")]
        [Alias("m")]
        public async Task UserAsyncm([Remainder]string query)
        {
            AnilistClient client = new AnilistClient();
            var ch = await client.GetMediaAsync(query, MediaFormat.TV, MediaFormat.OVA,
                MediaFormat.MOVIE, MediaFormat.MUSIC, MediaFormat.ONA, MediaFormat.SPECIAL, MediaFormat.TV_SHORT);
            int count = 0;
            string Genres = "Genres: ";
            string Score = ch.Score.ToString();

            while (count < ch.Genres.Count())
            {
                Genres = Genres + ch.Genres[count] + ", ";
                count++;
            }
            Genres = Genres.Remove(Genres.Length - 2);
            string Longueur = String.Empty;
            if (ch.Status.StartsWith("FINISHED"))
            {
                Longueur = ch.Chapters + " chapitres, " + ch.Volumes +
" volumes" + "\n" + "\n";
            }
            var embed = new EmbedBuilder()
            {
                Title = ch.DefaultTitle,
                Url = ch.Url,
                Description = Genres + "\n" + "\n" + Longueur + ch.Description,
                ThumbnailUrl = ch.CoverImage,
                Color = Discord.Color.Red,
                Footer = new EmbedFooterBuilder()
                {
                    IconUrl = ch.Url,
                    Text = "AL Score: " + Score + "/100",
                }
            }.Build();

            await ReplyAsync(Context.User.Mention, embed: embed);
        }

        [Command("Character")]
        [Alias("C")]
        public async Task UserAsyncc([Remainder]string query)
        {
            AnilistClient client = new AnilistClient();
            var ch = await client.GetCharacterAsync(query);
            string id = ch.Id.ToString();
            string nom = ch.FirstName + " " + ch.LastName;
            string Description = string.Empty;
            if (ch.Description.Length > 2048)
            {
                Description = ch.Description.Replace("~!", "||").Replace("!~", "||").Remove(2048);
            }
            else
            {
                Description = ch.Description.Replace("~!", "||").Replace("!~", "||");
            }
            var embed = new EmbedBuilder()
            {
                Title = nom.PadLeft(4, ','),
                Url = ch.SiteUrl,
                Description = Description,
                ThumbnailUrl = ch.LargeImageUrl,
                Color = Discord.Color.Red,
                Footer = new EmbedFooterBuilder()
                {
                    IconUrl = ch.LargeImageUrl,
                    Text = nom,
                }
            }.Build();

            await ReplyAsync(Context.User.Mention, embed: embed);
        }

        [Priority(1)]
        [Command("Staff")]
        [Alias("S")]
        public async Task UserAsynccid([Remainder]string query)
        {
            AnilistClient client = new AnilistClient();
            var ch = await client.GetStaffAsync(query);
            string nom = ch.FirstName + " " + ch.LastName;
            string Description = string.Empty;
            if (ch.Description.Length > 2048)
            {
                Description = ch.Description.Replace("~!", "||").Replace("!~", "||").Remove(2048);
            }
            else
            {
                Description = ch.Description.Replace("~!", "||").Replace("!~", "||");
            }
            var embed = new EmbedBuilder()
            {
                Title = nom.PadLeft(4, ','),
                Url = ch.SiteUrl,
                Description = Description,
                ThumbnailUrl = ch.LargeImageUrl,
                Color = Discord.Color.Red,
                Footer = new EmbedFooterBuilder()
                {
                    IconUrl = ch.LargeImageUrl,
                    Text = nom,
                }
            }.Build();
            await ReplyAsync(Context.User.Mention, embed: embed);

        }
        [Priority(100)]
        [Command("Staff")]
        [Alias("S")]
        public async Task UserAsyncid([Remainder]long Id)
        {
            AnilistClient client = new AnilistClient();
            var test = await client.GetStaffAsync(Id);
            string nom = test.FirstName + " " + test.LastName;
            string Description = string.Empty;
            if (test.Description.Length > 2048)
            {
                Description = test.Description.Replace("~!", "||").Replace("!~", "||").Remove(2048);
            }
            else
            {
                Description = test.Description.Replace("~!", "||").Replace("!~", "||");
            }
            var embed = new EmbedBuilder()
            {
                Title = nom.PadLeft(4, ','),
                Url = test.SiteUrl,
                Description = Description,
                ThumbnailUrl = test.LargeImageUrl,
                Color = Discord.Color.Red,
                Footer = new EmbedFooterBuilder()
                {
                    IconUrl = test.LargeImageUrl,
                    Text = nom,
                }
            }.Build();
            await ReplyAsync(Context.User.Mention, embed: embed);
        }
        GraphQLClient graph;

        //IGraphQLQuery getUserByNameQuery;

        //public IGraphQLQuery GetUserByNameQuery { get => getUserByNameQuery; set => getUserByNameQuery = value;}

        public async Task UserAsyncfavs([Remainder]string query)
        {
        //    graph = new GraphQLClient("https://graphql.anilist.co");

        //    GetUserByNameQuery = graph.CreateQuery()
        //            .WithSchema<AnilistUser, IQueryBuilder>(x =>
        //                x.WithDynamicParameter<string>("search")
        //                 .WithDynamicParameter<MediaFormat[]>("format_not_in"))
        //            .Compile();
        //    AnilistClient client = new AnilistClient();
        //    var test = await client.GetUserAsync(query);

        //    var embed = new EmbedBuilder()
        //    {
        //        Title = query + "'s " + "favorites",
        //        Url = test.SiteUrl,
        //        Color = Discord.Color.Red,
        //        Fields = new EmbedFieldBuilder()
        //        {ci, 
        //            Name = "Anime",
        //            Value = test.AnimeFavs,
        //            IsInline = true,
        //        },
        //        Fields = new EmbedFieldBuilder()
        //        {
        //            Name = "Manga",
        //            Value = test.MangaFavs,
        //            IsInline = true,
        //        },
        //        Fields = new EmbedFieldBuilder()
        //        {
        //            Name = "Characters",
        //            Value = test.CharaFavs,
        //            IsInline = true,
        //        },
        //        Fields = new EmbedFieldBuilder()
        //        {
        //            Name = "Staff",
        //            Value = test.StaffFavs,
        //            IsInline = true,
        //        },
        //    }.Build();
        //    await ReplyAsync(Context.User.Mention, embed: embed);
        }
    }
}

