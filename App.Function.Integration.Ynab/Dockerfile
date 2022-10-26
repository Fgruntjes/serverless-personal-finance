ARG DOTNET_VERSION=6.0
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src
COPY ["App.Function.Integration.Ynab/App.Function.Integration.Ynab.csproj", "App.Function.Integration.Ynab/"]
RUN dotnet restore "App.Function.Integration.Ynab/App.Function.Integration.Ynab.csproj"
COPY . .
WORKDIR "/src/App.Function.Integration.Ynab"
RUN dotnet build "App.Function.Integration.Ynab.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.Function.Integration.Ynab.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.Function.Integration.Ynab.dll"]
