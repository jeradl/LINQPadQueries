<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
</Query>

IQueryable<Org> schoolsQuery = Schools.Select(s => new Org
{
	Name = s.SchoolName,
	Type = OrgType.School,
	Identifier = s.SchoolNCESID,
	Id = s.SchoolID.ToString(),
	UpdatedAt = s.ModifiedDate ?? s.CreatedDate.Value,
	Status = s.SchoolOpenFlag ? StatusType.Active : StatusType.ToBeDeleted
});

var value = "2016-01-01t01:01:01.111z";
var modelType = typeof(Org);
var lookupMethod = modelType.GetMethod("LookupPrSoperty", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
var modelProp = (PropertyInfo)lookupMethod.Invoke(null, new object[] { modelType, "datelastmodified"});
var fieldType = modelProp.PropertyType;
var getter = modelProp.GetMethod;
var comparisonValue = Convert.ChangeType(value, fieldType);
var comparator = fieldType.GetMethod("op_GreaterThan");

Func<Org, bool> pred = w => (bool)comparator.Invoke(null, new object[] { getter.Invoke(w, null), comparisonValue });

var q = schoolsQuery.Where(pred).AsQueryable();

q.Dump();