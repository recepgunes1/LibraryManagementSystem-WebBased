FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/LMS.Web/LMS.Web.csproj", "src/LMS.Web/"]
COPY ["src/LMS.Entity/LMS.Entity.csproj", "src/LMS.Entity/"]
COPY ["src/LMS.Core/LMS.Core.csproj", "src/LMS.Core/"]
COPY ["src/LMS.Service/LMS.Service.csproj", "src/LMS.Service/"]
COPY ["src/LMS.Data/LMS.Data.csproj", "src/LMS.Data/"]
RUN dotnet restore "src/LMS.Web/LMS.Web.csproj"
COPY . .
WORKDIR "/src/src/LMS.Web"
RUN dotnet build "LMS.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LMS.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LMS.Web.dll"]
