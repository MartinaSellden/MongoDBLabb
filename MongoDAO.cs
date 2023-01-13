using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBLabb
{
    internal class MongoDAO :IEntryDAO
    {
        MongoClient dbClient;
        IMongoDatabase database;    

        public MongoDAO(string connectionString, string database)
        {
            dbClient = new MongoClient(connectionString);
            this.database = this.dbClient.GetDatabase(database);

        }

        public void CreateEntry()
        {
            throw new NotImplementedException();
        }

        public void DeleteEntry()
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllEntries()
        {
            throw new NotImplementedException();
        }

        public List <string> GetEntriesByFilter(string key)
        {
            throw new NotImplementedException();
        }

        public void UpdateEntry()
        {
            throw new NotImplementedException();
        }
    }
}
