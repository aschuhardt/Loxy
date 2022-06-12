ARG PROXY_REMOTE_URI

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
ARG PROXY_REMOTE_URI
ENV PROXY_REMOTE_URI=${PROXY_REMOTE_URI}
CMD ["sh", "-c", "dotnet loxy.dll -p 80 -u $PROXY_REMOTE_URI"]