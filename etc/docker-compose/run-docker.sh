#!/bin/bash

if [[ ! -d certs ]]
then
    mkdir certs
    cd certs/
    if [[ ! -f localhost.pfx ]]
    then
        dotnet dev-certs https -v -ep localhost.pfx -p 8c139e60-6f31-4409-9569-ea186c4ad673 -t
    fi
    cd ../
fi

docker-compose up -d
