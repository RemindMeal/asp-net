FROM mcr.microsoft.com/dotnet/sdk:6.0 as builder

WORKDIR /app
COPY RemindMeal.App/RemindMeal.csproj  RemindMeal.App/
COPY RemindMeal.Data/RemindMealData.csproj  RemindMeal.Data/
RUN dotnet restore RemindMeal.App

COPY RemindMeal.App/ RemindMeal.App/
COPY RemindMeal.Data/ RemindMeal.Data/
RUN dotnet publish -c Release -o publish --no-restore RemindMeal.App/

FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY --from=builder /app/publish /app/
WORKDIR /app
ENTRYPOINT ["dotnet", "RemindMeal.dll"]
