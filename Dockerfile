FROM mcr.microsoft.com/dotnet/sdk:6.0 as builder
WORKDIR /app

COPY RemindMeal/RemindMeal.csproj  ./
RUN dotnet restore

COPY RemindMeal/ .
RUN dotnet publish -c Release -o publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY --from=builder /app/publish /app/
WORKDIR /app
ENTRYPOINT ["dotnet", "RemindMeal.dll"]
