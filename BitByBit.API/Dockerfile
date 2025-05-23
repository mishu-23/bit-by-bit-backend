# Folosim imaginea oficială .NET SDK pentru build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiem fișierele proiectului
COPY ["BitByBit.API/BitByBit.API.csproj", "BitByBit.API/"]
RUN dotnet restore "BitByBit.API/BitByBit.API.csproj"

# Copiem restul codului
COPY . .
RUN dotnet build "BitByBit.API/BitByBit.API.csproj" -c Release -o /app/build

# Publicăm aplicația
FROM build AS publish
RUN dotnet publish "BitByBit.API/BitByBit.API.csproj" -c Release -o /app/publish

# Folosim imaginea .NET runtime pentru runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expunem portul pe care rulează aplicația
EXPOSE 5000
EXPOSE 5001

# Comanda care rulează aplicația
ENTRYPOINT ["dotnet", "BitByBit.API.dll"] 