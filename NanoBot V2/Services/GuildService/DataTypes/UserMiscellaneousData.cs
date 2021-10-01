using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    public class UserMiscellaneousData
    {
        public bool hasCustomCardQuote;
        public string customCardQuote;

        public UserMiscellaneousData()
        {
            hasCustomCardQuote = false;
            customCardQuote = "";
        }

        public void LoadFromBSON(MongoDB.Bson.BsonDocument _loadFrom)
        {
            hasCustomCardQuote = _loadFrom.GetValue("hasCustomCardQuote").AsBoolean;
            customCardQuote = _loadFrom.GetValue("customCardQuote").AsString;
        }

        public MongoDB.Bson.BsonDocument SaveToBSON()
        {
            var bson = new MongoDB.Bson.BsonDocument
            {
                { "hasCustomCardQuote", hasCustomCardQuote },
                { "customCardQuote", customCardQuote }
            };

            return bson;
        }

        public void SetCustomCardQuote(string _quote)
        {
            customCardQuote = _quote;
            hasCustomCardQuote = true;
        }

        public void RemoveCustomCardQuote()
        {
            customCardQuote = "";
            hasCustomCardQuote = false;
        }
    }
}
