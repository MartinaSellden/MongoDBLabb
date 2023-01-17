using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBLabb
{
    internal interface IEntryDAO
    {
        //Task<List<Entry>> GetAllEntries();
        List<EntryODM> ReadAllEntries();
        List<EntryODM> ReadEntriesByFilter(string fieldName, string fieldValue);
        Task CreateEntryAsync(EntryODM entry);
        Task UpdateEntryAsync(string date, string content);
        Task DeleteEntryAsync(string date);

    }
}
