FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Mining/SenseMining.Mining.API/SenseMining.Mining.API.csproj", "SenseMining.Mining.API/"]
RUN dotnet restore "Mining/SenseMining.Mining.API/SenseMining.Mining.API.csproj"
COPY . .
WORKDIR "/src/SenseMining.Mining.API"
RUN dotnet build "SenseMining.Mining.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SenseMining.Mining.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SenseMining.Mining.API.dll"]
