```mermaid
erDiagram
	User -- Tracking : references

	User {
		INTEGER Id
		VARCHAR(255) Guid
		TIMESTAMP CreatedAt
	}

	Tracking {
		INTEGER Id
		INTEGER Type
		TIMESTAMP TimeStamp
		INTEGER UserId
	}
```