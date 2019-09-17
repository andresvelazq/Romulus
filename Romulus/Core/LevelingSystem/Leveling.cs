using Discord.WebSocket;
using Romulus.Core.UserAccounts;
using System;

namespace Romulus.Core.LevelingSystem
{
    internal static class Leveling
    {
        internal static void UserSentMessage(SocketGuildUser user)
        {
            UserAccount account = UserAccounts.UserAccounts.GetAccount(user);

            if (account.LastMessage.Subtract(DateTime.Now).TotalMinutes < 1) { return; }

            else
            {
                account.EXP += 5;
                account.LastMessage = DateTime.Now;
                return;
            }
        }
    }
}
