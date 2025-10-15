# 💬 Real-Time Chat Application

## Overview 💡
Hello-Chat is a real time app proivde single and group chatting with control on group chat admins, media uploading and retrevie.

 ## Features ⚡
 - Real time chatting using **SignalR**
 - Manage message bus between servers using **SignalR backplane** and **Redis**
 - Handle username check using **bloom filter** which provides
   * Decrease time for checkness.
   * Solve bottleneck problem during check username from mssql database.
 - Manage media storage and reterive by integrate with ImageKit (third part)
 - Easy to maintenance , testing and expanding as using **clean architecture** for building the project

## Software requirements 🖥
  * Dotnet 9
  * Docker for using redis
  * Sql server

## How to install and run the project?
```
git clone https://github.com/Omar-Alaa-Elzanaty/Hello-Chat.git
cd Hello-Chat/
docker run -d --name redis-stack -p 6379:6379 -p 8001:8001 redis/redis-stack:latest
```
