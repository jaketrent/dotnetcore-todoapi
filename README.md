## Setup Local Env Vars

```
dotnet restore
cp secrets.json.example secrets.json
vim secrets.json
cat secrets.json dotnet user-secrets set 
dotnet user-secrets list
```