ARG PROXY_REMOTE_URI
ARG PROXY_CONTENT_ROOT
ARG PROXY_STYLESHEET
ARG PROXY_JAVASCRIPT

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
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
ARG PROXY_CONTENT_ROOT
ARG PROXY_STYLESHEET
ARG PROXY_JAVASCRIPT
ENV PROXY_REMOTE_URI=${PROXY_REMOTE_URI}
ENV PROXY_CONTENT_ROOT=${PROXY_CONTENT_ROOT}
ENV PROXY_STYLESHEET=${PROXY_STYLESHEET}
ENV PROXY_JAVASCRIPT=${PROXY_JAVASCRIPT}
CMD ["sh", "-c", "dotnet loxy.dll -p 80 -u $PROXY_REMOTE_URI -r $PROXY_CONTENT_ROOT -s $PROXY_STYLESHEET -j $PROXY_JAVASCRIPT"]
