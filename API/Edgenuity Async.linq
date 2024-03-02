<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.ServiceModel.dll</Reference>
  <NuGetReference>FastMember</NuGetReference>
  <Namespace>FastMember</Namespace>
  <Namespace>System.ServiceModel</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

E2020APIClient _client = new E2020APIClient();
string _apiUser = "";
string _apiKey = "";

List<StudentCourseProgressData> results = new List<StudentCourseProgressData>();

//edgenuity school ids
int[] edSchoolIds = { 38257, 36529, 38255, 38258, 38259, 38261, 38263, 38260, 37018, 38264, 34342, 38256, 38262, 29356 };

var tasks = edSchoolIds.Select(s => _client.GetStudentCourseProgressDataAsync(_apiUser, _apiKey, s, true, null, null));

var progress = await Task.WhenAll(tasks);

foreach (var prog in progress)
{
	if (prog == null) continue;

	results.AddRange(prog.StudentCourseProgressDataList);
}

var props = typeof(StudentCourseProgressDataDTO).GetProperties().Select(s => s.Name).ToArray();

DataTable table = new DataTable();
using (var reader = ObjectReader.Create(results, props))
{
	table.Load(reader);
}

table.Dump();