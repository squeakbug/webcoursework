FROM ubuntu:20.04

COPY run_server.sh /
COPY install_deps.sh /
RUN sh install_deps.sh

COPY nginx.conf /etc/nginx/nginx.conf
COPY static/ /usr/share/nginx/html/static/

ENV TZ=Europe/Moscow
