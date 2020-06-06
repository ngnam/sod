FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS builder
WORKDIR /source
COPY source/StoreOrder/src/StoreOrder.WebApplication ./StoreOrderWebApplication
#COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -r linux-musl-x64 -o /app-store-order ./StoreOrderWebApplication/StoreOrder.WebApplication.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app-store-order
#COPY --from=builder /app .
#COPY --from=node /app/build ./wwwroot
CMD ASPNETCORE_URLS=http://*:$PORT ./StoreOrderWebApplication
