# Asp-Dotnet-Core-Api

Template project for Asp.Net core REST api

Environment setup:

> docker-compose up

Connect to the postgres DB and create new user

sql> create user db_user with encrypted password 'password' createdb;

> dotnet ef database update (If ef not installed: dotnet tool install --global dotnet-ef)

User secrets:

> dotnet user-secrets set "DB_UserID" "username"
> dotnet user-secrets set "DB_Password" "password"
> dotnet user-secrets set "JWT_Key" "long secure jwt key"
