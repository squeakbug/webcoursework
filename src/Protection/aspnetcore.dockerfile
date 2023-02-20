FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
EXPOSE 7269

COPY *.sln .
COPY WebControllers/*.csproj ./WebControllers/
COPY Domain/*.csproj ./Domain/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY Application/*.csproj ./Application/
RUN dotnet restore

COPY . .
WORKDIR /src/WebControllers
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "WebControllers.dll"]
ENV ASPNETCORE_URLS http://*:7269
