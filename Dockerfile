# Get dotnet sdk
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine-arm64v8 AS build-env
WORKDIR /api

# Build app
COPY . ./
RUN dotnet publish ./nhl-player-trigger/LocalRunning --self-contained -r linux-musl-arm64 -p:PublishSingleFile=true -c Release -o ./deploy

# Generate image
FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine-arm64v8
WORKDIR /api
RUN addgroup -S apigroup && adduser -S apiuser 
USER apiuser 
COPY --from=build-env --chown=apiuser:apigroup /api/deploy/LocalRunning .
ENTRYPOINT ["./LocalRunning"]