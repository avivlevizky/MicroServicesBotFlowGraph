FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ApiGateway/ApiGw-Base/ApiGw-Base.csproj ApiGateway/ApiGw-Base/
RUN dotnet restore ApiGateway/ApiGw-Base/ApiGw-Base.csproj
COPY . .
WORKDIR /src/ApiGateway/ApiGw-Base
RUN dotnet build ApiGw-Base.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ApiGw-Base.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ApiGw-Base.dll"]
