<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.Data.dll</Reference>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

Func<SqlDataReader, OneRosterClassDTO> handler = (r) => 
{
	var orc = new OneRosterClassDTO();

	orc.Title = r.GetString(0);
	orc.ClassCode = r.GetString(1);
	orc.ClassType = r.GetString(2);
	orc.Location = r.GetString(3);
	orc.CourseId = r.GetInt32(4);
	orc.SchoolId = r.GetInt32(5);
	orc.TermId = r.GetInt32(6);
	orc.Period = r.GetString(7);
	orc.Id = r.GetInt32(8);
	orc.Status = r.GetString(9);
	orc.UpdatedAt = r.GetDateTime(10);

	return orc;
};

var classes = SqlHelper.ExecuteStoredProcedure("spOneRosterGetClasses", null, handler);

var groupedClasses = classes.GroupBy(cl => cl.Id);

var classList = new List<OneRosterClass>();

foreach (var c in groupedClasses)
{
	var cra = c.ToList();
	var orc = new OneRosterClass();
	
	orc.Title = cra.First().Title;
	orc.ClassCode = cra.First().ClassCode;
	orc.ClassType = ClassType.Scheduled;
	orc.Location = cra.First().Location;
	orc.CourseId = cra.First().CourseId;
	orc.SchoolId = cra.First().SchoolId;
	orc.TermIds = cra.Select(cr => cr.TermId).ToList();
	orc.Periods = cra.Select(cr => cr.Period).ToList();
	orc.Id = cra.First().Id.ToString();
	orc.Status = StatusType.Active;
	orc.UpdatedAt = cra.First().UpdatedAt;
	
	classList.Add(orc);
}

classList.Dump();