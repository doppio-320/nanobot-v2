using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

namespace NanoBot_V2.Services
{
    public static class DatabaseHeartbeat
    {
        private static DateTime firstLogin;        

        private static bool StopCommanded()
        {
            WebClient webCl = new WebClient();
            string stopCmd = webCl.DownloadString("https://drive.google.com/uc?export=download&id=1X-mrMUOWQPc8WtGd0x4AxB24Dkz5y6Ww");

            if(stopCmd == "STOP")
            {
                return true;
            }
            return false;
        }

        private static string GetIP()
        {
            WebClient webCl = new WebClient();
            string ip = webCl.DownloadString("https://checkip.amazonaws.com/");

            return ip.Trim();
        }

        public static void Login()
        {
            var collection = DatabaseConnectionManager.GetCollection("db_heartbeats");

            firstLogin = DateTime.Now;

            string id = TimeToID(firstLogin);            
            var loginNewSession = new BsonDocument
            {
                { "sessionID", id },
                { "type", "login" },
                { "time", DateTime.Now },
                { "ipv4", GetIP() }
            };
            collection.InsertOne(loginNewSession);

            ProgramLogger.LogMessage($"DatabaseHeartbeat Login, SessionID:[{id}]");

            Scheduler.MyScheduler.IntervalInMinutes(3f, () =>
            {
                Heartbeat();
            });            
        }

        public static void Heartbeat()
        {
            var collection = DatabaseConnectionManager.GetCollection("db_heartbeats");

            string id = TimeToID(firstLogin);
            var heartbeat = new BsonDocument
            {
                { "sessionID", id },
                { "type", "heartbeat" },
                { "time", DateTime.Now }
            };
            collection.InsertOne(heartbeat);

            ProgramLogger.LogMessage($"DatabaseHeartbeat Pulse, SessionID:[{id}], PulseTime:[{DateTime.Now.ToShortTimeString()}]");

            if (!BotRuntime.CheckVersion(false))
            {
                ProgramLogger.LogError($"NanoBot version is outdated! New version is out v{BotRuntime.GetLatestVersion()}", true);
            }

            if (StopCommanded())
            {
                ProgramLogger.LogError("Master instance manager has commanded immediate shutdown (Maybe for database maintenance reasons)", true);
                Environment.Exit(0);
            }
        }

        private static string TimeToID(DateTime _time)
        {
            long binary = _time.ToBinary();
            byte[] byteArray = BitConverter.GetBytes(binary);
            return Convert.ToBase64String(byteArray);            
        }
    }
}
