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

var _identifiers = new Dictionary<CherwellProperty, string>()
	{
		{ CherwellProperty.BusinessObjectID, "6dd53665c0c24cab86870a21cf6434ae" },
        { CherwellProperty.IncidentID, "BO:6dd53665c0c24cab86870a21cf6434ae,FI:6ae282c55e8e4266ae66ffc070c17fa3" },
        { CherwellProperty.Description, "BO:6dd53665c0c24cab86870a21cf6434ae,FI:252b836fc72c4149915053ca1131d138" },
        { CherwellProperty.Priority, "BO:6dd53665c0c24cab86870a21cf6434ae,FI:83c36313e97b4e6b9028aff3b401b71c" },
        { CherwellProperty.CustomerDisplayName, "BO:6dd53665c0c24cab86870a21cf6434ae,FI:93734aaff77b19d1fcfd1d4b4aba1b0af895f25788" },
        { CherwellProperty.Subcategory, "BO:6dd53665c0c24cab86870a21cf6434ae,FI:1163fda7e6a44f40bb94d2b47cc58f46" },
        { CherwellProperty.Service, "BO:6dd53665c0c24cab86870a21cf6434ae,FI:936725cd10c735d1dd8c5b4cd4969cb0bd833655f4" },
        { CherwellProperty.Category, "BO:6dd53665c0c24cab86870a21cf6434ae,FI:9e0b434034e94781ab29598150f388aa" },
        { CherwellProperty.PersonID, "BO:6dd53665c0c24cab86870a21cf6434ae,FI:945f021678200731ca64dd41f2866eb485eddbd37d" },
        { CherwellProperty.StudentID, "BO:6dd53665c0c24cab86870a21cf6434ae,FI:945f0217f0905bd4aa62294a1381d646ebe4bd23d0" },
		{ CherwellProperty.CreatedDateTime, "BO:6dd53665c0c24cab86870a21cf6434ae,FI:c1e86f31eb2c4c5f8e8615a5189e9b19" },
		{ CherwellProperty.ClosedDateTime, "BO:6dd53665c0c24cab86870a21cf6434ae,FI:11b6961ee55048b9a7240f7e2d3a2f8d" },
		{ CherwellProperty.Status, "BO:6dd53665c0c24cab86870a21cf6434ae,FI:5eb3234ae1344c64a19819eda437f18d" }
	};

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
//_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

//post save data
var endpoint = "savebusinessobject";

var ticket = new CherwellTicketModel
{
	Description = "",
	PersonName = "",
	PersonID = 12345,
	StudentID = 67890
};

var json = new
{
	busObId = _identifiers[CherwellProperty.BusinessObjectID],
	cacheScope = "Tenant",
	fields = new[]
				{
					new
					{
                        //description
                        fieldId = _identifiers[CherwellProperty.Description],
						dirty = true,
						html = ticket.Description,
						value = ticket.Description
                    },
                    new
                    {
                        //priority
                        fieldId = _identifiers[CherwellProperty.Priority],
                        dirty = true,
                        html = "null",
                        value = "2"
                    },
                    new
                    {
                        //customer display name
                        fieldId = _identifiers[CherwellProperty.CustomerDisplayName],
                        dirty = true,
                        html = "null",
                        value = ticket.PersonName
                    },
                    new
                    {
                        //subcategory
                        fieldId = _identifiers[CherwellProperty.Subcategory],
                        dirty = true,
                        html = "null",
                        value = "General Inquiry"
                    },
                    new
                    {
                        //service
                        fieldId = _identifiers[CherwellProperty.Service],
                        dirty = true,
                        html = "null",
                        value = "eLearning Companion Device (eLCD)"
                    },
                    new
                    {
                        //category
                        fieldId = _identifiers[CherwellProperty.Category],
                        dirty = true,
                        html = "null",
                        value = "General Inquiry"
					},
					new
					{
                        //person id
                        fieldId = _identifiers[CherwellProperty.PersonID],
						dirty = true,
						html = "null",
						value = ticket.PersonID.ToString()
					},
					new
					{
                        //student id
                        fieldId = _identifiers[CherwellProperty.StudentID],
						dirty = true,
						html = "null",
						value = ticket.StudentID.ToString()
					}
				},
	persist = true
};

string serializedJson = JsonConvert.SerializeObject(json);

var request = new HttpRequestMessage();
request.Method = HttpMethod.Post;
request.RequestUri = new Uri($"{_client.BaseAddress}/{_apiRoot}/{endpoint}");
request.Headers.Accept.Clear();
request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
request.Content = new StringContent(serializedJson, Encoding.UTF8, "application/json");

response = await _client.SendAsync(request, CancellationToken.None);

var jsonResponse = await response.Content.ReadAsStringAsync();
jsonResponse.Dump();
return;
JProperty.Parse(jsonResponse).Dump();

var q = JObject.Parse(jsonResponse);
//q.Dump();