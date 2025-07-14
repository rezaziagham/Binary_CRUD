# Binary CRUD

A binary-based CRUD (Create, Read, Update, Delete) implementation using two binary files: one for records and one for indexing.

## 📁 Project Structure

- **Record File**: Stores actual data entries (fixed-length binary format).
- **Index File**: Stores ID-pointer pairs, mapping each ID to a record location in the record file.

## 🧱 1. Storage Design

### 1.1 Fixed-Length Records

Each record is a fixed 300-bit block. Field lengths are pre-defined (e.g., `Fname` is always 10 characters). If a user inputs `"ali"` for a 10-character field, the rest is padded/truncated accordingly.

This approach ensures:
- Consistent record size
- Fast seeking using offsets

### 1.2 Index File Format

The index file maintains:
- `ID`: 2 bytes
- `Pointer`: 4 bytes

Each pointer indicates the **start byte offset** of the corresponding record in the record file. Since all records are fixed-size, the end of a record is simply `POINTER + RECORD_LENGTH`.

### 1.3 Non-Clustered Index

Initially, the index entries are sorted. However, after multiple insertions and deletions, they may become unsorted—typical of non-clustered indexes.

> Example of non-clustered layout:
>
> ![Non-clustered index](https://dataschool.com/assets/images/sql-optimization/how_to_index/indexToTable.png)

---

## ⚙️ 2. CRUD Process

### 2.1 Add Record

- ID is assigned sequentially (`lastID + 1`)
- Pointer = Current size of record file
- New record (300 bits) is appended at the end

### 2.2 Delete Record

- Search for the `ID` in the index file
- Retrieve its pointer
- Mark the ID as deleted (logic-dependent; e.g., overwrite or nullify)

### 2.3 Reuse Deleted Slots

If a record is deleted, its ID can be reused for future insertions. This is verified using consecutive differences between IDs:

```text
Original:     [1, 2, 3, 4, 5, 6]
After delete: [1, 2,    4, 5, 6]

Check difference:
2-1 = 1 ✅
4-2 = 2 ❌ ← Gap detected (ID 3 deleted)
