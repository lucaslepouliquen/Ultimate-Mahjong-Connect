FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 80
EXPOSE 443

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
ENTRYPOINT ["dotnet", "UltimateMahjongConnect.Server.WebApi.Net.dll"]