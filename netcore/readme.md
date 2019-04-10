# .net core counter
A simple .net core web application. It will use linux containers to build 
the source code and create a docker image.  
When running the container, the container will listen on port 80. 
When running docker for windows, you have to bind a port on the host to be able
to access it. Using `-p 5000:80` will bind port 5000 
on the host `http://localhost:5000` to the container port 80.

## Build the docker image
### Using docker file

type `docker build . ` to build the image with no tag

type `docker build -t myname:tag . ` to build the image with a name `myname`and tag `tag`

### Using shell script 

type `. build_docker.sh` to build an image with name `netcore-counter` and current time stamp as tag

## Run the docker image

By running/testing the docker image locally, you can be confident that it
will run on other machines as well, using exactly the same dependencies and frame
work versions. "Works on my machine"-approved   

type `docker run -p 5000:80 <sha>` to run the image using sha and listen on port 5000  
type `docker run -p 5000:80 myname:tag` to run the image using name and a specific tag and listen on port 5000  
type `docker run -p 5000:80 netcore-counter` to run the latest image built using the script and listen on port 5000

## Azure devops

The file azure-pipelines.yml contains the steps that will run on the build
server on Azure. It can also be used to define variables and which 
kind of server to use i.e. Ubuntu 16.04. This is referenced as a pool 
of equal servers. 

## Scripts

By using scripts, you can easily set up a build pipeline withing minutes. 
You can also verify the build
pipeline locally before pushing it to the registry. You should avoid 
depending on specific azure pre-defined build steps, otherwise it will 
be impossible to test and verify locally.