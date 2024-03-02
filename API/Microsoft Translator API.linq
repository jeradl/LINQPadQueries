<Query Kind="Program">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

static void Main()
{
	Translate();
}

const string host = "https://api.cognitive.microsofttranslator.com";
const string path = "/translate?api-version=3.0";
const string params_ = "&from=en&to=es";

const string uri = host + path + params_;

const string key = "";

static readonly string text = @"Grade Level Assessment of Standards";

async static void Translate()
{
	object[] body = new object[] { new { Text = text } };
	var requestBody = JsonConvert.SerializeObject(body);
	
	using (var client = new HttpClient())
	using (var request = new HttpRequestMessage())
	{
		request.Method = HttpMethod.Post;
		request.RequestUri = new Uri(uri);
		
		request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
		request.Headers.Add("Ocp-Apim-Subscription-Key", key);
		
		var response = await client.SendAsync(request);
		var responseBody = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Newtonsoft.Json.Formatting.Indented);
		
		result.Dump();
	}	
}