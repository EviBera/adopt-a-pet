#!/bin/sh

set -e

until curl -s http://backend:80/api/advertisement/current; do
  >&2 echo "Backend is unavailable - sleeping"
  sleep 5
done

>&2 echo "Backend is up - executing command"
exec "$@"
