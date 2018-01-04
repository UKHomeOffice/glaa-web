#!/usr/bin/env bash

set -o errexit
set -o nounset

export KUBE_NAMESPACE=glaa-dev
export KUBE_SERVER=${KUBE_TOKEN_ACP_NOTPROD}
export KUBE_TOKEN=${KUBE_TOKEN_ACP_NOTPROD} 

kd --insecure-skip-tls-verify --file kubedb/sa-secret.yaml --file kubedb/db-secret.yaml --file kubedb/deployment.yaml --file kubedb/ingress.yaml --file kubedb/service.yaml --file kube/db-conn-secret.yaml --file kube/deployment.yaml --file kube/ingress.yaml --file kube/service.yaml