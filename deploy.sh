#!/usr/bin/env bash

set -o errexit
set -o nounset

export KUBE_NAMESPACE=glaa-dev
export KUBE_SERVER=${KUBE_TOKEN_ACP_NOTPROD}
export KUBE_TOKEN=${KUBE_TOKEN_ACP_NOTPROD} 

kd --insecure-skip-tls-verify \
  --timeout 10m0s \
  -f kubedb/deployment.yaml \
  -f kubedb/ingress.yaml \
  -f kubedb/service.yaml \
  -f kube/deployment.yaml \
  -f kube/ingress.yaml \
  -f kube/service.yaml