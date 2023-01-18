ARG DOTNET_VERSION=7.0
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
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
