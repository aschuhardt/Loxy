# Loxy
*L(unar) (Pr)oxy*

A HTTP proxy frontend for the gemini protocol.

## Overview
Loxy serves a website that proxies incoming requests to a preconfigured gemini capsule.  Gemtext is transformed into HTML.


Loxy supports injecting CSS stylesheets, JavaScript, and `<head>` contents.  By setting a content root directory, static files may be served.

### Links
Links to external capsules are transformed into Loxy links.  For example:

If Loxy is passed `-u geminiprotocol.net` then
  - Loxy will proxy incoming requests to `gemini://geminiprotocol.net` by default
  - A link that points to `gemini://geminispace.info` will be translated to `/lxy/external/geminispace.info`, and from that route Loxy will proxy requests to the external capsule.

If Loxy is served from behind a reverse proxy such as NGINX or HAProxy, then external proxying can be disabled by restricting access to the `/lxy/external` route prefix.  This can be helpful for preventing abuse of the proxy by bad actors.

## Usage
Execute the 'loxy' binary to start the service.
```
./loxy --remote=gemini.circumlunar.space
```
Executing the binary without specifying a remote URI argument will result in this overview page being shown in response to web requests.
Various command-line arguments can be used to configure the proxy, and are described below.

### Docker-compose
Modify the build arguments in `docker-compose.yml`, and run `docker compose up -d` to start a new container.

## Configuration
The following are the typical command-line arguments that you'll use to configure Loxy.
Note that becuase Loxy is built upon the ASP.NET Core framework, there are many more options for configuration than those shown here. See documentation [here].

### Command-line Arguments
- `-u|--remote` The remote Gemini URI to which requests will be proxied. This should correspond to the primary capsule that you want to serve. Omitting this argument will result in this usage page being shown in response to each request. Note that other capsules will be served from the /lxy/external path.
- `--stats` Prints the response status, round-trip time, and request URL at the bottom of each page.
- `-r|--root` A path to a filesystem directory from which to serve static content.
- `-s|--stylesheet` A path to a CSS file within Content Root that will be linked on each response page. Only used if Content Root is set.
- `-j|--script` A path to a Javascript file within Content Root that will be included on each response page. Only used if Content Root is set.
- `--head` A path to an HTML file to insert into each page's `head` element.  Only used if Content Root is set.
- `--no-loxy-info` Disables the usage information page (`/lxy/overview`).
- `-p|--port` The network port to listen on. Default is 8080.

