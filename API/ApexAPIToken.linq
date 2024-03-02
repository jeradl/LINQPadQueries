<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>System.Net.Http</Namespace>
</Query>

var postTokenData = new
{
	grant_type = "client_credentials",
	client_id = "",
	client_secret = ""
};

var q = JsonConvert.SerializeObject(postTokenData);

HttpClient c = new HttpClient();
c.BaseAddress = new Uri("");
var resp = await c.PostAsync("token", new StringContent(q, Encoding.UTF8, "application/json"));
var obj = resp.Content.ReadAsStringAsync();
obj.Dump();
c.Dispose();