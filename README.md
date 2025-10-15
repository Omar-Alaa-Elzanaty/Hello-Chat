# 💬 Real-Time Chat Application

## Overview 💡
Hello-Chat is a real time app proivde single and group chatting with control on group chat admins, media uploading and retrevie.

 ## Features ⚡
 - Real time chatting using **SignalR**
 - Manage message bus between servers using **SignalR backplane** and **Redis**
 - Handle username check using **bloom filter** which provides.
   * Decrease time for check.
   * Solve bottleneck problem during check username from mssql database by check redis first.
 - Manage media storage and reterive by integrate with ImageKit (third part).
 - Easy to maintenance, testing and expanding as using **clean architecture** for building the project infrastructure.

## Software requirements 🖥
  * Dotnet 9
  * Sql server
  * Docker for using redis

## How to install and run the project?
```
docker run -d --name redis-stack -p 6379:6379 -p 8001:8001 redis/redis-stack:latest
git clone https://github.com/Omar-Alaa-Elzanaty/Hello-Chat.git
cd Hello-Chat/Hello.API
dotnet run Hello.API.csproj
```
