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
	nginx-extras \
