FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Trackmeal.csproj", "./"]
RUN dotnet restore "./Trackmeal.csproj"
COPY . .
RUN dotnet build "Trackmeal.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Trackmeal.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Trackmeal.dll"]