FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
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
RUN dotnet build TheDotFactoryChanges.sln
#RUN dotnet test TheDotFactoryChanges.sln

# run tests on docker run
ENTRYPOINT ["dotnet", "test"]
