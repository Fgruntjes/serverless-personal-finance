ARG DOTNET_VERSION=7.0
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src
COPY ["App.Function.Banktransaction.Import/App.Function.Banktransaction.Import.csproj", "App.Function.Banktransaction.Import/"]
RUN dotnet restore "App.Function.Banktransaction.Import/App.Function.Banktransaction.Import.csproj"
COPY . .
WORKDIR "/src/App.Function.Banktransaction.Import"
RUN dotnet build "App.Function.Banktransaction.Import.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.Function.Banktransaction.Import.csproj" -c Release -o /app/publish

FROM base AS final

# Create rootless user
RUN addgroup --group app --gid 1000 \
  && adduser --uid 1000 --gid 1000 app \
  && chown app:app /app 
USER app:app 

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.Function.Banktransaction.Import.dll"]
