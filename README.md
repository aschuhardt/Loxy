# Loxy
*L(unar) (Pr)oxy*

A HTTP proxy frontent for the gemini protocol.

- Loxy serves a website that proxies incoming requests to a preconfigured gemini capsule
- Gemtext is transformed into HTML 
- Links to external capsules are translated into links that will proxy requests to those capsules
  - For example:
    - If Loxy is passed `-u geminiprotocol.net` then it will proxy requests to `gemini://geminiprotocol.net` by default
    - A link that points to `gemini://geminispace.info` will be translated to `/lxy/external/geminispace.info` and from that route Loxy will continue to proxy requests to the external capsule

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
- `-r|--root` A path to a filesystem directory from which to serve static content.
- `-s|--stylesheet` A path to a CSS file within Content Root that will be linked on each response page. Only used if Content Root is set.
- `-j|--script` A path to a Javascript file within Content Root that will be included on each response page. Only used if Content Root is set.
- `--head` A path to an HTML file to insert into each page's `head` element.  Only used if Content Root is set.
- `--no-loxy-info` Disables the usage information page (`/lxy/overview`).
- `--stats` Include extra information about the proxied request at the bottom of each page.  This is useful for troubleshooting
- `-p|--port` The network port to listen on. Default is 8080.

