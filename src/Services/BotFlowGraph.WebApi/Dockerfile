FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Services/BotFlowGraph.WebApi/BotFlowGraph.WebApi.csproj", "src/Services/BotFlowGraph.WebApi/"]
COPY ["src/Class Libraries/Contracts/Contracts.csproj", "src/Class Libraries/Contracts/"]
COPY ["src/Class Libraries/MongoDataAccess/MongoDataAccess.csproj", "src/Class Libraries/MongoDataAccess/"]
RUN dotnet restore "src/Services/BotFlowGraph.WebApi/BotFlowGraph.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Services/BotFlowGraph.WebApi"
RUN dotnet build "BotFlowGraph.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BotFlowGraph.WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BotFlowGraph.WebApi.dll"]