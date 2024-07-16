This repository was created as a learning exercise to familiarize with the integration of Keycloak with ASP.NET Core Web API. It serves as a practical example of authentication and authorization using Keycloak, including user registration, login, and permission management.

## Features

- User Registration: Allows new users to register in the system with input data validation.
- Login: Authenticates existing users using the provided credentials.
- Get Users: Retrieves a list of all registered users (requires admin permission).

## Technologies Used

- ASP.NET Core Web API
- Keycloak

## Endpoints

### Get All Users

**GET api/v1/users**

- Authorization: Requires the user to be an Admin (Policy = "Admin").

Retrieves a list of all registered users.

**Response**

- 200 OK Returns a list of users.

### Register User

**POST api/v1/register**

Registers a new user with the provided details.

**Request**

```
{
  "UserName": "string",
  "Email": "string",
  "Password": "string"
}
```

**Response**

- 200 OK: Returns the details of the registered user.
- 500 Internal Server Error: If an error occurs while logging in the user.

**Response Example**

```
{
    "User": {
        "Id": "7fcc019f-7dc5-43e6-b386-f50bc3beef9e",
        "UserName": "and1211111",
        "Email": "app_user111111111@test.com"
    },
    "AccessToken": "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJ6VmVqTjA1NTdGbW5ERHF5QmV6bVBjbmxrR2RiYlJaUUo0b3ZZd2w0SGZzIn0.eyJleHAiOjE3MjEwMTU0MTMsImlhdCI6MTcyMTAxNDgxMywianRpIjoiMjc3YjAyZTQtYmI3Mi00Mjk5LTkyMDItYWM0N2ZhNWIwN2NjIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rZXljbG9hY2t0ZXN0IiwiYXVkIjoiYWNjb3VudCIsInN1YiI6IjdmY2MwMTlmLTdkYzUtNDNlNi1iMzg2LWY1MGJjM2JlZWY5ZSIsInR5cCI6IkJlYXJlciIsImF6cCI6ImtleWNsb2FrLWNsaWVudCIsInNlc3Npb25fc3RhdGUiOiJiODU3OTc4NS0yYjUzLTRlMGUtYTM3MS00MTE1YTA1MmY2MWQiLCJhY3IiOiIxIiwiYWxsb3dlZC1vcmlnaW5zIjpbImh0dHBzOi8vbG9jYWxob3N0OjcyMTIiXSwicmVhbG1fYWNjZXNzIjp7InJvbGVzIjpbImRlZmF1bHQtcm9sZXMta2V5Y2xvYWNrdGVzdCIsIm9mZmxpbmVfYWNjZXNzIiwidW1hX2F1dGhvcml6YXRpb24iXX0sInJlc291cmNlX2FjY2VzcyI6eyJhY2NvdW50Ijp7InJvbGVzIjpbIm1hbmFnZS1hY2NvdW50IiwibWFuYWdlLWFjY291bnQtbGlua3MiLCJ2aWV3LXByb2ZpbGUiXX19LCJzY29wZSI6ImtleWNsb2FrLXNjb3BlIHByb2ZpbGUgZW1haWwiLCJzaWQiOiJiODU3OTc4NS0yYjUzLTRlMGUtYTM3MS00MTE1YTA1MmY2MWQiLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsInByZWZlcnJlZF91c2VybmFtZSI6ImFuZDEyMTExMTEiLCJlbWFpbCI6ImFwcF91c2VyMTExMTExMTExQHRlc3QuY29tIn0.baoIWE26cHooWzJZh89KomF69mVxErfcyTnsPBcfqaxaUtEUO-uzaukWOS5xOWlCtqxFxRcuiLuAskjzGKhXl7iyYHFU9Zc7GwXetS6iCrWSPQ6ruXkOi7dZvvU3ngHg7Crx7a360dKMwfFsx168znOj9JyiV56sp4btSvc63E__Un2GvyzBI-97UTVeWKUNo6Q_7NYioT1URjQyasnn2LvoMVuIRj0Gutc7KRZHEc6rjiK7bt7uRLHez0mDIppnKvRQS_31h_FLLfl_1ynBJPn2I3uQb1pEGsnXNGQXL98Cgj9hTPkGn-AsuOnBHvcParXY76gj9xjOQuivURvA_A",
    "RefreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICIzNGQxYmZlMS1jZTUwLTQ2YzgtYTZiMy0wYzRkYjhmNGUxZjUifQ.eyJleHAiOjE3MjIyMjQ0MTMsImlhdCI6MTcyMTAxNDgxMywianRpIjoiMWFhY2QwYzEtYjU3Ny00ODkyLWI0YTgtZmQxYTJiOTQyNDVmIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rZXljbG9hY2t0ZXN0IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rZXljbG9hY2t0ZXN0Iiwic3ViIjoiN2ZjYzAxOWYtN2RjNS00M2U2LWIzODYtZjUwYmMzYmVlZjllIiwidHlwIjoiUmVmcmVzaCIsImF6cCI6ImtleWNsb2FrLWNsaWVudCIsInNlc3Npb25fc3RhdGUiOiJiODU3OTc4NS0yYjUzLTRlMGUtYTM3MS00MTE1YTA1MmY2MWQiLCJzY29wZSI6ImtleWNsb2FrLXNjb3BlIHByb2ZpbGUgZW1haWwiLCJzaWQiOiJiODU3OTc4NS0yYjUzLTRlMGUtYTM3MS00MTE1YTA1MmY2MWQifQ.h4BTDLECrgYsRGGe6lIg_0QnyZysfAMXB-HJQKgjINI"
}
```

