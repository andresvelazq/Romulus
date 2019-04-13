using Discord.WebSocket;
using Romulus.Core.UserAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romulus.Core.LevelingSystem
{
    internal static class Leveling
    {
        internal static void UserSentMessage(SocketGuildUser user)
        {
            var account = UserAccounts.UserAccounts.GetAccount(user);

            if (account.LastMessage.Subtract(DateTime.Now).TotalMinutes < 1) { return; }


        }
    }
}
