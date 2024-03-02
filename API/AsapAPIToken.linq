<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

var apiData = new
{
	username = "",
	password = "",
	orgId = "",
	apiKey = ""
};

string connStr = $"user={apiData.username}&password={apiData.password}&organizationId={apiData.orgId}&apiKey={apiData.apiKey}";

HttpWebRequest request = (HttpWebRequest)WebRequest.Create("");
request.Headers.Add(HttpRequestHeader.Authorization, connStr);
request.Accept = "application/json";
HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
string token = resp.Headers["asap_accesstoken"];
token.Dump();

resp.Close();