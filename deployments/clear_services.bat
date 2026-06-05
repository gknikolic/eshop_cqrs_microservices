@echo off

REM Set the current context to use the 'eshop' namespace
kubectl config set-context --current --namespace=eshop

kubectl delete deployments --all 
kubectl delete services --all 
kubectl delete secrets --all 
kubectl delete configmap --all 

@echo All services cleared successfully.