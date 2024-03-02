<Query Kind="Statements" />

DateTime effectiveDate = DateTime.Today;
var z = SchoolYearTermTypes.Join(SchoolYears, e => e.SchoolYear, d => d, (e,d) => new { e }).Where(w => w.e.SchoolYearTermTypeId == 144 && w.e.SchoolYear.CurrentSchoolYearFlag).FirstOrDefault();
var g = Groups.Include(i => i.GroupCategory).Include(i => i.Expression).Where(w => w.SchoolID == 90 && w.EffectiveStartDate <= effectiveDate && w.EffectiveStartDate >= effectiveDate).Select(w => w);
if (z is null)
	g.Where(w => w.ForNextYearScheduling).Dump();
else
	g.Where(w => !w.ForNextYearScheduling).Dump();
	

	