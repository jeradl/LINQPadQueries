<Query Kind="Statements" />

DateTime effectiveDate = DateTime.Today;

SchoolYearTermType syttc = (from sytt in SchoolYearTermTypes
							join sy in SchoolYears on sytt.SchoolYear equals sy
							where sy.CurrentSchoolYearFlag && sytt.SchoolYearTermTypeId == 144
							select sytt).FirstOrDefault();

var q = from g in Groups.Include("GroupCategory").Include("Expression")
		where g.SchoolID == 90 && g.EffectiveStartDate <= effectiveDate && g.EffectiveEndDate >= effectiveDate
		select g;

IQueryable<Group> group;

if (syttc is null)
	group = q.Where(a => a.ForNextYearScheduling);
else
	group = q.Where(a => !a.ForNextYearScheduling);

//4.479
//1.323
var m = group.Select(s => new GroupDc
{
	GroupName = s.Name,
	GroupID = s.GroupID
}).ToList();

//var groupList = group.ToList();
//
//groupList.Select(s => new GroupDc
//{
//	GroupName = s.Name,
//	GroupID = s.GroupID
//}).ToList();