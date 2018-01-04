#!/usr/bin/env bash

set -o errexit
set -o nounset

export KUBE_NAMESPACE=glaa-dev
export KUBE_SERVER=${kube_server_acp_notprod}
export KUBE_TOKEN=${kube_token_acp_notprod}

kd --insecure-skip-tls-verify --file kubedb/sa-secret.yaml --file kubedb/db-secret.yaml --file kubedb/deployment.yaml --file kubedb/ingress.yaml --file kubedb/service.yaml --file kube/db-conn-secret.yaml --file kube/deployment.yaml --file kube/ingress.yaml --file kube/service.yaml