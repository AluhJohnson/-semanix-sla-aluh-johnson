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

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

# Build and Test
TODO: Describe and show how to build your code and run the tests. 

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)