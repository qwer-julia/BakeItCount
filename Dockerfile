FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo da solução e os projetos
COPY ["BakeItCountApi.sln", "./"]
COPY ["BakeItCountApi/BakeItCountApi.csproj", "BakeItCountApi/"]
COPY ["BakeItCountApi.Tests/BakeItCountApi.Tests.csproj", "BakeItCountApi.Tests/"]

# Restaura as dependências
RUN dotnet restore "BakeItCountApi/BakeItCountApi.csproj"

# Copia todo o código fonte
COPY . .

# Build do projeto
WORKDIR "/src/BakeItCountApi"
RUN dotnet build "BakeItCountApi.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "BakeItCountApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "BakeItCountApi.dll"]