FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine as builder
WORKDIR /app

COPY src/RemindMeal.csproj  ./
RUN dotnet restore

COPY src/ .
RUN dotnet publish -c Release -o publish --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine
COPY --from=builder /app/publish /app/
WORKDIR /app
ENTRYPOINT ["dotnet", "RemindMeal.dll"]
