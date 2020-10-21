# escape=`
FROM ubuntu
LABEL author="Jawad Hasan"

#RUN: Used to execute a command during the build process of the docker image
RUN apt-get update
RUN apt-get install -y

#on container
WORKDIR /app

COPY dist-broker/ .


#CMD: Used for executing commands when we build a new container from the docker image
CMD ["./ConsoleBroker"]




