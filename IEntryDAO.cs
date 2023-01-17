using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBLabb
{
    internal interface IEntryDAO
    {
        Task CreateEntryAsync(EntryODM entry);
        List<EntryODM> ReadAllEntries();
        List<EntryODM> ReadEntriesByFilter(string fieldName, string fieldValue);
        EntryODM ReadEntryById(ObjectId id);
        Task UpdateEntryAsync(ObjectId id, string content);
        Task DeleteEntryAsync(ObjectId id);
    }
}
