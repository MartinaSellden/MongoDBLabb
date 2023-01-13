using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBLabb
{
    internal interface IEntryDAO
    {
        List <string> GetAllEntries();
        List<string> GetEntriesByFilter(string key);
        void CreateEntry();
        void UpdateEntry();
        void DeleteEntry();

    }
}
