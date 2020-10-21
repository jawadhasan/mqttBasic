
  function publishApp{ 
  Param($sourcePath, $targetPath)

write-host $sourcePath $targetPath

  dotnet publish $sourcePath -c debug -r linux-x64 --self-contained true -o $targetPath #work on it

}

 <# 

	This Script, Targets the .NET Core Console Applications in this solution folder.
	It will Build the application, Delete the old Publish folder (if any), Publish new output into specific dist folder (Inside docker directory)
	However, this script can be optimized as it can leverage functions for common tasks
 #>
 Write-Host "Working-Dir" ${pwd} -ForegroundColor Yellow 
 
 Write-Host ***********************BrokerApp*********************************************** -ForegroundColor Red
 
 $BrokerAppSourcePath = ".\MqttBasic\ConsoleBroker\ConsoleBroker.csproj"
 $BrokerAppPublishPath =  Join-Path -Path ${pwd} -ChildPath "docker/broker/dist-broker"
 
 #Delete BrokerApp publish folder
 Write-Host "Deleting BrokerApp Publish-Path:" $BrokerAppPublishPath -ForegroundColor Green
 Remove-Item -Recurse -Force $BrokerAppPublishPath -ErrorAction Ignore

 #Publish BrokerApp
 Write-Host "Publishing BrokerApp Publish-Path:" $BrokerAppPublishPath -ForegroundColor Green 
 dotnet publish $BrokerAppSourcePath -c debug -r linux-x64 --self-contained true -o $BrokerAppPublishPath
 
Write-Host ===================================================================================================== -ForegroundColor Red



Write-Host ***********************PublisherApp*********************************************** -ForegroundColor Red

$PublisherAppSourcePath = ".\MqttBasic\ConsolePublisher\ConsolePublisher.csproj"
$PublisherAppPublishPath =  Join-Path -Path ${pwd} -ChildPath "docker/publisher/dist-publisher"

#Delete PublisherApp publish folder
Write-Host "Deleting PublisherApp Publish-Path:" $PublisherAppPublishPath -ForegroundColor Green
Remove-Item -Recurse -Force $PublisherAppPublishPath -ErrorAction Ignore

#Publish PublisherApp
Write-Host "Publishing PublisherApp Publish-Path:" $PublisherAppPublishPath -ForegroundColor Green
dotnet publish $PublisherAppSourcePath -c debug -r linux-x64 --self-contained true -o $PublisherAppPublishPath

Write-Host ===================================================================================================== -ForegroundColor Red
 
 
 
Write-Host ***********************SubscriberApp*********************************************** -ForegroundColor Red

$SubscriberAppSourcePath = ".\MqttBasic\ConsoleSubscriber\ConsoleSubscriber.csproj"
$SubscriberAppPublishPath =  Join-Path -Path ${pwd} -ChildPath "docker/subscriber/dist-subscriber"

#Delete SubscriberApp publish folder
Write-Host "Deleting SubscriberApp Publish-Path:" $SubscriberAppPublishPath -ForegroundColor Green
Remove-Item -Recurse -Force $SubscriberAppPublishPath -ErrorAction Ignore

#Publish SubscriberApp
Write-Host "Publishing SubscriberApp Publish-Path:" $SubscriberAppPublishPath -ForegroundColor Green
dotnet publish $SubscriberAppSourcePath -c debug -r linux-x64 --self-contained true -o $SubscriberAppPublishPath

Write-Host ===================================================================================================== -ForegroundColor Red
