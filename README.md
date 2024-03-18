---

# RabbitMQ with .NET - Quick Start Guide

This guide outlines how to set up and use a basic message publishing and consuming system using RabbitMQ in a .NET WebAPI application. It includes using Docker Compose for running RabbitMQ.

## Prerequisites

- .NET SDK (compatible version for your project)
- Docker and Docker Compose installed on your machine
- Visual Studio or another code editor of your choice

## Setup

### 1. Running RabbitMQ with Docker Compose

use a `docker-compose.yml` file inside the root of the project with the following content to set up and run RabbitMQ:

```yaml
version: '3.8'  
services:
  rabbitmq:
    image: rabbitmq:3-management
    environment:
      RABBITMQ_DEFAULT_USER: admin
      RABBITMQ_DEFAULT_PASS: admin@123
      RABBITMQ_VIRTUAL_HOST: "/"
    ports:
      - 15672:15672
      - 5672:5672
```

To start RabbitMQ, run the following command in the directory where your `docker-compose.yml` file is located:

```shell
docker-compose up -d
```

This command will start RabbitMQ in a Docker container. You can access the RabbitMQ management interface at `http://localhost:15672` using the username `admin` and the password `admin@123`.

### 2. Add RabbitMQ Dependency

Recovery the RabbitMQ.Client library to your .NET project. This can be done using the NuGet Package Manager with the following command:

```shell
dotnet restore
```

### 3. Setting Up the Consumer

Implement the consumer service as shown earlier in `RabbitMQConsumerService`. This service will listen for messages sent to the queue and process them as necessary.

Ensure to register this service in the `ConfigureServices` method of your `Startup.cs` file:

```csharp
services.AddHostedService<RabbitMQConsumerService>();
```

### 4. Setting Up the Publisher

Create a `RabbitMQPublisher` class to encapsulate the logic for publishing messages. Use the provided example earlier to implement the publishing methods.

### 5. Exposing an API to Publish Messages

Implement a controller, like `PublishController`, to provide an HTTP interface through which messages can be published. This controller will use the `RabbitMQPublisher` class to send messages to the RabbitMQ queue.

### 6. Running the Application

With the RabbitMQ Server running in a Docker container, start your .NET WebAPI application. The application should connect to RabbitMQ and be ready to consume messages from the configured queue and publish messages via the exposed API.

## Testing the Application

To test publishing messages, you can use a tool like Postman or cURL. Here is an example of how to publish a message using cURL:

```shell
curl -X POST http://localhost:5000/publish -H "Content-Type: application/json" -d "{\"RoutingKey\":\"example.topic\",\"Message\":\"Hello RabbitMQ\"}"
```

Replace `localhost:5000` with the correct address and port of your application.

## Conclusion

This guide provided a starting point for working with RabbitMQ in a .NET application, covering both message publishing and consumption. The inclusion of Docker Compose for RabbitMQ setup simplifies the initial configuration and allows for easy deployment and scalability.

---