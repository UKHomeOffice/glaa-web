#!/usr/bin/env bash

set -o errexit
set -o nounset

export KUBE_NAMESPACE=glaa-dev
export KUBE_SERVER=${KUBE_SERVER_ACP_NOTPROD}
export KUBE_TOKEN=${KUBE_TOKEN_ACP_NOTPROD}

kubectl --insecure-skip-tls-verify --ignore-not-found --server=${KUBE_SERVER} --token=${KUBE_TOKEN} --namespace=${KUBE_NAMESPACE} delete deployments glaa-web

kd --insecure-skip-tls-verify \
  --timeout 10m0s \
  -f kube/deployment.yaml \
  -f kube/ingress.yaml \
  -f kube/service.yaml \
  -f kube/network-policy.yaml \