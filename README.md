# BigFileProcessor

This project is my solution to a provided challenge.
The purpose of the application is to **monitor a specified directory and process files that appear there**.

Files are processed in batches and saved into an MS SQL database. For large files, processing happens in bulks to avoid excessive memory usage. After each successful batch save, the current progress is written into a checkpoint file. This allows the next run to continue from the last successful line instead of reprocessing the file from the beginning.

* If processing **fails**, the file is moved to the `failed` folder.
* If processing **succeeds**, the file is moved to the `processed` folder.

The monitored directory, output folders, and bulk sizes are configurable via the `appsettings.json` file.

---

## Prerequisites

* .NET 9 SDK
* MS SQL Server
* (Optional) Docker with Docker Compose for running SQL Server locally

The project includes a `compose.yaml` file to start a local SQL Server instance
Your machine must be prepared to use Docker if you choose this option.

---

### Quick Start
1. Start SQL Server with Docker

   In terminal run command `docker-compose up`

   This will
    * run an MS SQL Server instance locally, accessible at localhost:1433
    * create `BigFileProcessorDb` database.

   If you don’t want to use Docker, you can start a server instance and create the database manually. For example, in SQL Server Management Studio (SSMS):

   `CREATE DATABASE BigFileProcessorDb`


2. Update configuration

   Edit appsettings.json with your desired connection string and file paths (see Configuration).


3. Run the application

   In terminal run command: `dotnet run --project BigFileProcessor --configuration Release`


4. Drop files into the incoming folder

   Files will be detected automatically.

   Successfully processed files -> moved to processed.

   Failed files -> moved to failed.

---

## Configuration

Before the first run, update all required fields in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=BigFileProcessorDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=true;"
},
"FileWatcher": {
  "BaseDirectory": "C:\\Data\\BigFileProcessor",
  "WatchFolderName": "incoming",
  "ProcessedFolderName": "processed",
  "FailedFolderName": "failed"
},
"FileProcessing": {
  "BoxCountInRam": "10000",
  "SqlBatchSize": "10000"
}
```

### Notes

* On the first run, the application will:
    * Create the required directories if they do not exist.
    * Execute an SQL script to generate the required tables in the database.
* Depending on your SQL Server setup, appropriate **permissions** may be required.

---

## Status

The application is **not production-ready**.

Below are potential improvements and considerations:

---

## If more time, here are things I would improve:

### Performance

1. **Database bottleneck** - currently, the app writes sequentially with a single connection. Parallel writes with multiple connections could improve performance, but ID generation must be reconsidered (e.g., use GUIDs or database-generated IDs).
2. **Identifiers** - currently, IDs are assigned in code assuming that `Box.Identifier` is not unique. If it is unique, we could drop ID assignment and use the identifier directly.
3. **Batching logic** - batches are limited by **box count**. If a single box has an unusually large number of contents, this may overload memory. Refactoring to limit by **row count** would be safer.
4. **Indexes** - the `Box -> Content` relation has no foreign key index. For read-heavy use cases, an index should be added.

    * An alternative is bulk inserting into a staging table without indexes, building indexes afterward, and then performing a partition swap into the main table for atomic updates.
5. **Memory leaks** - some objects remain referenced after each run. While memory eventually drops on processing the next file, resource disposal could be improved.

### Security

1. Raw SQL is currently used -> input sanitization and/or parameterized queries should be applied to mitigate SQL injection risks.

### Resilience

1. Add unit and integration test coverage.
2. Add retry mechanisms for database operations.
3. Implement input validation - each row should be checked after parsing to handle invalid data.
4. Improve error handling and add structured logging.

### Scalability

1. If huge data volumes are expected, I would consider using a **NoSQL database** for better scalability.

### Architecture/Readability

* Separate **Core** logic and **Infrastructure** into dedicated projects.
* Naming - some names don’t feel right, but I’m not sure what better alternatives would be.