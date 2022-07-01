summer22

Database setup:
```
const database = 'FourKeyMetrics';
const collection = 'deployments';

// The current database to use.
use(database);

// Create a new collection.
db.createCollection(collection);
db.deployments.createIndex( { "Platform": 1, "Organization": 1, "Project": 1, "Definition": 1, "Repository": 1, "StartTime": 1, "FinishTime": 1 }, { unique: true } )
```

