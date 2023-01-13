using BinCrud.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    static class IdSupport
    {
        //cAdditional information in Progrma.cs
        const int ID_LENGTH = 8;
        const int POINTER_LENGTH = 16;
        const int INDEX_LENGTH = ID_LENGTH + POINTER_LENGTH;
        const int FNAME_LENGTH = 10 * 8;
        const int LNAME_LENGTH = 15 * 8;
        const int MAJOR_LENGTH = 10 * 8;
        const int SALARY_LENGTH = 5 * 4;
        const int RECORD_LENGTH = FNAME_LENGTH + LNAME_LENGTH + MAJOR_LENGTH + SALARY_LENGTH;
        public static index Generateindex()
        {
            ///ids are sequnce.
            ///it means that for example if we delete id=4
            ///subtraction of id=3 and id=5 will give us 2.
            ///[,1,2,3,5,6,7]
            ///2-1=1
            ///3-2=1
            ///5-3=2
            ///6-5=1
            ///7-6=1
            ///for this reason we found that there we have a lost id
            /// after finding id , its pointer refer to last position of records file
            /// 
            ///=====================================================================
            ///
            ///this region of codes , read index file and seperate it in 24bit
            ///each 24 bit will made new index object(it has ID and POINTER)
            ///finally we got list of indices
            #region
            Binary bin = new();
            IO IOindexes = new(new ConstVariable().IndicesPath);
            string indexes = IOindexes.ReadAll();
            IO IOrecord = new(new ConstVariable().RecordsPath);
            string record = IOrecord.ReadAll();
            List<index> indexesList = new();
            if (indexes == "")
            {
                return new index() { id= 0,pointer=0};
            }
            //update();
            for (int i = 0; i < indexes.Length; i = i + INDEX_LENGTH)
            {
                index index = new index();
                index.id = bin.ToInt(indexes.Substring(i, ID_LENGTH));
                index.pointer = bin.ToInt(indexes.Substring(i + ID_LENGTH, POINTER_LENGTH));
                indexesList.Add(index);
            }
            #endregion

            //finding lost id
            for (int i = 0; i < indexesList.Count; i++)
            {
                if (indexesList[i].id!=i)
                {
                    return new index() { id = i, pointer = lastpointer() };
                }
            }
            //If it has not been returned yet, it means that we do not have a lost ID
            //then return lastid+1 in indices file and pointer refer to alst position in records file
            return new index() { id = indexesList.Last().id+1, pointer =record.Length};
        }
        public static int lastpointer()
        {
            //return last poistion in recrod file
            IO IOrecord = new(new ConstVariable().RecordsPath);
            string records = IOrecord.ReadAll();
            return records.Length;
        }
        public static void update()
        {
            ///this region of codes , read index file and seperate it in 24bit
            ///each 24 bit will made new index object(it has ID and POINTER)
            ///finally we got list of indices
            #region
            Binary bin = new();
            IO IOindexes = new(new ConstVariable().IndicesPath);
            string indexes = IOindexes.ReadAll();
            List<index> indexesList = new();
            for (int i = 0; i < indexes.Length; i = i + INDEX_LENGTH)
            {
                index index = new index();
                index.id = bin.ToInt(indexes.Substring(i, ID_LENGTH));
                index.pointer = bin.ToInt(indexes.Substring(i + ID_LENGTH, POINTER_LENGTH));
                indexesList.Add(index);
            }
            #endregion
            //sort objec-index and write sequentially in file
            List<index> indices = indexesList.OrderBy(x => x.id).ToList();
            string Allindexes = "";
            foreach (var item in indices)
            {
                Allindexes= Allindexes +bin.ToBinary( item.id,2) +bin.ToBinary( item.pointer,4);
            }
            IOindexes.WriteAll(Allindexes);
        }
        public static int GetPointer(int id)
        {
            ///this region of codes , read index file and seperate it in 24bit
            ///each 24 bit will made new index object(it has ID and POINTER)
            ///finally we got list of indices
            #region
            Binary bin = new();
            IO IOindexes = new(new ConstVariable().IndicesPath);
            string indexes = IOindexes.ReadAll();
            List<index> indexesList = new();
            for (int i = 0; i < indexes.Length; i = i + INDEX_LENGTH)
            {
                index index = new index();
                index.id = bin.ToInt(indexes.Substring(i, ID_LENGTH));
                index.pointer = bin.ToInt(indexes.Substring(i + ID_LENGTH, POINTER_LENGTH));
                indexesList.Add(index);
            }
            #endregion

            //find index
            index? foundindex =indexesList.Find(x => x.id == id);
            if (foundindex==null)
            {
                return -1;
            }
            //return pointer
            return foundindex.pointer;
        }

    }
}
