#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Loxy/Loxy.csproj", "Loxy/"]
RUN dotnet restore "Loxy/Loxy.csproj"
COPY . .
WORKDIR "/src/Loxy"
RUN dotnet build "Loxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Loxy.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV PROXY_REMOTE_URI
ENTRYPOINT ["dotnet", "Loxy.dll", "--remote=${PROXY_REMOTE_URI}"]