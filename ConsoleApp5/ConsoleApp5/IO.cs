using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinCrud.Classes
{
    class IO
    {
        //constractor
        public IO(string Path)
        {
            this.Path = Path;
        }
        //file's path
        public string Path { get; set; }
        //read all bit sequentially
        public string ReadAll()
        {
            return File.ReadAllText(Path, Encoding.UTF8);
        }
        //write bits
        public void Write(string Record)
        {
            File.AppendAllText(Path, Record);
        }
        //write list of bits
        public void Write(List<string> Records)
        {
            foreach (string Record in Records)
            {
                Write(Record);
            }
        }
        //replace all bits with new bits
        public void WriteAll(string Records)
        {
            File.WriteAllText(Path, Records);
        }
    }
}
