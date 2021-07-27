# Pokemon API

I have written an ASP.Net Web API to simulate the game of Match!.

## Requirements

```
.NET 5 SDK
```

This can be downloaded [here](https://dotnet.microsoft.com/download/dotnet/5.0) (The SDK as opposed to just the runtime will install the cli used in steps below).

## Running the API

Once you have cloned the project, navigate to the root folder in a terminal or powershell window and follow the below steps to build, run the tests and run the application. 

```
dotnet build
dotnet test
dotnet run --project PokemonApi
```

Swagger should be running at [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html). 

The 2 available endpoints are:

[http://localhost:5000/pokemon/mewtwo](http://localhost:5000/pokemon/mewtwo)

[http://localhost:5000/pokemon/translated/mewtwo](http://localhost:5000/pokemon/translated/mewtwo)

The application can also be run using docker by following the below instructions. (Please [install docker](https://docs.docker.com/get-docker/) if it is not already). Under the root folder run:

```
docker build -t pokemon/api:1.0 .
docker run -d -p 5000:80 pokemon/api:1.0
```

## Future improvements

 - Changing the return type to include metadata to provide information about the result such as the status, any errors (if present) and the content of the response. Curently if a search is not found it returns an empty 204 response which doesn't give a developer much feedback.
 - HTTPS support.
 - The API is open so some sort of Authorisation would be needed for production.
 - The logging implementation is quite basic and could be extended to povide more insights on usage/performance.
 - There is very little exception management. It would be good to push exceptions to Sentry/Exceptionless/similar to be able to manage and monitor these in a single place.
 - There could be rate limiting as per the Translation API to help govern usage.
 - Similarly the api could benefit from caching either server side or through a CDN.
 - For the production environment there would be multiple instaces of the container that will be load balanced to add resilience and enable a rolling deployment strategy.
 - To support deployments and monitoring, a healthcheck endpoint should be added to provide feedback about whether the api is up and running and if it's dependancies are available.
