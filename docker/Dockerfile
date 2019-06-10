FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine as builder
WORKDIR /app
COPY RemindMeal.csproj  ./
RUN dotnet restore

COPY . .
ARG Configuration=Release
RUN dotnet publish -c ${Configuration} -o publish --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
COPY --from=builder /app/publish /app/
WORKDIR /app
ENTRYPOINT dotnet RemindMeal.dll
