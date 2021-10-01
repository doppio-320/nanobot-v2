using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;
using System.IO;

namespace NanoBot_V2.CommandModules
{
    public class SniperCommands : ModuleBase<SocketCommandContext>
    {
        [Command("snipe")]
        [Alias("snitch")]
        public async Task Snipe(int _back = 1)
        {
            try
            {
                var snipe = Services.SniperService.GetSnipedMessage(Context.Guild.Id, Context.Channel.Id, _back);

                if (snipe == null)
                {
                    await ReplyAsync("Nothing to snipe!");
                    return;
                }

                var embed = new EmbedBuilder();
                embed.WithTitle($"Sniped!");
                embed.WithColor(Color.Blue);
                if (!string.IsNullOrEmpty(snipe.Message))
                {
                    embed.AddField($"{snipe.Author.Username}#{snipe.Author.Discriminator}", snipe.Message);
                }
                else
                {
                    embed.AddField($"{snipe.Author.Username}#{snipe.Author.Discriminator}", "There is no text (image upload maybe)");
                }
                embed.AddField($"Deleted on", $"{snipe.TimeDeleted.ToShortTimeString()}");
                await ReplyAsync(embed: embed.Build());

                if (snipe.Attachment != null)
                {
                    await ReplyAsync("Uploading attached attachment:");
                    await ReplyAsync(snipe.Attachment.Url);
                }
            }
            catch (Exception e)
            {
                await ReplyAsync($"Had an error yawa: {e.ToString()}");
            }
        }

        [Command("snipe")]
        [Alias("snitch")]
        public async Task Snipe()
        {
            try
            {
                var snipe = Services.SniperService.GetSnipedMessage(Context.Guild.Id, Context.Channel.Id, 1);

                if (snipe == null)
                {
                    await ReplyAsync("Nothing to snipe!");
                    return;
                }

                var embed = new EmbedBuilder();
                embed.WithTitle($"Sniped!");
                embed.WithColor(Color.Blue);
                if (!string.IsNullOrEmpty(snipe.Message))
                {
                    embed.AddField($"{snipe.Author.Username}#{snipe.Author.Discriminator}", snipe.Message);
                }
                else
                {
                    embed.AddField($"{snipe.Author.Username}#{snipe.Author.Discriminator}", "There is no text (image upload maybe)");
                }
                embed.AddField($"Deleted on", $"{snipe.TimeDeleted.ToShortTimeString()}");
                await ReplyAsync(embed: embed.Build());

                if (snipe.Attachment != null)
                {
                    await ReplyAsync("Uploading attached attachment:");
                    await ReplyAsync(snipe.Attachment.Url);
                }
            }
            catch (Exception e)
            {
                await ReplyAsync($"Had an error yawa: {e.ToString()}");
            }
        }

        [Command("editsnipe")]
        [Alias("esnipe", "esnitch", "editsnitch")]
        public async Task ESnipe(int _back = 1)
        {
            try
            {
                var snipe = Services.SniperService.GetEditedMessage(Context.Guild.Id, Context.Channel.Id, _back);

                if (snipe == null)
                {
                    await ReplyAsync("Nothing to snipe!");
                    return;
                }

                var embed = new EmbedBuilder();
                embed.WithTitle($"Edit Sniped!");
                embed.WithColor(Color.Blue);
                embed.AddField($"{snipe.Author.Username}#{snipe.Author.Discriminator}", $"{snipe.TimeEdited.ToShortTimeString()}");
                embed.AddField($"Before", snipe.BeforeEditMessage);
                embed.AddField($"After", snipe.AfterEditMessage);
                await ReplyAsync(embed: embed.Build());
            }
            catch (Exception e)
            {
                await ReplyAsync($"Had an error yawa: {e.ToString()}");
            }
        }

        [Command("editsnipe")]
        [Alias("esnipe", "esnitch", "editsnitch")]
        public async Task ESnipe()
        {
            try
            {
                var snipe = Services.SniperService.GetEditedMessage(Context.Guild.Id, Context.Channel.Id, 1);

                if (snipe == null)
                {
                    await ReplyAsync("Nothing to snipe!");
                    return;
                }

                var embed = new EmbedBuilder();
                embed.WithTitle($"Edit Sniped!");
                embed.WithColor(Color.Blue);
                embed.AddField($"{snipe.Author.Username}#{snipe.Author.Discriminator}", $"{snipe.TimeEdited.ToShortTimeString()}");
                embed.AddField($"Before", snipe.BeforeEditMessage);
                embed.AddField($"After", snipe.AfterEditMessage);
                await ReplyAsync(embed: embed.Build());
            }
            catch (Exception e)
            {
                await ReplyAsync($"Had an error yawa: {e.ToString()}");
            }
        }
    }
}
