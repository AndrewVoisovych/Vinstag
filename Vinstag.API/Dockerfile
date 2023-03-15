FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Vinstag.API/Vinstag.API.csproj", "Vinstag.API/"]
RUN dotnet restore "Vinstag.API/Vinstag.API.csproj"
COPY . .
WORKDIR "/src/Vinstag.API"
RUN dotnet build "Vinstag.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vinstag.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vinstag.API.dll"]
