FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["App.Job.Import.Ynab/App.Job.Import.Ynab.csproj", "App.Job.Import.Ynab/"]
RUN dotnet restore "App.Job.Import.Ynab/App.Job.Import.Ynab.csproj"
COPY . .
WORKDIR "/src/App.Job.Import.Ynab"
RUN dotnet build "App.Job.Import.Ynab.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.Job.Import.Ynab.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.Job.Import.Ynab.dll"]
