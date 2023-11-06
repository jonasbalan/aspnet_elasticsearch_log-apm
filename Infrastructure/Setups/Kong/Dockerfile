FROM golang:1.21.3 as build

WORKDIR /app

COPY go.mod ./
COPY go.sum ./
RUN go mod download

COPY *.go ./

RUN CGO_ENABLED=0 go build -o /elastic-apm


FROM kong:2.8.1-alpine

LABEL description="Alpine + Kong  + kong-oidc plugin + elastic-apm plugin"

USER root
RUN apk update && apk add curl git gcc musl-dev
RUN luarocks install luaossl OPENSSL_DIR=/usr/local/kong CRYPTO_DIR=/usr/local/kong
RUN luarocks install --pin lua-resty-jwt
RUN luarocks install kong-oidc

COPY --from=build /elastic-apm /usr/local/bin/

USER kong
