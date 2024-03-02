<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
</Query>

var ClientID = "";
var Username = @"";
var Password = "";
var _apiRoot = "";
HttpResponseMessage response;

HttpClient _client = new HttpClient();

//token
_client.BaseAddress = new Uri("");

var formContent = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("grant_type", "password"),
				new KeyValuePair<string, string>("client_id", ClientID),
				new KeyValuePair<string, string>("username", Username),
				new KeyValuePair<string, string>("password", Password)
			});

response = await _client.PostAsync("token", formContent);
var tokenResponse = await response.Content.ReadAsStringAsync();

var jObj = JObject.Parse(tokenResponse);
var token = jObj["access_token"].ToString();

//set token headers
_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);