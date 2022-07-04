summer22

Database setup:
```
const database = 'FourKeyMetrics';

// The current database to use.
use(database);

// Create a new collection.
db.createCollection('deployments');
db.deployments.createIndex( { "Platform": 1, "Organization": 1, "Project": 1, "Definition": 1, "Repository": 1, "StartTime": 1, "FinishTime": 1 }, { unique: true } )
db.createCollection('changes');
db.changes.createIndex( { "Platform": 1, "Organization": 1, "Project": 1, "Repository": 1, "Branch": 1, "PullRequestId": 1, "StartTime": 1}, { unique: true } )
```

