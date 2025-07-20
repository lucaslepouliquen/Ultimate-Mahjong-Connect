FROM --platform=linux/arm64 mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development

ARG GOOGLE_CLIENT_ID=""
ARG GOOGLE_CLIENT_SECRET=""
ARG FACEBOOK_APP_ID=""
ARG FACEBOOK_APP_SECRET=""
ARG INSTAGRAM_CLIENT_ID=""
ARG INSTAGRAM_CLIENT_SECRET=""
ARG CONNECTION_STRING=""
ARG JWT_SECRET_KEY=""
ARG JWT_ISSUER=""
ARG JWT_AUDIENCE=""
ARG JWT_EXPIRED_MINUTES=""

ENV GOOGLE_CLIENT_ID=$GOOGLE_CLIENT_ID
ENV GOOGLE_CLIENT_SECRET=$GOOGLE_CLIENT_SECRET
ENV FACEBOOK_APP_ID=$FACEBOOK_APP_ID
ENV FACEBOOK_APP_SECRET=$FACEBOOK_APP_SECRET
ENV INSTAGRAM_CLIENT_ID=$INSTAGRAM_CLIENT_ID
ENV INSTAGRAM_CLIENT_SECRET=$INSTAGRAM_CLIENT_SECRET
ENV ConnectionStrings__DefaultConnection=$CONNECTION_STRING
ENV JwtSettings__SecretKey=$JWT_SECRET_KEY
ENV JwtSettings__Issuer=$JWT_ISSUER
ENV JwtSettings__Audience=$JWT_AUDIENCE
ENV JwtSettings__ExpiredInMinutes=$JWT_EXPIRED_MINUTES

EXPOSE 8080
EXPOSE 8443

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:3fcf6f1e809c0553f9feb222369f58749af314af6f063f389cbd2f913b4ad556 AS build

WORKDIR /src
COPY ["UltimateMahjongConnect.WebApi/UltimateMahjongConnect.Server.WebApi.Net.csproj", "UltimateMahjongConnect.WebApi/"]
RUN dotnet restore "UltimateMahjongConnect.WebApi/UltimateMahjongConnect.Server.WebApi.Net.csproj"
COPY . .

WORKDIR "/src/UltimateMahjongConnect.WebApi"
RUN dotnet build "UltimateMahjongConnect.Server.WebApi.Net.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UltimateMahjongConnect.Server.WebApi.Net.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create the database directory and set proper permissions
RUN mkdir -p /app/data && \
    chown -R 1000:1000 /app/data && \
    chmod -R 755 /app/data

ENTRYPOINT ["dotnet", "UltimateMahjongConnect.Server.WebApi.Net.dll"]