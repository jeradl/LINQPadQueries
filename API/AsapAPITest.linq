<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

var _apiKey = "";

HttpClient c = new HttpClient();
c.BaseAddress = new Uri("");
c.DefaultRequestHeaders.Add("asap_accesstoken", _apiKey);
c.DefaultRequestHeaders.Add("Accept", "application/json");

var q = await c.GetAsync("coursesclasses");
var z = await q.Content.ReadAsStringAsync();

var data = JsonConvert.DeserializeObject<List<CourseDTO>>(z);

data.Dump();