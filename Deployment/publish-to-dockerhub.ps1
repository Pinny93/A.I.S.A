Set-Location -Path ../
docker build --file Deployment/Dockerfile --tag schnitzel9999/aisa .
docker push schnitzel9999/aisa

Set-Location -Path ./Deployment