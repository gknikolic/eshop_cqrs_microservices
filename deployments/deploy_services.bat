@echo off

REM Check if the namespace exists
kubectl get namespace eshop >nul 2>&1

REM If the namespace does not exist, create it
IF ERRORLEVEL 1 (
    echo Namespace 'eshop' does not exist. Creating...
    kubectl create namespace eshop
)

REM Set the current context to use the 'eshop' namespace
kubectl config set-context --current --namespace=eshop

kubectl apply -f catalog-read-db.yaml 
kubectl apply -f catalog-write-db.yaml
kubectl apply -f basketdb.yaml
kubectl apply -f inventorydb.yaml
kubectl apply -f customerdb.yaml
kubectl apply -f distributedcache.yaml
kubectl apply -f orderdb.yaml
kubectl apply -f messagebroker.yaml
kubectl apply -f catalog-read-api.yaml
kubectl apply -f catalog-write-api.yaml
kubectl apply -f basket-api.yaml
kubectl apply -f inventory-api.yaml
kubectl apply -f customer-api.yaml
kubectl apply -f discount-grpc.yaml
kubectl apply -f ordering-api.yaml
kubectl apply -f yarpapigateway.yaml
kubectl apply -f shopping-web.yaml

@echo All services deployed successfully.