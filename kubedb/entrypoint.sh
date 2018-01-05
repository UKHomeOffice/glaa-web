#!/bin/bash
cat /etc/passwd > /tmp/passwd
echo "$(id -u):x:$(id -u):$(id -g):dynamic uid:/tmp:/bin/false" >>
/tmp/passwd
cat /etc/group > /tmp/group
echo "$(id -u):x:$(id -u):" >> /tmp/group
export NSS_WRAPPER_PASSWD=/tmp/passwd
export NSS_WRAPPER_GROUP=/tmp/group
export LD_PRELOAD=libnss_wrapper.so
