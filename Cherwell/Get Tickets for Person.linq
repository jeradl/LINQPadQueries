<Query Kind="Program">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var ClientID = "";
	var Username = @"";
	var Password = "";
	var _apiRoot = "";
	HttpResponseMessage response;

	var _identifiers = new Dictionary<CherwellProperty, string>()
	{
		{ CherwellProperty.BusinessObjectID, "BO:6dd53665c0c24cab86870a21cf6434ae,FI:6dd53665c0c24cab86870a21cf6434ae" },
		{ CherwellProperty.IncidentID, "BO:6dd53665c0c24cab86870a21cf6434ae,FI:6ae282c55e8e4266ae66ffc070c17fa3"},
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

	var endpoint = $"getsearchresults";

	var requestjson = new
	{
		busObId = "6dd53665c0c24cab86870a21cf6434ae",
		fields = new []
				{
					_identifiers[CherwellProperty.IncidentID],
					_identifiers[CherwellProperty.Status],
					_identifiers[CherwellProperty.CreatedDateTime],
					_identifiers[CherwellProperty.ClosedDateTime],
					_identifiers[CherwellProperty.Description]
				},
		filters = new[]
				{
					new
					{
                        //field ID for personID. will probably be different in prod
                        fieldId = "BO:6dd53665c0c24cab86870a21cf6434ae,FI:945f021678200731ca64dd41f2866eb485eddbd37d",
						@operator = "equals",
						value = "12345"
					}
				},
		includeAllFields = false,
		includeSchema = true,
		pageNumber = 0,
		pageSize = 0,
		sorting = new[]
				{
					new
					{
                        //this is the CreatedByDate field
                        fieldId = "BO:6dd53665c0c24cab86870a21cf6434ae,FI:c1e86f31eb2c4c5f8e8615a5189e9b19",
						sortDirection = 1
					}
				}
	};

	string serializedJson = JsonConvert.SerializeObject(requestjson);

	var request = new HttpRequestMessage();
	request.Method = HttpMethod.Post;
	request.RequestUri = new Uri($"{_client.BaseAddress}/{_apiRoot}/{endpoint}");
	request.Headers.Accept.Clear();
	request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
	request.Content = new StringContent(serializedJson, Encoding.UTF8, "application/json");

	response = await _client.SendAsync(request, CancellationToken.None);
	
	var json = await response.Content.ReadAsStringAsync();	

	var incidentList = JsonConvert.DeserializeObject<IncidentList>(json).Dump();

	if (incidentList?.Incidents != null)
	{
		var incidents = incidentList.Incidents;
		
		List<CherwellTicketModel> model = new List<CherwellTicketModel>();
		
		foreach (var inc in incidents)
		{
			var fields = inc.Fields;
			var ticket = new CherwellTicketModel();
			
			int incidentID;
			Int32.TryParse(fields.SingleOrDefault(s => s.FieldID == _identifiers[CherwellProperty.IncidentID]).Value, out incidentID);
			
			DateTime createdDate;
			DateTime.TryParse(fields.SingleOrDefault(s => s.FieldID == _identifiers[CherwellProperty.CreatedDateTime]).Value, out createdDate);
			
			DateTime closedDate;
			DateTime.TryParse(fields.SingleOrDefault(s => s.FieldID == _identifiers[CherwellProperty.ClosedDateTime]).Value, out closedDate);
			
			ticket.IncidentID = incidentID;
			ticket.CreatedDate = createdDate;
			ticket.ClosedDate = closedDate;
			ticket.Status = fields.SingleOrDefault(s => s.FieldID == _identifiers[CherwellProperty.Status]).Value;
			ticket.Description = fields.SingleOrDefault(s => s.FieldID == _identifiers[CherwellProperty.Description]).Value;
			
			model.Add(ticket);
		}
		model.Dump();
	}
}


class Field
{
	[JsonProperty("fieldId")]
	public string FieldID { get; set; }
	[JsonProperty("name")]
	public string FieldName { get; set; }
	[JsonProperty("value")]
	public string Value { get; set; }
}

class Incident
{
	[JsonProperty("busObPublicId")]
	public int IncidentID { get; set; }
	[JsonProperty("fields")]
	public List<Field> Fields { get; set; }
}

class IncidentList
{
	[JsonProperty("businessObjects")]
	public List<Incident> Incidents { get; set; }
}

public enum CherwellProperty
{
	BusinessObjectID,
	IncidentID,
	Description,
	Priority,
	CustomerDisplayName,
	Subcategory,
	Service,
	Category,
	PersonID,
	StudentID,
	CreatedDateTime,
	ClosedDateTime,
	Status
}