<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
</Query>

string apiKey = "";
string apiSecret = "";
long SSID = 0000000000;

string tokenEndpoint = "api/v1/token";
string endpoint = "api/v1/students/{0}/score-reports";
byte[] bytes = Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}");
string base64str = Convert.ToBase64String(bytes);
base64str.Dump();

var client = new HttpClient();
client.BaseAddress = new Uri("");
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64str);
client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();
client.DefaultRequestHeaders.CacheControl.NoCache = true;

var tokenResp = client.GetStringAsync(tokenEndpoint);
var tokenResult = JObject.Parse(tokenResp.Result);
var	tokenValue = tokenResult.GetValue("token");

if (tokenValue == null)
	throw new Exception();

client.DefaultRequestHeaders.Clear();
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenValue.ToString());

var resp = client.GetStringAsync(String.Format(endpoint, SSID));
var reports = JArray.Parse(resp.Result);

List<ScoreReportModel> reps = new List<ScoreReportModel>();

//reps = reports.Select(s => new ScoreReportModel
//{
//	Grade = (string)s["grade"]
//}).ToList();

foreach (var rep in reports)
{
	var model = new ScoreReportModel();
	model.Grade = rep["grade"].ToString();
	model.Language = rep["language"].ToString();
	model.ReportCode = rep["type"].ToString();
	//model.ReportDesc = String.Empty;
	model.Url = rep["url"].ToString();
	model.Year = rep["year"].ToString();
	reps.Add(model);
}

resp.Dump();
reps.Dump();