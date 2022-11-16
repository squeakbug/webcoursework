FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /src

COPY *.sln .
COPY WebController/*.csproj ./WebController/
COPY Presenter/*.csproj ./Presenter/
COPY DataAccessSQLServer/*.csproj ./DataAccessSQLServer/
COPY DataAccessInterface/*.csproj ./DataAccessInterface/
COPY ConverterService/*.csproj ./ConverterService/
COPY AuthService/*.csproj ./AuthService/
COPY AuthServiceTest/*.csproj ./AuthServiceTest/
COPY ConverterServiceTest/*.csproj ./ConverterServiceTest/
COPY SQLServerDALTest/*.csproj ./SQLServerDALTest/
COPY CommonITCase/*.csproj ./CommonITCase/
COPY E2ETest/*.csproj ./E2ETest/
COPY DBBenchmark/*.csproj ./DBBenchmark/
RUN dotnet restore

COPY . .
WORKDIR /src/WebController
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "WebControllers.dll"]

ENV ASPNETCORE_URLS http://*:59572

# see https://github.com/dotnet/dotnet-docker/tree/main/samples/dotnetapp
# and https://learn.microsoft.com/ru-ru/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-6.0
# alpine aspnet doesn't works unfotunately
# use .dockerignore to prevent obj files coping (https://github.com/dotnet/dotnet-docker/issues/1649)
# this can be helpful: https://stackoverflow.com/questions/47103570/asp-net-core-2-0-multiple-projects-solution-docker-file
