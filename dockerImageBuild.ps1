docker image build --tag dotnetbroker --file docker\broker\Dockerfile .\docker\broker
docker image build --tag dotnetpublisher --file docker\publisher\Dockerfile .\docker\publisher
docker image build --tag dotnetsubscriber --file docker\subscriber\Dockerfile .\docker\subscriber

