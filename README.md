# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

# Setup
Run sql script in Design.md

# NOTE
A worker service exist in application layer to send alert
# Connection String
    "SemanixDbContext": "Server=localhost;Initial Catalog=SemanixDb;Encrypt=False;TrustServerCertificate=False;User ID={{your id}};Password={{your password}};Trusted_Connection=True;"

# Curl Script
ALERT

Get: get-alert
curl -X 'GET' \
  'https://localhost:44323/api/alert/get-alert-by-id' \
  -H 'accept: text/plain' \
  -H 'X-Tenant-Id: 250'

Ticket
Post: Post ticket
curl -X 'POST' \
  'https://localhost:44323/api/ticket/add-ticket' \
  -H 'accept: text/plain' \
  -H 'X-Tenant-Id: 250' \
  -H 'Content-Type: application/json' \
  -d '{
  "title": "Server down",
  "description": "server is down",
  "priority": 0
}'

Get: Ticket
curl -X 'GET' \
  'https://localhost:44323/api/ticket/get-ticket-by-id' \
  -H 'accept: text/plain' \
  -H 'X-Tenant-Id: 250'

Post: Update ticket
curl -X 'POST' \
  'https://localhost:44323/api/ticket/update-ticket-by-id' \
  -H 'accept: text/plain' \
  -H 'X-Tenant-Id: 250' \
  -H 'Content-Type: application/json' \
  -d '{
  "ticketId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "newStatus": 3
}'

# Scalability
The Architecture helps an app scale by
 Making it easier to add features
1. Enabling teams to work in parallel
2. Supporting testable, modular design
3. Facilitating long-term maintainability
4. Preparing the app for distributed systems

The apps combine CQRS (Command Query Responsibility Segregation) with Clean Architecture for read/write separation, which improves:

1. Performance (e.g., optimized read models)
2. Scalability (different services for reads/writes)
3. Reliability (fewer dependencies per service)

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

# NOTE
This app Architecture is mostly about code structure, it indirectly supports runtime scaling because it:

Enables:
1. Easy Microservice Splitting: You can pull out certain use cases or modules into separate services when needed.
2. Improved Performance Tuning: You can isolate and optimize only the infrastructure components (e.g., caching, DB, message brokers) without rewriting core logic.

# Async Patterns: Supports CQRS and event-driven designs (queues, handlers) which scale well in distributed systems.
The actual horizontal/vertical scaling (load balancing, databases, containers) is done by infrastructure tools (e.g., Kubernetes, Docker, Azure, AWS), not Clean Architecture directly.

# Codebase / Team Scalability
This Architecture helps the app scale organizationally and logically as more features, modules, or developers are added.
1. Separation of Concerns: Teams can work independently on UI, business logic, or infrastructure.
2. Loosely Coupled Code: Easy to swap database, queue, email provider, etc. without affecting core logic.
3. Testability: Unit tests can target business logic without touching UI or DB.
4. Maintainability: Clear structure prevents spaghetti code as the project grows.
4. Pluggability: new features (e.g., another API endpoint or message handler) doesn't affect existing logic.

# Build and Test
TODO: Describe and show how to build your code and run the tests. 

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)