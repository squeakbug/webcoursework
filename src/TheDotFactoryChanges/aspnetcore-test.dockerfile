FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
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
RUN dotnet restore

RUN dotnet build
