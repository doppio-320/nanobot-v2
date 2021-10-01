using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.WebSocket;

namespace NanoBot_V2.Services
{
    public static class ServiceMain
    {        
        public static DiscordSocketClient DiscordClient
        {
            get
            {
                return client;
            }
        }

        private static DiscordSocketClient client;

        public static void SendClient(DiscordSocketClient _ctx)
        {
            client = _ctx;            
        }
    }
}
