using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using MongoDB.Bson;
using MongoDB.Driver;

namespace NanoBot_V2.Services
{
    public class UserCacheServices
    {
        public static Dictionary<ulong, UserCachedInfo> loadedCache = new Dictionary<ulong, UserCachedInfo>();

        public static void Load()
        {
            var collection = DatabaseConnectionManager.GetCollection("user_info_cache");
            var allCache = collection.Find(new BsonDocument()).ToList();

            foreach (var bsonUCache in allCache)
            {
                var userID = bsonUCache.GetValue("userID").AsInt64.LongToUlong();
                var userName = bsonUCache.GetValue("userName").AsString;
                var pfpURL = bsonUCache.GetValue("pfpURL").AsString;
                var lastSet = bsonUCache.GetValue("lastSet").ToUniversalTime();

                AddOrUpdateUserCache(userID, userName, pfpURL, lastSet);
            }

            Scheduler.MyScheduler.IntervalInMinutes(5d, () => {
                Save(false);
            });


            Console.WriteLine("Loaded UserCacheInfo Data.");
        }

        public static void Save(bool _bkp)
        {
            var collection = DatabaseConnectionManager.GetCollection("user_info_cache");

            if (_bkp)
            {
                collection = DatabaseConnectionManager.GetCollection("user_info_cache_bkp");
            }

            try
            {
                foreach (var cacheKV in loadedCache)
                {
                    var cache = cacheKV.Value;

                    if (cache.isCreatedNull)
                        continue;

                    var existingFilter = Builders<BsonDocument>.Filter.Eq("userID", cache.userID);
                    collection.DeleteOne(existingFilter);

                    var finalCacheDoc = new BsonDocument
                {
                    { "userID", cache.userID.UlongToLong() },
                    { "userName", cache.userName },
                    { "pfpURL", cache.pfpURL },
                    { "lastSet", cache.lastSet },
                };

                    collection.InsertOne(finalCacheDoc);
                }
            }
            catch(Exception e) { }
        }

        public static void AddOrUpdateUserCache(ulong _id, string _userName, string _pfpUrl)
        {
            if (!loadedCache.ContainsKey(_id))
            {
                loadedCache.Add(_id, new UserCachedInfo(_id));
            }
            loadedCache[_id].SetAll(_id, _userName, _pfpUrl);            
        }

        public static void AddOrUpdateUserCache(ulong _id, string _userName, string _pfpUrl, DateTime _dateTime)
        {
            if (!loadedCache.ContainsKey(_id))
            {
                loadedCache.Add(_id, new UserCachedInfo(_id));
            }
            loadedCache[_id].SetAll(_id, _userName, _pfpUrl, _dateTime);            
        }

        public static UserCachedInfo GetUserCache(ulong _id)
        {
            if (!loadedCache.ContainsKey(_id))
            {
                loadedCache.Add(_id, new UserCachedInfo(_id));
                loadedCache[_id].SetTemporaryNull(_id);
            }
            return loadedCache[_id];
        }
    }
}
