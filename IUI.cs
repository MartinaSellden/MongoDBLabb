using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBLabb
{
    internal interface IUI
    {
        public string GetString();
        public void PrintString(string output);
        public void Clear();
        public void Exit();

    }
}
