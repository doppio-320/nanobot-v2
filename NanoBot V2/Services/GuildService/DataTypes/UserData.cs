using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    public class UserData
    {
        public ulong userID;

        public UserRank rank;
        public UserBalance balance;
        public UserInventory inventory;
        public UserMiscellaneousData miscData;

        public UserData(ulong _userid)
        {
            userID = _userid;

            rank = new UserRank();
            balance = new UserBalance();
            inventory = new UserInventory();
            miscData = new UserMiscellaneousData();
        }
    }
}
