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
	var _apiRoot = "api/v1";
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

	var endpoint = $"{_apiRoot}/getbusinessobject/busobid/6dd53665c0c24cab86870a21cf6434ae/publicid/53498";

	response = await _client.GetAsync(endpoint);

	var json = await response.Content.ReadAsStringAsync();

	var fieldList = JsonConvert.DeserializeObject<Incident>(json);
	
	var model = new CherwellTicketModel();

	if (fieldList?.Fields != null)
	{
		var fields = fieldList.Fields;
				
		int incidentID;
		Int32.TryParse(fields.SingleOrDefault(s => s.FieldID == _identifiers[CherwellProperty.IncidentID]).Value, out incidentID);

		int personID;
		Int32.TryParse(fields.SingleOrDefault(s => s.FieldID == _identifiers[CherwellProperty.PersonID]).Value, out personID);

		int studentID;
		Int32.TryParse(fields.SingleOrDefault(s => s.FieldID == _identifiers[CherwellProperty.StudentID]).Value, out studentID);
		
		DateTime createdDate;
		DateTime.TryParse(fields.SingleOrDefault(s => s.FieldID == _identifiers[CherwellProperty.CreatedDateTime]).Value, out createdDate);
		
		DateTime closedDate;
		DateTime.TryParse(fields.SingleOrDefault(s => s.FieldID == _identifiers[CherwellProperty.ClosedDateTime]).Value, out closedDate);

		model.IncidentID = incidentID;
		model.PersonID = personID;
		model.StudentID = studentID;
		model.PersonName = fields.SingleOrDefault(s => s.FieldID == _identifiers[CherwellProperty.CustomerDisplayName]).Value;
		model.Description = fields.SingleOrDefault(s => s.FieldID == _identifiers[CherwellProperty.Description]).Value;
		model.CreatedDate = createdDate;
		model.ClosedDate = closedDate;
		model.Status = fields.SingleOrDefault(s => s.FieldID == _identifiers[CherwellProperty.Status]).Value;
	}

	model.Dump();
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