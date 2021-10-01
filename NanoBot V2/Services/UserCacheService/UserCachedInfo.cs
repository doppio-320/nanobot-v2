using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    public class UserCachedInfo
    {
        public ulong userID;
        public string userName;
        public string pfpURL;
        public bool isCreatedNull;
        public DateTime lastSet;

        public UserCachedInfo(ulong _id)
        {
            userID = _id;
            userName = "NULL";
            pfpURL = "";
            isCreatedNull = false;

            lastSet = DateTime.Now.Subtract(TimeSpan.FromHours(12));
        }

        public void SetAll(ulong _id, string _name, string _pfp)
        {
            if (DateTime.Now < lastSet.AddHours(12))
                return;

            userID = _id;
            userName = _name;
            pfpURL = _pfp;

            lastSet = DateTime.Now;
        }

        public void SetTemporaryNull(ulong _id)
        {            
            userID = _id;
            userName = "UNKNOWN";
            pfpURL = "";

            isCreatedNull = true;

            lastSet = DateTime.Now;
        }

        public void SetAll(ulong _id, string _name, string _pfp, DateTime _dateTime)
        {
            if (DateTime.Now < lastSet.AddHours(12))
                return;

            userID = _id;
            userName = _name;
            pfpURL = _pfp;

            lastSet = _dateTime;
        }
    }
}