### User Login

**POST api/v1/login**

Authenticates a user with the provided email and password.

**Request**

```
{
  "Email": "string",
  "Password": "string"
}
```

**Response**

- 200 OK: Returns the authenticated user details.
- 500 Internal Server Error: If an error occurs while logging in the user.

**Response Example**

```
{
    "User": {
        "Id": "7fcc019f-7dc5-43e6-b386-f50bc3beef9e",
        "UserName": "and1211111",
        "Email": "app_user111111111@test.com"
    },
    "AccessToken": "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJ6VmVqTjA1NTdGbW5ERHF5QmV6bVBjbmxrR2RiYlJaUUo0b3ZZd2w0SGZzIn0.eyJleHAiOjE3MjEwMTU0MTMsImlhdCI6MTcyMTAxNDgxMywianRpIjoiMjc3YjAyZTQtYmI3Mi00Mjk5LTkyMDItYWM0N2ZhNWIwN2NjIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rZXljbG9hY2t0ZXN0IiwiYXVkIjoiYWNjb3VudCIsInN1YiI6IjdmY2MwMTlmLTdkYzUtNDNlNi1iMzg2LWY1MGJjM2JlZWY5ZSIsInR5cCI6IkJlYXJlciIsImF6cCI6ImtleWNsb2FrLWNsaWVudCIsInNlc3Npb25fc3RhdGUiOiJiODU3OTc4NS0yYjUzLTRlMGUtYTM3MS00MTE1YTA1MmY2MWQiLCJhY3IiOiIxIiwiYWxsb3dlZC1vcmlnaW5zIjpbImh0dHBzOi8vbG9jYWxob3N0OjcyMTIiXSwicmVhbG1fYWNjZXNzIjp7InJvbGVzIjpbImRlZmF1bHQtcm9sZXMta2V5Y2xvYWNrdGVzdCIsIm9mZmxpbmVfYWNjZXNzIiwidW1hX2F1dGhvcml6YXRpb24iXX0sInJlc291cmNlX2FjY2VzcyI6eyJhY2NvdW50Ijp7InJvbGVzIjpbIm1hbmFnZS1hY2NvdW50IiwibWFuYWdlLWFjY291bnQtbGlua3MiLCJ2aWV3LXByb2ZpbGUiXX19LCJzY29wZSI6ImtleWNsb2FrLXNjb3BlIHByb2ZpbGUgZW1haWwiLCJzaWQiOiJiODU3OTc4NS0yYjUzLTRlMGUtYTM3MS00MTE1YTA1MmY2MWQiLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsInByZWZlcnJlZF91c2VybmFtZSI6ImFuZDEyMTExMTEiLCJlbWFpbCI6ImFwcF91c2VyMTExMTExMTExQHRlc3QuY29tIn0.baoIWE26cHooWzJZh89KomF69mVxErfcyTnsPBcfqaxaUtEUO-uzaukWOS5xOWlCtqxFxRcuiLuAskjzGKhXl7iyYHFU9Zc7GwXetS6iCrWSPQ6ruXkOi7dZvvU3ngHg7Crx7a360dKMwfFsx168znOj9JyiV56sp4btSvc63E__Un2GvyzBI-97UTVeWKUNo6Q_7NYioT1URjQyasnn2LvoMVuIRj0Gutc7KRZHEc6rjiK7bt7uRLHez0mDIppnKvRQS_31h_FLLfl_1ynBJPn2I3uQb1pEGsnXNGQXL98Cgj9hTPkGn-AsuOnBHvcParXY76gj9xjOQuivURvA_A",
    "RefreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICIzNGQxYmZlMS1jZTUwLTQ2YzgtYTZiMy0wYzRkYjhmNGUxZjUifQ.eyJleHAiOjE3MjIyMjQ0MTMsImlhdCI6MTcyMTAxNDgxMywianRpIjoiMWFhY2QwYzEtYjU3Ny00ODkyLWI0YTgtZmQxYTJiOTQyNDVmIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rZXljbG9hY2t0ZXN0IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rZXljbG9hY2t0ZXN0Iiwic3ViIjoiN2ZjYzAxOWYtN2RjNS00M2U2LWIzODYtZjUwYmMzYmVlZjllIiwidHlwIjoiUmVmcmVzaCIsImF6cCI6ImtleWNsb2FrLWNsaWVudCIsInNlc3Npb25fc3RhdGUiOiJiODU3OTc4NS0yYjUzLTRlMGUtYTM3MS00MTE1YTA1MmY2MWQiLCJzY29wZSI6ImtleWNsb2FrLXNjb3BlIHByb2ZpbGUgZW1haWwiLCJzaWQiOiJiODU3OTc4NS0yYjUzLTRlMGUtYTM3MS00MTE1YTA1MmY2MWQifQ.h4BTDLECrgYsRGGe6lIg_0QnyZysfAMXB-HJQKgjINI"
}
```
