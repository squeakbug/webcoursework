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
WORKDIR /src/DBBenchmark
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS runtime
WORKDIR /app
RUN mkdir -p ./run_result
COPY run-result/*.* ./run_result/
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "DBBenchmark.dll", "ss", "/app/run_result/query_runs_ss.csv"]
