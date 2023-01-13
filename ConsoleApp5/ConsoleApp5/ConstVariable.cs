using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinCrud.Classes
{
    //calss for return paths of indices and records
    class ConstVariable
    {
        public ConstVariable()
        {
            //creat files in each PC (C:\\...\\document\\....
            string PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\exercises";
            if (!Directory.Exists(PATH))
            {
                Directory.CreateDirectory(PATH);
                File.Create(PATH + "\\Records.bin");
                File.Create(PATH + "\\Indices.bin");
            }
            this.RecordsPath = PATH + "\\Records.bin";
            this.IndicesPath = PATH + "\\Indices.bin";
        }
        //all records path in project
        public string RecordsPath { get; } /*= Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Records.bin");*/
        public string IndicesPath { get; } //Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Indices.bin");
        
        //public readonly string BinRecordLength =;
    }
}
