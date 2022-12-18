# visionbox-cardgame

Tech stack used : ASP.NET 6.0, C# 9.0 and SqlServer
## running in developing software like visual studio, rider, vs code

if you want to run with Visual studio or rider you should set your `appsettings.Development.json` below parts:
this code for set connection string to connect to your sql server database
```
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost,1433;Initial Catalog=VisionBox;User Id=sa;Password=@Daneshgah65411887;Integrated Security=false;"
  },
```
## Docker
you can build and running application in docker with this command in terminal or dos:
```
docker-compose up -d
```
## API description
all api in this project is version 1 (v1)

|          api           |             description             | type |
|:----------------------:|:-----------------------------------:|:----:|
|      /api/v1/Game      |           create the game           | POST |
|      /api/v1/Game      | get all game with fullname of owner | GET  |
|   /api/v1/Game/Join    |          join to the game           | PUT  |
| /api/v1/Game/guessCard |           send your guess           | POST |
|   /api/v1/Game/Left    |          you left the game          | POST |
|   /api/v1/User/start   |           start the game            | POST |

## Notice:
* I did not use SignalR technology although we should use it because another gamers view should be updated online.
* I did not use integration or BDD because of the time. if it existed the test coverage would be better.


