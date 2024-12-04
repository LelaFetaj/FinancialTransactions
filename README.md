Objective:

Develop a Financial Transactions Management API using .NET. The API should allow
users to perform CRUD (Create, Read, Update, Delete) operations on Transaction
entities, and generate a summary report.

Requirements:

1. Environment Setup:
• Use the latest.NET version
• Implement Entity Framework as the ORM or propose a more suitable
alternative for performance and maintainability
• Use SQL Server for the database

2. Entity Definition: A transaction (and relations where necessary) should contain
atleastthe following:
• TransactionId
• Amount
• TransactionType (either "Credit" or"Debit").
• TransactionDate
• Description
• Status (Failed, Successful, Voided)
• Customer Full name
• Customer main phone number
• Customer main address
• Customer main email address

3. API Endpoints:Implement the following API endpoints:
• Create a Transaction: POST /api/transactions
• Get All Transactions: GET /api/transactions (provide
filtering options)
• Get a Transaction by ID: GET /api/transactions/{id}
• Update a Transaction: PUT /api/transactions/{id}
• Delete a Transaction: DELETE /api/transactions/{id}
(provide different deletion patterns)
• Get Transaction Summary: GET /api/transactions/summary
▪ Response: A JSON object containing the total number of
transactions, total credits, total debits, and net balance. Provide
filtering options based on the customer data.

4. Documentation:
• Use Swagger to generate API documentation.

The task is designed to evaluate your technical skills and your ability to implement
design patterns and best practices. You are encouraged to provide complex
solutions.
