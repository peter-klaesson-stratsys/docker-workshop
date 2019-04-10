# Docker workshop

Syftet med workshopen är att få en liten introduktion till att använda docker som ett redskap vid build/deploy. För att se att det går att använda samma script och miljö på den egna maskinen som på en byggserver i "molnet".

## Installera docker

1. Ladda ner och installera docker for windows från `Z:\Simplicity platform\Utvecklare\Peter\docker` (så slipper du registrera dig på docker för att komma åt filen)
1. Se till att det är Linux-containers som körs (höger-klicka på valen nere till höger, står det "switch to Linux containers" i context-menyn, klicka på den. Annars är det OK.)
1. Starta en console (cmd.exe) och veriera att det går att köra docker-cli och att docker-cli får kontakt med dockerdaemonen genom att skriva `docker info`  

## Kommandon

* `docker` visar alla tillgänliga kommandon
* `docker pull` laddar ner en image från docker registry
* `docker push` laddar upp en image till docker registry

* `docker run [option] image [name]`: kör en image som container
* `docker rm container` tar bort en container
* `docker ps -a` visar alla containers (både som körs och som har stoppats)

Tillgängliga images från dockerhub finns sökbara på [dockerhub](https://hub.docker.com)

## Förberedelse för uppvärmningar

ladda ner "hello world"

* `docker pull hello-world`

`Docker run` försökar ladda ner en image automatiskt om den inte finns nedladdad

## Uppvärming 1

Testa att köra en container som bara skriver ut hello world och sedan terminerar. Efter att programmet terminerat kommer container att finnas kvar, vilket innebär att man kan köra fler kommandon i samma container genom `docker exec`. (Men det är inget vi behöver göra i detta fallet).  

Kör "hello world" och ta sedan bort containern

* `docker run -it hello-world`
* `docker ps -a` nu visas en stoppad container av hello world och dess hash
* `docker rm <hash>` tar bort containern

Förklaringar:  

* -it kör interactive terminal, dvs. stdin och stdout binds till din egen terminal. -t gör att även ex. ctrl+c fångas av containern.

* ps -a listar ALLA containers, även de som inte körs

Du behöver inte skriva hela hashen utan det kan räcker med de två första tecknen, ifall de är unika.

## Uppvärming 2

Kör containers med automatiskt borttagning

* `docker run --rm -it hello-word`

`--rm` tar bort containern när den kört klart  

Skall resultera i att `Hello from docker...` skrivs ut på skärmen och containern stoppas och tas bort autmatiskt.  
Det är lätt hänt att man får en massa stoppade containers på datorn som skräpar, så det kan vara bra att känna till `--rm`-flaggan.

## Uppvärming 3

Kör "en web-app" i bakgrunden som lyssnar på port 8080 på din localhost

* `docker run -d -p 9090:8000 --name webapp jwilder/whoami`

```
-d kör "detached", i bakgrunden
-p binder port 9090 på din dator till port 8000 på containern
-n ger containern ett namn
```

testa med en browser på http://localhost:9090 programmet kommer att skriva ut sitt hostname

1. Se att container körs genom att skriva `docker ps`.
1. Prova att stoppa containern genom att skriva `docker stop webapp`
Nu visas den inte längre med `docker ps`  
1. Det går att starta containern igen med `docker start webapp`
1. Se att det återigen går att öppna länken http://localhost:9090
1. För att ta bort container utan att stoppa den först går det att tvinga borttagning med -f `docker rm -f webapp`. Ta bort containern

## Bygg .net-applikation m.h.a. Dockerfile och kör en container av denna image

Gå in i katalogen `netcore`.
Här finns flera filer som berör byggprocessen. Kika på dessa och se om du kan lista ut vad de gör.

1. Dockerfile
1. build_docker.sh
1. azure-pipelines.yml

### Dockerfile

Definierar bygg-stegen genom att använda docker images. Docker filen är en s.k. "multistage" docker file. Dvs. det används olika images för att bygga och för att köra.  

* `FROM microsoft/dotnet:2.2-sdk AS build` öppnar en container från image med .net core SDK för att bygga.

* `COPY . /app` kopierar källkoden in i containern under /app

* `RUN dotnet publish -c Release -r linux-x64 -o /release Web` Bygger Web-projektet och publicerar applikationen i katalogen /Web

Sedan byggs image för release med en runtime-image som bas.

* `FROM microsoft/dotnet:2.2-runtime` öppnar en ny container med .net core runtime

* `COPY --from=build /release /app` kopierar allt i från bygg-containern in i den nya runtime-containern

* `WORKDIR /app` sätter aktuell katalog till `/app` (som att skriva cd /app)

* `ENTRYPOINT ["dotnet", "Web.dll"]` talar om vad som skall köras när den färdiga imagen körs som en container (den images som skapas från containen som bygger på dotnet runtime)

### build_docker.sh

Bygger en docker image med namnet `netcore-counter` och aktuell datum och tid som tag.  
I windows-miljö, kör gitbash och skriv `. build_docker.sh` för att exekvera skriptet

### azure-pipelines.yml

Ett exempel på en azure-definition som använder Ubutuntu 16.04 och bygger en docker image genom att använda scriptet `build_dockerfile.sh`

### Övning

Testa köra bygga och köra applikationen i ./netcore och testa att det går att komma åt den via webläsare.  

Följ instruktionen i ./netcore/readme.md

## Azure devops (valfritt, ej steg för steg)

Tiden räckte inte till för att förbereda denna del med en steg för steg-guide. Men i princip så kan man testa med följande:

Logga in med ditt MSDN-konto och skapa en devopsresurs och se om du kan få till en pipeline utifrån ditt github-konto och det clonade projektet.

Om intresse finns, kan vi ta denna del i en annan workshop.