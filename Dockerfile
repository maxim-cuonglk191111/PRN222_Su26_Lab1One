FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["BusinessObjects/BusinessObjects.csproj", "BusinessObjects/"]
COPY ["DataAccessObjects/DataAccessObjects.csproj", "DataAccessObjects/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["ProductManagementMVC/ProductManagementMVC.csproj", "ProductManagementMVC/"]

RUN dotnet restore "ProductManagementMVC/ProductManagementMVC.csproj"

COPY . .
RUN dotnet publish "ProductManagementMVC/ProductManagementMVC.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ProductManagementMVC.dll"]
