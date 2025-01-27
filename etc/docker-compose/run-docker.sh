#!/bin/bash

if [[ ! -d certs ]]
then
    mkdir certs
    cd certs/
    if [[ ! -f localhost.pfx ]]
    then
        dotnet dev-certs https -v -ep localhost.pfx -p 7c8ddec1-48ba-4758-bf89-7f01a569d2d8 -t
    fi
    cd ../
fi

docker-compose up -d
