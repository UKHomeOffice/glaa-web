#!/usr/bin/env bash

set -o errexit
set -o nounset

export KUBE_NAMESPACE=glaa-dev
export KUBE_SERVER=${KUBE_SERVER_ACP_NOTPROD}
export KUBE_TOKEN=${KUBE_TOKEN_ACP_NOTPROD}

#kubectl --server=$KUBE_SERVER --token=$KUBE_TOKEN --namespace=$KUBE_NAMESPACE delete deployments glaa-web
#kubectl --server=$KUBE_SERVER --token=$KUBE_TOKEN --namespace=$KUBE_NAMESPACE delete deployments glaa-db-setup

kd --debug --insecure-skip-tls-verify \
  --timeout 10m0s \
  -f kube-db-setup/deployment.yaml \
  -f kube/deployment.yaml \
  -f kube/ingress.yaml \
  -f kube/service.yaml \
  -f kube/network-policy.yaml \