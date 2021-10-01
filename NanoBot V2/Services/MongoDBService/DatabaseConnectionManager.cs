using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

namespace NanoBot_V2.Services
{
    public class DatabaseConnectionManager
    {
        private static MongoClient client;
        private static IMongoDatabase database;
        private static IMongoDatabase staticDatabase;

        public static void Initialize()
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://nanobot:3Sk7Kk8UfnFeXwFa@doppiecluster.xywaj.mongodb.net/nanobot_db?retryWrites=true&w=majority");
            client = new MongoClient(settings);
            database = client.GetDatabase("nanobot_db");
            staticDatabase = client.GetDatabase("nanobot_db_static");

            Console.WriteLine("Connected MongoDB...");
        }

        public static IMongoCollection<BsonDocument> GetCollection(string _name)
        {
            return database.GetCollection<BsonDocument>(_name);
        }

        public static IMongoCollection<BsonDocument> GetCollectionFromStatic(string _name)
        {
            return staticDatabase.GetCollection<BsonDocument>(_name);
        }
    }
}
