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

        public static void Initialize(string _con)
        {
            ProgramLogger.LogMessage($"Resolving connection to MongoDB");

            var settings = MongoClientSettings.FromConnectionString(_con);
            client = new MongoClient(settings);
            database = client.GetDatabase("nanobot_db");
            staticDatabase = client.GetDatabase("nanobot_db_static");

            ProgramLogger.LogImportant($"Connected MongoDB [Cluster ID: {client.Cluster.ClusterId}]");            
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
