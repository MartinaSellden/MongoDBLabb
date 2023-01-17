using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBLabb
{
    internal class MongoDAO : IEntryDAO
    {
        MongoClient dbClient;
        IMongoDatabase database;
        IMongoCollection<EntryODM> entriesCollection;  //ska det vara kvar?

        public MongoDAO(string connectionString, string database)
        {
            dbClient = new MongoClient(connectionString);
            this.database = this.dbClient.GetDatabase(database);
            entriesCollection = this.database.GetCollection<EntryODM>("Entries");

        }

        public async Task CreateEntryAsync(EntryODM entry)         
        { 
            await entriesCollection.InsertOneAsync(entry);
        }

        public async Task DeleteEntryAsync(string date)
        {
            var filter = Builders<EntryODM>.Filter.Eq("date", date);
            await entriesCollection.DeleteOneAsync(filter);

        }
          public List<EntryODM> ReadAllEntries()
        {
            return entriesCollection.Find(new BsonDocument()).ToList();

        }

        public List<EntryODM> ReadEntriesByFilter(string fieldName, string fieldValue)
        {
            var filter = Builders<EntryODM>.Filter.Eq(fieldName, fieldValue);           
            return entriesCollection.Find(filter).ToList();
        }

        public async Task UpdateEntryAsync(string date, string content)
        {
            var filter = Builders<EntryODM>.Filter.Eq("date", date);
            var update = Builders<EntryODM>.Update.Set("content", content);

            await entriesCollection.UpdateOneAsync(filter, update);
        }
    }
}
