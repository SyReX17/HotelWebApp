FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HotelWebApp/HotelWebApp.csproj", "HotelWebApp/"]
RUN dotnet restore "HotelWebApp/HotelWebApp.csproj"
COPY . .
WORKDIR "/src/HotelWebApp"
RUN dotnet build "HotelWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HotelWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "HotelWebApp.dll"]
