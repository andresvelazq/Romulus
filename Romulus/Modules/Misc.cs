using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Romulus.Core.UserAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
    }
}
