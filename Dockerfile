FROM mcr.microsoft.com/dotnet/sdk:5.0 as builder
WORKDIR /app

COPY src/RemindMeal.csproj  ./
RUN dotnet restore

COPY src/ .
RUN dotnet publish -c Release -o publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY --from=builder /app/publish /app/
WORKDIR /app
ENTRYPOINT ["dotnet", "RemindMeal.dll"]
