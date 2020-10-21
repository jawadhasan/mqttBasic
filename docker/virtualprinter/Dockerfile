# escape=`
FROM ubuntu
LABEL author="Jawad Hasan"

RUN apt-get update
RUN apt-get install -y
Env MY_ID="virtualprinter1"
#on container
WORKDIR /app

COPY dist-virtualprinter/ .
CMD ["./VirtualPrinter"]




