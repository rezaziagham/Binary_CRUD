using BinCrud.Classes;
using ConsoleApp5;
using System;
using System.Linq;

//in index file , id length equal 8bit(2byte)
const int ID_LENGTH = 8;
//in index file , pointer length equal 16bit(4byte)
const int POINTER_LENGTH = 16;
//in index file , id-record (consist of ids an their pointers) length equal 24bit(6byte)
const int INDEX_LENGTH = ID_LENGTH + POINTER_LENGTH;
// fname equal 10 charecter and each of them equal 8bit
const int FNAME_LENGTH = 10 * 8;
// lname equal 15 charecter and each of them equal 8bit
const int LNAME_LENGTH = 15 * 8;
// major equal 10 charecter and each of them equal 8bit
const int MAJOR_LENGTH = 10 * 8;
// salary equal 5 charecter and each of them equal 4bit(its and inteager value)
const int SALARY_LENGTH = 5 * 4;
//sum all length . exactlly equal 300bit
const int RECORD_LENGTH = FNAME_LENGTH + LNAME_LENGTH + MAJOR_LENGTH + SALARY_LENGTH;

//this while for demostrate view in console
while (true)
{
    Console.WriteLine(">>enter commend");
    string? _ =Console.ReadLine();
    string[] input = _.Split(' ');
    try
    {
        switch (input[0])
        {
            case "help":
                help();
                break;
            case "showid":
                showid();
                break;
            case "delete":
                Delete(Convert.ToInt32(input[1]));
                show();
                break;
            case "find":
                Stu stu =Find(Convert.ToInt32(input[1]));
                Print(stu);
                break;
            case "add":
                add();
                show();
                break;
            case "exit":
                Environment.Exit(0);
                break;
            case "edit":
                Edit(Convert.ToInt32(input[1]));
                break;
            case "show":
                show();
                break;
            default:
                Console.WriteLine("commend is not correct . use 'help' for demonstrate all commends");
                break;
        }
    }
    catch (Exception ex) { Console.WriteLine(ex.Message); }
}
//just write all commend and their results
static void help()
{
    Console.WriteLine("\n----------------------------\n");
    Console.WriteLine("available commend\n");

    Console.WriteLine("add       || add one record");
    Console.WriteLine("delete    || used for delete record by id");
    Console.WriteLine("edit      || enter an id and edit thats properties");
    Console.WriteLine("show      || demonstrate all records");
    Console.WriteLine("showid    || demonstrate ids and their pointers");
    Console.WriteLine("Find      || enter an id and You will see its specifications");
    Console.WriteLine("exit      || exit");
    Console.WriteLine("\n----------------------------\n");

}
//delete record
static void Delete(int id)
{
    ///this region of codes , read index file and seperate it in 24bit
    ///each 24 bit will made new index object(it has ID and POINTER)
    ///finally we got list of indices
    #region
    Binary bin = new();
    IO IOrecords = new(new ConstVariable().RecordsPath);
    IO IOindexes = new(new ConstVariable().IndicesPath);
    string indexes = IOindexes.ReadAll();
    string record = IOrecords.ReadAll();
    List<index> indexesList = new();
    for (int i = 0; i < indexes.Length; i = i + INDEX_LENGTH)
    {
        index index = new index();
        index.id = bin.ToInt(indexes.Substring(i, ID_LENGTH));
        index.pointer = bin.ToInt(indexes.Substring(i + ID_LENGTH, POINTER_LENGTH));
        indexesList.Add(index);
    }
    #endregion

    ///order list of indices by their pointer.
    
    ///The reason for sorting the list is that when we are going to delete
    ///the index, all the pointers after them must change.
    List<index> indices = indexesList.OrderBy(x => x.pointer).ToList();
    
    //find pointer of selcted item 
    int foundindex = indices.FindIndex(x => x.id == id);
    if (foundindex == -1)
    {
        return;
    }
    int pointer = indices.First(x=>x.id==id).pointer;


    //remove record base on pointer
    record = record.Remove(pointer, RECORD_LENGTH);

    //Subtract RECORD_LENGTH from all pointers after deleting the item
    for (int i = foundindex; i < indices.Count; i++)
    {
        indices[i].pointer = indices[i].pointer - RECORD_LENGTH;
    }
    ///remove index from indices file
    indices.RemoveAt(foundindex);
    indices = indices.OrderBy(x => x.id).ToList();
    string Allindexes = "";
    foreach (var item in indices)
    {
        Allindexes = Allindexes + bin.ToBinary(item.id, 2) + bin.ToBinary(item.pointer, 4);
    }

    //write records and inex in their files
    IOrecords.WriteAll(record);
    IOindexes.WriteAll(Allindexes);
}
static void show()
{
    BinCrud.Classes.Binary Bin = new();

    //read sequential index and record files
    string Records = File.ReadAllText(new ConstVariable().RecordsPath);
    string indexes = File.ReadAllText(new ConstVariable().IndicesPath);

    //storage of record in obj
    List<Stu> RecordsList = new List<Stu>();

    //seperate indices file by 24bit and find record bi their poiner
    for (int i = 0; i < indexes.Length; i = i + INDEX_LENGTH)
    {
        Stu stu = new Stu();
        int pointer = Bin.ToInt(indexes.Substring(i + ID_LENGTH, POINTER_LENGTH));
        stu.id = Bin.ToInt(indexes.Substring(i, ID_LENGTH));
        stu.fname = Bin.ToString(Records.Substring(pointer, FNAME_LENGTH));
        stu.lname = Bin.ToString(Records.Substring(pointer + FNAME_LENGTH, LNAME_LENGTH));
        stu.major = Bin.ToString(Records.Substring(pointer + FNAME_LENGTH + LNAME_LENGTH, MAJOR_LENGTH));
        stu.salary = Bin.ToLong(Records.Substring(pointer + FNAME_LENGTH + LNAME_LENGTH + MAJOR_LENGTH, SALARY_LENGTH));
        RecordsList.Add(stu);
    }
    //show records
    print(RecordsList);
}
static void print(List<Stu> records)
{

    Console.WriteLine("=============================================");
    Console.WriteLine("ID ||Lname    ||Fname    ||major    ||salalry");
    Console.WriteLine("---------------------------------------------\n");

    for (int i = 0; i < records.Count; i++)
    {
        Print(records[i]);
        Console.WriteLine("\n");
    }
    Console.WriteLine("=============================================");

}
static void Print(Stu record)
{
    Console.WriteLine($"{record.id}  || {record.lname}  || {record.fname}   || {record.major}   || {record.salary}  ||");
}
static void add()
{
    Binary bin = new();
    string Fname = "";
    string Lname= "";
    string Major = "";
    int salary = 0;
    //for validating input (each property must have defind bit)
    #region
    while (true)
    {
        Console.WriteLine(">>Fname");
        Fname = Console.ReadLine();
        if (Fname.Length > 10)
        {
            Console.WriteLine( "Fname must be less than 10 char");
        }
        else { break; }
    }
    while (true)
    {
        Console.WriteLine(">>lname");
        Lname = Console.ReadLine();
        if (Lname.Length > 15)
        {
            Console.WriteLine("lname must be less than 10 char");
        }
        else { break; }
    }
    while (true)
    {
        Console.WriteLine(">>major");
        Major = Console.ReadLine();
        if (Major.Length > 10)
        {
            Console.WriteLine("Major must be less than 10 char");
        }
        else { break; }
    }
    while (true)
    {
        Console.WriteLine(">>salary");
        salary = Convert.ToInt32(Console.ReadLine());
        if (bin.ToBinary(Convert.ToInt64(salary)).Length > 20)
        {
            Console.WriteLine( "Salary must be less than 5Byte(1048575)");
        }
        else { break;}
    }
    #endregion
    //generate new id and pointer (ids between 0 - 255)
    index index = IdSupport.Generateindex();
    //input/output record and index in their files
    IO IOrecord = new(new ConstVariable().RecordsPath);
    IO IOindex = new(new ConstVariable().IndicesPath);
    //creat new record
    Stu stu = new();
    stu.fname = Fname;
    stu.lname = Lname;
    stu.major = Major;
    stu.salary = salary;
    //convert index to 24bit 0/1
    string indx = bin.ToBinary(index.id, 2) + bin.ToBinary(index.pointer, 4);
    //convert record to 300bit 0/1
    string record = bin.ToBinary(stu.fname, 10) + bin.ToBinary(stu.lname, 15) + bin.ToBinary(stu.major, 10) + bin.ToBinary(stu.salary, 5);
    //write records and indices
    IOrecord.Write(record);
    IOindex.Write(indx);
    //sort indices file by id
    IdSupport.update();
}
//show id and pointers
static void showid()
{
    string indices = File.ReadAllText(new ConstVariable().IndicesPath);
    BinCrud.Classes.Binary Bin = new();

    for (int i = 0; i < indices.Length; i = i + INDEX_LENGTH)
    {
        Console.WriteLine($"{Bin.ToInt(indices.Substring(i, ID_LENGTH))}    {Bin.ToInt(indices.Substring(i + ID_LENGTH, POINTER_LENGTH))} \n");
    }
    Console.WriteLine("===================");
}
static Stu Find(int id)
{
    ///this region of codes , read index file and seperate it in 24bit
    ///each 24 bit will made new index object(it has ID and POINTER)
    ///finally we got list of indices
    #region
    Binary bin = new();
    IO IOrecords = new(new ConstVariable().RecordsPath);
    IO IOindexes = new(new ConstVariable().IndicesPath);
    string indexes = IOindexes.ReadAll();
    string record = IOrecords.ReadAll();
    List<index> indexesList = new();
    for (int i = 0; i < indexes.Length; i = i + INDEX_LENGTH)
    {
        index index = new index();
        index.id = bin.ToInt(indexes.Substring(i, ID_LENGTH));
        index.pointer = bin.ToInt(indexes.Substring(i + ID_LENGTH, POINTER_LENGTH));
        indexesList.Add(index);
    }
    #endregion

    //find passed-id index 
    //If the entity was empty would be null
    //get pointer
    var entity = indexesList.FirstOrDefault(x => x.id == id);
    if (entity == null)
        return null;
    int pointer = entity.pointer;

    //creat stu by pointer
    Stu stu = new Stu();
    stu.id = entity.id;
    stu.fname = bin.ToString(record.Substring(pointer, FNAME_LENGTH));
    stu.lname = bin.ToString(record.Substring(pointer + FNAME_LENGTH, LNAME_LENGTH));
    stu.major = bin.ToString(record.Substring(pointer + FNAME_LENGTH + LNAME_LENGTH, MAJOR_LENGTH));
    stu.salary = bin.ToLong(record.Substring(pointer + FNAME_LENGTH + LNAME_LENGTH + MAJOR_LENGTH, SALARY_LENGTH));
    return stu;
}
static void Edit(int id)
{
    ///this region of codes , read index file and seperate it in 24bit
    ///each 24 bit will made new index object(it has ID and POINTER)
    ///finally we got list of indices
    #region
    Binary bin = new();
    IO IOrecords = new(new ConstVariable().RecordsPath);
    IO IOindexes = new(new ConstVariable().IndicesPath);
    string indexes = IOindexes.ReadAll();
    string records = IOrecords.ReadAll();
    List<index> indexesList = new();
    for (int i = 0; i < indexes.Length; i = i + INDEX_LENGTH)
    {
        index index = new index();
        index.id = bin.ToInt(indexes.Substring(i, ID_LENGTH));
        index.pointer = bin.ToInt(indexes.Substring(i + ID_LENGTH, POINTER_LENGTH));
        indexesList.Add(index);
    }
    #endregion

    //find index anf get its pointer
    var entity = indexesList.FirstOrDefault(x => x.id == id);
    if (entity == null)
        return;
    int pointer = entity.pointer;

    //fill new stu
    string Fname = "";
    string Lname = "";
    string Major = "";
    int salary = 0;
    //for validating input (each property must have defind bit)
    #region
    while (true)
    {
        Console.WriteLine(">>Fname");
        Fname = Console.ReadLine();
        if (Fname.Length > 10)
        {
            Console.WriteLine("Fname must be less than 10 char");
        }
        else { break; }
    }
    while (true)
    {
        Console.WriteLine(">>lname");
        Lname = Console.ReadLine();
        if (Lname.Length > 15)
        {
            Console.WriteLine("lname must be less than 10 char");
        }
        else { break; }
    }
    while (true)
    {
        Console.WriteLine(">>major");
        Major = Console.ReadLine();
        if (Major.Length > 10)
        {
            Console.WriteLine("Major must be less than 10 char");
        }
        else { break; }
    }
    while (true)
    {
        Console.WriteLine(">>salary");
        salary = Convert.ToInt32(Console.ReadLine());
        if (bin.ToBinary(Convert.ToInt64(salary)).Length > 20)
        {
            Console.WriteLine("Salary must be less than 5Byte(1048575)");
        }
        else { break; }
    }
    #endregion
    //convert record to 0/1 (it will be 300bit)
    string record = bin.ToBinary(Fname, 10) + bin.ToBinary(Lname, 15) + bin.ToBinary(Major, 10) + bin.ToBinary(salary, 5);
    //remove old record and put new record 
    records = records.Remove(pointer, RECORD_LENGTH);
    records=records.Insert(pointer,record);
    //write records in file
    IOrecords.WriteAll(records);
}