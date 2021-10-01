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
    public static class GuildServices
    {
        public static Dictionary<ulong, GuildData> loadedGuilds = new Dictionary<ulong, GuildData>();

        public static void Load()
        {
            var collection = DatabaseConnectionManager.GetCollection("guild_data");
            var allGuilds = collection.Find(new BsonDocument()).ToList();

            foreach (var guild in allGuilds)
            {
                var guildID = guild.GetValue("guildID").AsInt64.LongToUlong();

                var gData = new GuildData(guildID);

                gData.chatChannelID = guild.GetValue("chatChannelID").AsInt64.LongToUlong();
                gData.logChannelID = guild.GetValue("logChannelID").AsInt64.LongToUlong();
                gData.muteRoleID = guild.GetValue("muteRoleID").AsInt64.LongToUlong();
                gData.supressRankUpMessage = guild.GetValue("supressRankUpMessage").AsBoolean;

                gData.autoresData = new GuildAutores();
                var ARDatas = guild.GetValue("autoresData").AsBsonDocument.GetValue("autoresResponses").AsBsonArray;

                foreach (var arDataObj in ARDatas)
                {
                    var arData = arDataObj.AsBsonDocument;

                    gData.autoresData.AddAutores(arData.GetValue("trigger").AsString,
                        arData.GetValue("response").AsString,
                        AutoresResponse.ParseAutoresCondition(arData.GetValue("condition").AsString),
                        arData.GetValue("caseSensitive").AsBoolean);
                }

                gData.userDatas = new Dictionary<ulong, UserData>();
                var udatas = guild.GetValue("userDatas").AsBsonArray;
                foreach (var udataObj in udatas)
                {
                    var uData = udataObj.AsBsonDocument;

                    var userData = new UserData(uData.GetValue("userID").AsInt64.LongToUlong());

                    var rankData = uData.GetValue("rank").AsBsonDocument;
                    userData.rank.currentXP = rankData.GetValue("currentXP").AsInt32;
                    userData.rank.currentRank = rankData.GetValue("currentRank").AsInt32;

                    var balanceData = uData.GetValue("balance").AsBsonDocument;
                    userData.balance.balance = balanceData.GetValue("balance").AsInt32;

                    var inventory = uData.GetValue("inventory").AsBsonDocument;
                    var arr = inventory.GetValue("items").AsBsonArray;

                    foreach (var arrItem in arr)
                    {
                        var bsonItem = arrItem.AsBsonDocument;

                        userData.inventory.AddItem(bsonItem.GetValue("item_id").AsInt32, bsonItem.GetValue("quantity").AsInt32);
                    }

                    try
                    {
                        var miscData = uData.GetValue("misc").AsBsonDocument;
                        userData.miscData.LoadFromBSON(miscData);
                    }
                    catch (Exception e) { Console.WriteLine($"error reading inv {e}"); }

                    gData.userDatas.Add(userData.userID, userData);
                }

                if (!loadedGuilds.ContainsKey(gData.guildID))
                {
                    loadedGuilds.Add(gData.guildID, gData);
                }
                else
                {
                    loadedGuilds[gData.guildID] = gData;
                }
            }

            Scheduler.MyScheduler.IntervalInMinutes(5d, () => {
                Save(false);
            });

            Console.WriteLine("Loaded Guild Data.");
        }

        public static void Save(bool _bkp)
        {
            var collection = DatabaseConnectionManager.GetCollection("guild_data");

            if (_bkp)
            {
                collection = DatabaseConnectionManager.GetCollection("guild_data_bkp");
            }

            foreach (var kguild in loadedGuilds)
            {
                var guild = kguild.Value;

                var existingFilter = Builders<BsonDocument>.Filter.Eq("guildID", guild.guildID);
                collection.DeleteOne(existingFilter);

                var userDataBArray = new BsonArray();
                foreach (var kUData in guild.userDatas)
                {
                    var uData = kUData.Value;

                    var invItemsArray = new BsonArray();
                    foreach(var item in uData.inventory.storedItems)
                    {
                        var bsonItem = new BsonDocument
                        {
                            { "item_id", item.Key },
                            { "quantity", item.Value },
                        };

                        invItemsArray.Add(bsonItem);
                    }

                    var userData = new BsonDocument
                    {
                        { "userID", uData.userID.UlongToLong() },
                        { "rank", new BsonDocument
                            {
                                { "currentRank", uData.rank.currentRank },
                                { "currentXP", uData.rank.currentXP }
                            }
                        },
                        { "balance" , new BsonDocument
                            {
                                {"balance", uData.balance.balance }
                            }
                        },
                        { "inventory" , new BsonDocument
                            {
                                { "items", invItemsArray }
                            }
                        },
                        { "misc" , uData.miscData.SaveToBSON() }
                    };

                    userDataBArray.Add(userData);
                }

                var autoresDoc = new BsonDocument
                {
                    { "autoresResponses", new BsonArray() }
                };

                foreach (var autoRes in guild.autoresData.autoresResponses)
                {
                    var autoresSample = new BsonDocument
                    {
                        { "condition", autoRes.condition.ToString() },
                        { "caseSensitive", autoRes.caseSensitive },
                        { "trigger", autoRes.trigger },
                        { "response", autoRes.response }
                    };

                    autoresDoc.GetValue("autoresResponses").AsBsonArray.Add(autoresSample);
                }


                var finalGuildDoc = new BsonDocument
                            {
                                { "guildID", guild.guildID.UlongToLong() },
                                { "chatChannelID", guild.chatChannelID.UlongToLong() },
                                { "logChannelID", guild.logChannelID.UlongToLong() },
                                { "muteRoleID", guild.muteRoleID.UlongToLong() },
                                { "supressRankUpMessage", guild.supressRankUpMessage },
                                { "userDatas", userDataBArray},
                                { "autoresData", autoresDoc}
                            };

                collection.InsertOne(finalGuildDoc);
            }
        }

        public static GuildData GetGuildData(ulong _id)
        {
            if (!loadedGuilds.ContainsKey(_id))
            {
                loadedGuilds.Add(_id, new GuildData(_id));
            }
            return loadedGuilds[_id];
        }
    }
}
