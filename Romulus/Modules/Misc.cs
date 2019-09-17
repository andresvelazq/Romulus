using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Romulus.Core.UserAccounts;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace Romulus.Cogs
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        // sends random image of a kitten
        [Command("kitten")]
        public async Task Kitten()
        {
            Random rand = new Random();
            int randWid = rand.Next(300, 700);
            int randHi = rand.Next(300, 700);

            string width = randWid.ToString();
            string height = randHi.ToString();
            string url = ($"https://placekitten.com/g/{width}/{height}");

            var embed = new EmbedBuilder();
            embed.WithImageUrl(url);
            embed.WithColor(new Color(0, 250, 255));

            await Context.Channel.SendMessageAsync("*meow*", embed: embed);
        }

        // check account stats
        [Command("stats")]
        public async Task Stats([Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();

            target = mentionedUser ?? Context.User;
            
            var account = UserAccounts.GetAccount(target);
            await Context.Channel.SendMessageAsync($"{target.Username} have {account.EXP} EXP and {account.Credits} credits.");
        }

        // add xp to a user, probably delete later
        [Command("addxp")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddXP(uint exp)
        {
            var account = UserAccounts.GetAccount(Context.User);
            account.EXP += exp;
            UserAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync($"You gained {exp} EXP.");
        }

        // echo user message with embeded text
        [Command("echo")]
        public async Task Echo([Remainder]string message)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(Context.User.Username);
            embed.WithDescription(message);
            embed.WithColor(new Color(0, 250, 255));

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        // randomly choose one of multiple options
        [Command("choose")]
        public async Task Choose([Remainder]string message)
        {
            string[] options = message.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            Random choice = new Random();
            string selection = options[choice.Next(0, options.Length)];

            var embed = new EmbedBuilder();
            embed.WithTitle("I choose");
            embed.WithDescription(selection);
            embed.WithColor(new Color(0, 250, 255));

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        // connects nation to user's Discord account
        [Command("set")]
        public async Task Set([Remainder]string message)
        {
            string region = null, url, state, FlagURL;
            Boolean WA = false;
            var user = Context.User;

            state = message.Replace(' ', '_');
            url = @"https://www.nationstates.net/cgi-bin/api.cgi?nation=" + state + @"&q=name+region+wa+flag";
            HttpClient httpClient = new HttpClient();
            WebClient wc = new WebClient();
            wc.Headers.Add("User-Agent: Romulus");
            var task = await wc.DownloadStringTaskAsync(url);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(task);

            var nameNode = xml.SelectSingleNode("/NATION/NAME");
            state = nameNode.InnerText;
            var regionNode = xml.SelectSingleNode("/NATION/REGION");
            region = regionNode.InnerText;
            var WaNode = xml.SelectSingleNode("/NATION/UNSTATUS");
            WA = (WaNode.InnerText == "WA Member");
            var flagNode = xml.SelectSingleNode("/NATION/FLAG");
            FlagURL = flagNode.InnerText;

            UserAccount account = UserAccounts.GetAccount(user);
            account.Nation = state;
            UserAccounts.SaveAccounts();

            if (region == "The Empire of Mare Nostrum")
            {
                // gives Resident role
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Resident");
                await (user as IGuildUser).AddRoleAsync(role);

                if (WA)
                {
                    // gives World Assembly Resident role
                    role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "World Assembly Resident");
                    await (user as IGuildUser).AddRoleAsync(role);
                    await Context.Channel.SendMessageAsync($"Welcome WA member, {state}! You're a resident of EMN!");
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"Welcome, {state}! You're a resident of EMN!");
                }

                var embed = new EmbedBuilder();
                embed.WithImageUrl(FlagURL);
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            else
            {
                // gives Visitor role
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Visitor");
                await (user as IGuildUser).AddRoleAsync(role);
                await Context.Channel.SendMessageAsync($"Welcome, {state} visitor from {region}! Please enjoy your stay!");
            }
        }

        // prints nation info from NationStates
        [Command("nation")]
        public async Task Nation([Remainder]string message)
        {
            string name, region, WA, found, active;
            string state = message.Replace(' ', '_');
            string url = @"https://www.nationstates.net/cgi-bin/api.cgi?nation=" + state + "&q=region+wa+fullname+lastactivity+founded";
            HttpClient httpClient = new HttpClient();
            WebClient wc = new WebClient();
            wc.Headers.Add("User-Agent: Romulus");
            var task = await wc.DownloadStringTaskAsync(url);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(task);

            var nameNode = xml.SelectSingleNode("/NATION/FULLNAME");
            name = nameNode.InnerText;
            var regionNode = xml.SelectSingleNode("/NATION/REGION");
            region = regionNode.InnerText;
            var WaNode = xml.SelectSingleNode("/NATION/UNSTATUS");
            WA = WaNode.InnerText;
            var foundedNode = xml.SelectSingleNode("/NATION/FOUNDED");
            found = foundedNode.InnerText;
            var activeNode = xml.SelectSingleNode("/NATION/LASTACTIVITY");
            active = activeNode.InnerText;

            var embed = new EmbedBuilder();
            embed.WithColor(new Color(50, 210, 50));
            embed.AddField($"{name}", $"Founded {found}. Currently a member of {region}. Last active {active}.");
            embed.WithFooter(footer => footer.Text = WA);

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
