using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    [Serializable]
    public class GuildData
    {
        public ulong guildID;

        public ulong logChannelID;
        public ulong chatChannelID;
        public ulong muteRoleID;
        public bool supressRankUpMessage;        

        public Dictionary<ulong, UserData> userDatas;

        public GuildAutores autoresData;

        public GuildData(ulong _guildID)
        {
            guildID = _guildID;
            logChannelID = 0; chatChannelID = 0; muteRoleID = 0;
            supressRankUpMessage = false;

            userDatas = new Dictionary<ulong, UserData>();
            autoresData = new GuildAutores();
        }

        public UserData GetUserData(ulong _userID)
        {
            if (!userDatas.ContainsKey(_userID))
            {
                userDatas.Add(_userID, new UserData(_userID));
            }
            return userDatas[_userID];
        }       
        
        public void AddUserData(UserData _data)
        {
            userDatas.Add(_data.userID, _data);
        }
    }
}
