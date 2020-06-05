FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS builder
WORKDIR /source
COPY source/StoreOrder/src/StoreOrder.WebApplication ./StoreOrderWebApplication
#COPY . .
# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*
RUN dotnet restore
RUN dotnet publish -c Release -r linux-musl-x64 -o /app-store-order ./StoreOrderWebApplication/StoreOrder.WebApplication.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app-store-order
#COPY --from=builder /app .
#COPY --from=node /app/build ./wwwroot
CMD ASPNETCORE_URLS=http://*:$PORT ./StoreOrderWebApplication
