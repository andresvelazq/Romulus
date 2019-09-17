using Discord.Commands;
using Discord.WebSocket;
using Romulus.Core.LevelingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Romulus
{
    class CommandHandler
    {
        DiscordSocketClient _client;
        CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            client.UserJoined += WelcomeMessage;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        public async Task WelcomeMessage(SocketGuildUser user)
        {
            var channel = _client.GetChannel(511983395249848330) as SocketTextChannel;
            await channel.SendMessageAsync($"Welcome {user.Mention} to {channel.Guild.Name}! " +
                $"Please use the command `r.set` followed by your nation name for masking! Enjoy your stay :D");
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;
            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);

            // does nothing if a bot sends a message
            if (context.User.IsBot) return;

            Leveling.UserSentMessage((SocketGuildUser)context.User);

            int argPos = 0;
            if (msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos) 
                || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
