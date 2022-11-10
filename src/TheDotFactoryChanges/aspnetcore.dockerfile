FROM mcr.microsoft.com/dotnet/sdk:6.0-windowsservercore-ltsc2022 AS build
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
RUN dotnet restore -r win-x64 /p:PublishReadyToRun=true

COPY . .
WORKDIR /src/WebController
RUN dotnet publish -c Release -o /app -r win-x64 --self-contained false --no-restore

#--use-current-runtime --self-contained false

FROM mcr.microsoft.com/dotnet/aspnet:6.0-windowsservercore-ltsc2022 AS runtime
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "WebControllers.dll"]

ENV ASPNETCORE_URLS http://*:59572

# see https://github.com/dotnet/dotnet-docker/tree/main/samples/dotnetapp
# and https://learn.microsoft.com/ru-ru/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-6.0
# alpine aspnet doesn't works unfotunately
# use .dockerignore to prevent obj files coping (https://github.com/dotnet/dotnet-docker/issues/1649)
# this can be helpful: https://stackoverflow.com/questions/47103570/asp-net-core-2-0-multiple-projects-solution-docker-file
