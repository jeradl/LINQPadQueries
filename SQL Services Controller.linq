<Query Kind="Statements">
  <NuGetReference>System.ServiceProcess.ServiceController</NuGetReference>
  <Namespace>System.ServiceProcess</Namespace>
</Query>

var mainService = new ServiceController("MSSQLSERVER"); 
var agentService = new ServiceController("SQLSERVERAGENT");

if (mainService.Status == ServiceControllerStatus.Stopped) 
{
	mainService.Start();
	agentService.Start();
}
else
{
	mainService.Stop();
}

mainService.Status.Dump();