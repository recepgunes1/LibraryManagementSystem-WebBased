FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Library_Management_System.Web/Library_Management_System.Web.csproj", "src/Library_Management_System.Web/"]
COPY ["src/Library_Management_System.Service/Library_Management_System.Service.csproj", "src/Library_Management_System.Service/"]
COPY ["src/Library_Management_System.Data/Library_Management_System.Data.csproj", "src/Library_Management_System.Data/"]
COPY ["src/Library_Management_System.Entity/Library_Management_System.Entity.csproj", "src/Library_Management_System.Entity/"]
COPY ["src/Library_Management_System.Core/Library_Management_System.Core.csproj", "src/Library_Management_System.Core/"]
RUN dotnet restore "src/Library_Management_System.Web/Library_Management_System.Web.csproj"
COPY . .
WORKDIR "/src/src/Library_Management_System.Web"
RUN dotnet build "Library_Management_System.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Library_Management_System.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Library_Management_System.Web.dll"]
