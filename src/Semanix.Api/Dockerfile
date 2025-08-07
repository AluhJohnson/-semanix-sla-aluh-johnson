FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SlipFree.Api/SlipFree.Api.csproj", "SlipFree.Api/"]
RUN dotnet restore "SlipFree.Api/SlipFree.Api.csproj"
COPY . .
WORKDIR "/src/SlipFree.Api"
RUN dotnet build "SlipFree.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SlipFree.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SlipFree.Api.dll"]
