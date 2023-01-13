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

## 2- Activity process
### 1-2 - Indexing method
The indexing of this project is consecutively from 0 to 255. In this way, whenever a record is to be added, the new id is equal to lasted+1 and its pointer is equal to the length of the entire record file. The record values, which are equal to 300 bits, are added to the end of the record file.

### 2-2 How to delete a record
First, it searches for the selected ID in the sorted list and finds its related pointer. This pointer indicates the start of the record in the record file

### 2-3 how to add again in case of previous deletion
If we have already deleted and want to add to a file again, the process of adding will change. The ID that should be added should actually be deleted. The consecutiveness of the ideas helps us that the subtraction of two consecutive ideas is always 1. With this principle, we can find the target values.
