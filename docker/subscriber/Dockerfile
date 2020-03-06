# escape=`
FROM ubuntu
LABEL author="Jawad Hasan"

RUN apt-get update
RUN apt-get install -y

#on container
WORKDIR /app

COPY dist-subscriber/ .
CMD ["./ConsoleSubscriber"]




