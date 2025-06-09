FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY . . 
RUN dotnet restore "./BookStore/BookStore.csproj" --disable-parallel
RUN dotnet publish "./BookStore/BookStore.csproj" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "BookStore.dll"]