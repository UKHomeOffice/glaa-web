#!/bin/bash
kubectl create secret generic secret-appsettings --from-file=./appsettings.secrets.json