using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Newtonsoft.Json;
using System.IO;
using Discord.WebSocket;
using Discord;

namespace NanoBot_V2.Services
{
    public class SniperService
    {
        private static Dictionary<ulong, Dictionary<ulong, List<SnipedMessage>>> snipedMessages = new Dictionary<ulong, Dictionary<ulong, List<SnipedMessage>>>();
        private static Dictionary<ulong, Dictionary<ulong, List<EditedMessage>>> editedMessages = new Dictionary<ulong, Dictionary<ulong, List<EditedMessage>>>();

        public static void OnDeleted(ulong _guild, IMessage _message, IAttachment _attachment = null)
        {
            if (!snipedMessages.ContainsKey(_guild))
            {
                snipedMessages.Add(_guild, new Dictionary<ulong, List<SnipedMessage>>());
            }
            if (!snipedMessages[_guild].ContainsKey(_message.Channel.Id))
            {
                snipedMessages[_guild].Add(_message.Channel.Id, new List<SnipedMessage>());
            }

            snipedMessages[_guild][_message.Channel.Id].Add(new SnipedMessage(_message.Author, _message.Content, DateTime.Now, _attachment));
        }

        public static SnipedMessage GetSnipedMessage(ulong _guildID, ulong _channelID, int _back)
        {
            if (!snipedMessages.ContainsKey(_guildID))
            {
                return null;
            }
            if (!snipedMessages[_guildID].ContainsKey(_channelID))
            {
                return null;
            }
            if (_back > snipedMessages[_guildID][_channelID].Count)
                return null;
            return snipedMessages[_guildID][_channelID][snipedMessages[_guildID][_channelID].Count - _back];
        }

        public static void OnEdited(ulong _guild, IMessage _message, string _before, string _after)
        {
            if (!editedMessages.ContainsKey(_guild))
            {
                editedMessages.Add(_guild, new Dictionary<ulong, List<EditedMessage>>());
            }
            if (!editedMessages[_guild].ContainsKey(_message.Channel.Id))
            {
                editedMessages[_guild].Add(_message.Channel.Id, new List<EditedMessage>());
            }

            editedMessages[_guild][_message.Channel.Id].Add(new EditedMessage(_message.Author, _before, _after, DateTime.Now));
        }

        public static EditedMessage GetEditedMessage(ulong _guildID, ulong _channelID, int _back)
        {
            if (!editedMessages.ContainsKey(_guildID))
            {
                return null;
            }
            if (!editedMessages[_guildID].ContainsKey(_channelID))
            {
                return null;
            }
            if (_back > editedMessages[_guildID][_channelID].Count)
                return null; 
            return editedMessages[_guildID][_channelID][editedMessages[_guildID][_channelID].Count - _back];
        }
    }
}
