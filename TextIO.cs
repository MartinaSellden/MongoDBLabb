using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBLabb
{
    internal class TextIO : IUI
    {
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            System.Environment.Exit(0);
        }

        public string GetString()
        {
            return Console.ReadLine();
        }

        public void PrintString(string output)
        {
            Console.WriteLine(output);
        }
    }
}
