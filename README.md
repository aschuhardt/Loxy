# Loxy
*L(unar) (Pr)oxy*

A HTTP proxy frontent for the Gemini protocol

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
Note that becuase Loxy is built upon the ASP.NET Core framework, there are many more options for configuration than those shown here. See documentation here and here.

### Command-line Arguments
- `-u|--remote` The remote Gemini URI to which requests will be proxied. This should correspond to the primary capsule that you want to serve. Omitting this argument will result in this usage page being shown in response to each request. Note that other capsules will be served from the /lxy/external path.
- `-r|--root` A path to a filesystem directory from which to serve static content.
- `-s|--stylesheet` A path to a CSS file within Content Root that will be linked on each response page. Only used if Content Root is set.
- `-j|--script` A path to a Javascript file within Content Root that will be included on each response page. Only used if Content Root is set.
- `-p|--port` The network port to listen on. Default is 8080.

