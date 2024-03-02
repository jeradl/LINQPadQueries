<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.ServiceModel.dll</Reference>
  <NuGetReference>FastMember</NuGetReference>
  <Namespace>FastMember</Namespace>
  <Namespace>System.ServiceModel</Namespace>
</Query>

E2020APIClient client = new E2020APIClient();

var q = client.GetStudentCourseProgressData("", "", 38258, null, null, null);
q.Dump();

var props = typeof(StudentCourseProgressDataDTO).GetProperties().Select(s => s.Name).ToArray();
props.Dump();

DataTable table = new DataTable();
using (var reader = ObjectReader.Create(q.StudentCourseProgressDataList, props))
{
	reader.Dump();
	table.Load(reader);
}

table.Dump();