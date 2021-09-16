## DotNet-5 Api

<br>

#### dotnet development certificate

```bash
dotnet dev-certs https --trust
```

<br>

#### VSCode shortcut for build

- first add group section in .vscode/tasks.json#build this you can use below mentioned shortcut for building.

```
Ctrl + Shift + B
```

<br>

#### MongoDB docker run

```bash
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=<passowrd> mongo
```

<br>

#### DotNet Secret Management

- Initialize dotnet 5 secret management.
  ```bash
  dotnet user-secrets init
  ```

- Add mongo pasword property as secret.
    - MongoDbSettings:Password (follows appsettings.json convention)
      ```bash
      sudo dotnet user-secrets set MongoDbSettings:Password <password>
      ```