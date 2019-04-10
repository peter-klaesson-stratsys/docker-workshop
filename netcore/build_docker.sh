#!/usr/bin/env bash

REPOSITORY=netcore-counter
TIME_STAMP=$"`date "+%Y%m%d-%H%M"`"

docker build -t ${REPOSITORY}:${TIME_STAMP} .
