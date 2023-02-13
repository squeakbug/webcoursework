#!/bin/sh

set -e -x

apt-get update

DEBIAN_FRONTEND="noninteractive" apt-get -y install tzdata

apt-get install -y \
	ssh \
	git \
	vim \
	gdb \
	wget \
	iputils-ping \
	nginx \
	binutils-dev \
	net-tools \
	nginx-extras 

mkdir -p /etc/ssl/private
chmod 700 /etc/ssl/private

touch ssl_data
echo "RU" > ssl_data
echo "Volodya" >> ssl_data
echo "Moscow" >> ssl_data
echo "BMSTU" >> ssl_data
echo "ICS" >> ssl_data
echo "localhost" >> ssl_data
echo "volodya@mail.ru" >> ssl_data

openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout /etc/ssl/private/nginx-selfsigned.key -out /etc/ssl/certs/nginx-selfsigned.crt < ssl_data
openssl dhparam -out /etc/ssl/certs/dhparam.pem 2048
