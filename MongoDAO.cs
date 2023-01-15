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
        IMongoCollection<Entry> entriesCollection;  //ska det vara kvar?

        public MongoDAO(string connectionString, string database)
        {
            dbClient = new MongoClient(connectionString);
            this.database = this.dbClient.GetDatabase(database);
            entriesCollection = this.database.GetCollection<Entry>("Entries"); //ska det vara kvar?

        }

        public async Task CreateEntryAsync(Entry entry)         
        { 
            
            //var collection = database.GetCollection<BsonDocument>("Entries");
            //var document = new BsonDocument
            //{
            //    {"date", DateTime.Now.ToString()},
            //    {"title", title},
            //    {"content",  content}
            //};
            //await collection.InsertOneAsync(document);
            await entriesCollection.InsertOneAsync(entry);
        }

        public async Task <bool> DeleteEntryAsync(string date)
        {
            var filter = Builders<Entry>.Filter.Eq("date", date);
            var result = await entriesCollection.DeleteOneAsync(filter);
            return result.DeletedCount != 0;

        }

        //public async Task<List<Entry>> GetAllEntries()
          public List<Entry> GetAllEntries()
        {
            return entriesCollection.Find(new BsonDocument()).ToList();
            //return await entriesCollection.Find(new BsonDocument()).ToListAsync();
        }

        public List<Entry> GetEntriesByFilter(string fieldName, string fieldValue)
        {
            var filter = Builders<Entry>.Filter.Eq(fieldName, fieldValue);
            var result = entriesCollection.Find(filter).ToList();

            return result;
        }

        public async Task <bool> UpdateEntryAsync(string date, string content)
        {
            var filter = Builders<Entry>.Filter.Eq("date", date);
            var update = Builders<Entry>.Update.Set("content", content);

            var result = await entriesCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount != 0;
        }
    }
}
