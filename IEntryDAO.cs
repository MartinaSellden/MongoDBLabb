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
        List<Entry> GetAllEntries();
        List<Entry> GetEntriesByFilter(string fieldName, string fieldValue);
        Task CreateEntryAsync(Entry entry);
        Task <bool> UpdateEntryAsync(string date, string content);
        Task <bool> DeleteEntryAsync(string date);

    }
}
