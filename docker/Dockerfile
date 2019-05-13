FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine as builder
ARG Configuration=Release
COPY . /app
WORKDIR /app
RUN dotnet publish -c ${Configuration} -o publish 

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
COPY --from=builder /app/publish /app/
WORKDIR /app
CMD dotnet RemindMeal.dll
