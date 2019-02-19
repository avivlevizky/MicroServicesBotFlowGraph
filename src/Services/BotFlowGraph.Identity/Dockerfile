FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["BotFlowGraph.Identity/BotFlowGraph.Identity.csproj", "BotFlowGraph.Identity/"]
RUN dotnet restore "BotFlowGraph.Identity/BotFlowGraph.Identity.csproj"
COPY . .
WORKDIR "/src/BotFlowGraph.Identity"
RUN dotnet build "BotFlowGraph.Identity.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BotFlowGraph.Identity.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BotFlowGraph.Identity.dll"]