using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Discord.Rest;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using NanoBot_V2.Services;

namespace NanoBot_V2
{
    public class BotRuntime
    {
        private const string BOT_VERSION = "2.0.85.69.42";        

        private static DiscordSocketClient discordClient;
        public static DiscordSocketClient DiscordClient
        {
            get
            {
                return discordClient;
            }
        }

        private CommandService commandService;
        private IServiceProvider serviceProvider;

        public void Run()
        {
            RunBotAsync().GetAwaiter().GetResult();
        }

        public async Task RunBotAsync()
        {
            Console.WriteLine("Enter the bot TOKEN:");
            var token = Console.ReadLine();

            RNGManager.Create();

            discordClient = new DiscordSocketClient(new DiscordSocketConfig() { MessageCacheSize = 100, AlwaysDownloadUsers = true });
            commandService = new CommandService();
            serviceProvider = new ServiceCollection()
                .AddSingleton(discordClient)
                .AddSingleton(commandService)
                .BuildServiceProvider();

            discordClient.Log += Log;

            discordClient.MessageReceived += ProcessMessage;
            discordClient.MessageDeleted += MessageDeleted;
            discordClient.MessageUpdated += MessageEdited;

            await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProvider);

            await discordClient.LoginAsync(TokenType.Bot, token);
            await discordClient.StartAsync();
            await discordClient.SetGameAsync("n2!help is INOP - v" + BOT_VERSION);

            DatabaseConnectionManager.Initialize();
            GuildServices.Load();
            UserCacheServices.Load();
            InventoryItemServices.Load();
            StoreServices.Load();

            await Task.Delay(-1);
        }

        private async Task MessageEdited(Cacheable<IMessage, ulong> arg1, SocketMessage arg2, ISocketMessageChannel arg3)
        {
            try
            {
                var arg = await arg1.GetOrDownloadAsync();
                if (arg != null)
                {
                    var channel = arg.Channel as SocketGuildChannel;
                    var guild = channel.Guild.Id;
                    SniperService.OnEdited(guild, arg, arg.Content, arg2.Content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private async Task MessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            try
            {
                var arg = await arg1.GetOrDownloadAsync();
                if (arg != null)
                {
                    if (!arg.Author.IsBot)
                    {
                        LoggingServices.LogMessageDeleted(arg);
                    }
                    var channel = arg.Channel as SocketGuildChannel;
                    var guild = channel.Guild.Id;                    

                    if(arg.Attachments.Count > 0)
                    {
                        SniperService.OnDeleted(guild, arg, arg.Attachments.ToArray()[0]);
                    }
                    else
                    {
                        SniperService.OnDeleted(guild, arg);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private async Task ProcessMessage(SocketMessage _arg)
        {
            var message = (SocketUserMessage)_arg;
            var context = new SocketCommandContext(discordClient, message);

            if (message.Author.IsBot)
                return;

            int argPos = 0;

            if (message.HasStringPrefix("n2!", ref argPos))
            {
                var result = await commandService.ExecuteAsync(context, argPos, serviceProvider);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
            else
            {
                UserCacheServices.AddOrUpdateUserCache(context.User.Id, context.User.Username, context.User.GetAvatarUrl());

                if (AutoresService.MessageCouldTriggerAutores(context))
                {
                    await context.Channel.SendMessageAsync(AutoresService.GetAutores(context));
                }

                if (GuildServices.GetGuildData(context.Guild.Id).chatChannelID == context.Channel.Id)
                {
                    var prevMoney = GuildServices.GetGuildData(context.Guild.Id).GetUserData(context.User.Id).balance.balance;
                    var result = GuildServices.GetGuildData(context.Guild.Id).GetUserData(context.User.Id).rank.AddXP(
                        GuildServices.GetGuildData(context.Guild.Id).GetUserData(context.User.Id).balance);

                    if (GuildServices.GetGuildData(context.Guild.Id).supressRankUpMessage)
                        return;

                    if (result == 1)
                    {
                        await context.Channel.SendMessageAsync($"Congratulations **{context.User.Username}** you have ranked up to rank **{GuildServices.GetGuildData(context.Guild.Id).GetUserData(context.User.Id).rank.currentRank}**! :partying_face:");
                    }
                    else if (result > 1)
                    {
                        await context.Channel.SendMessageAsync($"Congratulations **{context.User.Username}** you have skipped **{result}** ranks and ranked up to rank **{GuildServices.GetGuildData(context.Guild.Id).GetUserData(context.User.Id).rank.currentRank}**! :partying_face:");
                    }
                    if (result >= 1)
                    {
                        await context.Channel.SendMessageAsync($"You have received a bonus of **{GuildServices.GetGuildData(context.Guild.Id).GetUserData(context.User.Id).balance.balance - prevMoney}** credits");
                    }
                }
            }
        }
    }
}
