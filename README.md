## DotNet-5 Api

#### DotNet: Secret Management

- Initialize dotnet 5 secret management.

```bash  
dotnet user-secrets init
```

- Add mongo pasword property as secret.
    - MongoDbSettings:Password (follows appsettings.json convention)
      ```bash
      sudo dotnet user-secrets set MongoDbSettings:Password <password>
      ```

<br>

#### DotNet: development certificate

```bash
dotnet dev-certs https --trust
```

<br>

#### VSCode: shortcut for build

- first add group section in .vscode/tasks.json:build this you can use below mentioned shortcut for building.

```
Ctrl + Shift + B
```

<br>

#### Docker: MongoDB docker run

- Without network

```bash
docker run -d --rm --name mongo \
-p 27017:27017 -v mongodbdata:/data/db \
-e MONGO_INITDB_ROOT_USERNAME=<username> \
-e MONGO_INITDB_ROOT_PASSWORD=<passowrd> mongo
```

- With network

```bash
docker run -d --rm --name mongo \
-p 27017:27017 -v mongodbdata:/data/db \
-e MONGO_INITDB_ROOT_USERNAME=<username> \
-e MONGO_INITDB_ROOT_PASSWORD=<passowrd> \
--network=<network name> mongo
```

- For change password / reset container don't forget to remove volume, using below cmd before creating container.

```bash
dotnet volume rm mongodbdata
```

<br>

#### Docker: build & tag image

```bash
docker build -t maxxnair/catalog:v1 .         
```

<br>

#### Docker: create a local network

```bash
docker network create catalogdemo         
```

<br>

#### Docker: view existing networks

```bash
docker network ls
```

<br>

#### Docker: run Catalog.Api run image

```bash
docker run -it --rm -p 8080:80 \
-e MongoDbSettings:Host=mongo \
-e MongoDbSettings:Password=<password> \
--network=catalogdemo maxxnair/catalog:v1
```

<br>

#### Docker: push image to docker hub

```bash
docker push maxxnair/catalog:v1
```
