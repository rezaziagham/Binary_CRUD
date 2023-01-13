# Binary_CRUD

## Binary-based CRUD implementation project.
This implementation includes two files, index and record, to view it, go to the C drive of the document folder.
The records without id are stored in the record file and the ids along with their pointer are stored in the index file.

## 1-Organize files
A record contains characteristics, each of which hypothetically has a range.

### 1-1-Fixed length record
Each of our codes has a fixed length regardless of whether the length of the value entered by the user is less than the range. For example, if we consider the range of 10 characters for Fname and the user enters the value "ali". The value of "ali" is stored in the file. This means that the user does not have the right to add more than 10 characters to the variable.

