Notes
============

This sample project is created to build Board Game API for Battleship game. 

There are 3 projects in solution.

BoardGame.API - Contains the main logic for API Controller. Swagger is added to provide API documentation and Redis is used as a cache to preserve state between API calls.

BoardGame.Domain - Contains the domain objects used in application. There  are three classes representing  a Board which consists of n number of Cells and a Battleship class which represents a Battleship to be used on game board.

BoardGame.Tests - Contains unit tests for the project.

Important
============
 Make sure you have Redis configuration setup to connect to your redis instance in appsettings.json in API project before you run application on your local machine.
 
 Calling a CreateBoard method will give you a GUID as BoardId to use for subsequent calls to other API endpoints.
 
 
