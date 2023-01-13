# Binary_CRUD

## Binary-based CRUD implementation project.
This implementation includes two files, index and record, to view it, go to the C drive of the document folder.
The records without id are stored in the record file and the ids along with their pointer are stored in the index file.

## 1-Organize files
A record contains characteristics, each of which hypothetically has a range.

### 1-1 Fixed length record
Each of our codes has a fixed length regardless of whether the length of the value entered by the user is less than the range. For example, if we consider the range of 10 characters for Fname and the user enters the value "ali". The value of "ali" is stored in the file. This means that the user does not have the right to add more than 10 characters to the variable.
With this assumption, we limit all record characteristics and finally all records will be limited forever. In the project, the length of a record is 300 bits.

### 1-2 index file
The index file contains id and pointer, which is a two-bit id and a four-bit pointer. Each pointer shows the starting location of that id in the record file. Because the range of bits of a record is known, then the end of a record is equal to POINTER+RECORD_LENGTH.

### 1-3 non-clustered index
At first, adding records and IDs are sorted in the file. But by successive additions and deletions of the index, it comes out in a random way (as shown in the figure below).
![non-clustered index](https://dataschool.com/assets/images/sql-optimization/how_to_index/indexToTable.png)
