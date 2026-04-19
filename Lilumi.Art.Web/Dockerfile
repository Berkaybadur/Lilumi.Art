FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Lilumi.Art.Web/Lilumi.Art.Web.csproj", "Lilumi.Art.Web/"]
COPY ["Lilumi.Art.Application/Lilumi.Art.Application.csproj", "Lilumi.Art.Application/"]
COPY ["Lilumi.Art.Infrastructure/Lilumi.Art.Infrastructure.csproj", "Lilumi.Art.Infrastructure/"]
COPY ["Lilumi.Art.Domain/Lilumi.Art.Domain.csproj", "Lilumi.Art.Domain/"]
RUN dotnet restore "Lilumi.Art.Web/Lilumi.Art.Web.csproj"
COPY . .
WORKDIR "/src/Lilumi.Art.Web"
RUN dotnet publish "Lilumi.Art.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Lilumi.Art.Web.dll"]
