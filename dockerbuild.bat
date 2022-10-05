docker build --force-rm -t identityimage:0.0.1 -f "C:\PERSONAL\Proiecte\RADBURG\src\Services\Identity\Identity.API\Dockerfile" "C:\PERSONAL\Proiecte\RADBURG\src\Services\Identity"
docker stop identitycontainer
docker run --rm -d -it -p 80 --name identitycontainer identityimage:0.0.1