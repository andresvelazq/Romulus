﻿using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romulus
{
    internal static class Global
    {
        internal static DiscordSocketClient Client { get; set; }
    }
}
