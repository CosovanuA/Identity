docker build -t identityimg:v1 .
docker tag identityimg:v1 radburgcontainerregistry.azurecr.io/identityimg:v1
docker push radburgcontainerregistry.azurecr.io/identityimg:v1
