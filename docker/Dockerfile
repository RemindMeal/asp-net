FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine as builder
WORKDIR /app
COPY RemindMeal.csproj  ./
RUN dotnet restore

COPY . .
ARG Configuration=Release
RUN dotnet publish -c ${Configuration} -o publish --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine
COPY --from=builder /app/publish /app/
WORKDIR /app
ENTRYPOINT dotnet RemindMeal.dll
