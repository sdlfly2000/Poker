# Poker

## Introduction

Poker will help your team on voting work efforts (for scrum team), with mulit players participating. Poker is open source without any limitation on using or modification. It would be better if you contribute your improvements to here.

## Deployment

### Ubuntu 18.04

1. Environment Preparation
    - Nginx version: nginx/1.14.0 (Ubuntu)
    - Supervisor
    - Dotnet core 3.1.3
2. Configure
    - Nginx

    ```bash
    /etc/nginx/sites-available/default

    map $http_upgrade $connection_upgrade {
    default upgrade;
    ''      close;
    }

    upstream websocket {
        server localhost:5000;
    }

    server {
        listen 80;
        location / {
            proxy_pass http://localhost:5000;
            proxy_http_version 1.1;
            proxy_set_header Upgrade $http_upgrade;
            proxy_set_header Connection $connection_upgrade;
            proxy_set_header Host $host;
            proxy_cache_bypass $http_upgrade;
        }
    }
    ```

    - Supervisor

    ```bash
    /etc/supervisor/conf.d/Poker.conf

    [program:Poker]
    directory=[Directory]/Poker
    command=dotnet [Directory]/Poker.dll
    autostart=true
    autorestart=true
    startsecs=10
    startretries=50
    stderr_logfile=/var/log/Poker.err.log
    stdout_logfile=/var/log/Poker.out.log
    environment=ASPNETCORE__ENVIRONMENT=Production
    user=root
    stopsignal=INT
    ```

## Live Demo

[Demo](http://182.61.37.221:8100/)
