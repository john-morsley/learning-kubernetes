# Kubernetes

In order to use these manifest files, you'll first need a Kubernetes cluster.
By far the easist method is to install Docker Desktop and use its built in Kubernetes feature.

## Make

1. namespace
2. deploy-ui
3. service-ui
4. deploy-api
5. service-api

To get the services to work, you'll need the following entries in your hosts file:

example-api.morsley.io
example-ui.morsley.io

## Istio

Now we have a deployment and a service we will now use Istio to privide us with an Istio Ingress Gateway.

6. make gate-ui

```
kubectl get gateway --namespace example
```

Next we will use a virtual service to connect to our service.

