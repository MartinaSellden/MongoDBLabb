using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBLabb
{
    internal class EntryODM
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("content")]
        public string Content { get; set; }
        [BsonElement("date")]
        public string Date { get; set; }
        [BsonElement("shortDate")]
        public string ShortDate { get; set; }


        public EntryODM(string title, string content)
        {
            Title = title;
            Content = content;
            Date = DateTime.Now.ToString();
            ShortDate= DateTime.Now.ToShortDateString();
        }

    }
}
