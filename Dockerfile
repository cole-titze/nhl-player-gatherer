# Get dotnet sdk
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine-arm64v8 AS build-env
WORKDIR /api

# Build app
COPY . ./
RUN dotnet publish ./nhl-player-trigger/LocalRunning --self-contained -r linux-musl-arm64 -p:PublishSingleFile=true -c Release -o ./deploy

# Generate image
FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine-arm64v8
WORKDIR /api
ENV DOTNET_RUNNING_IN_CONTAINER=true
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN addgroup -S playergroup && adduser -S playeruser 
USER playeruser 
COPY --from=build-env --chown=playeruser:playergroup /api/deploy/LocalRunning .
ENTRYPOINT ["./LocalRunning"]