<Query Kind="Statements" />

int studentsWithoutCourses = 0;

var set = RecommendationSets.Include("RecommendationFilters").Include("RecommendationFilters.RecommendationCourses").Single(s => s.RecommendationSetID == 4814);
foreach (var filter in set.RecommendationFilters)
{
	if (filter.RecommendationCourses.Count == 0)
		studentsWithoutCourses += filter.StudentCount;
}

set.StudentSubGroupCount = studentsWithoutCourses;

if (set.EntityState == System.Data.Entity.EntityState.Detached)
{
	RecommendationSets.Attach(set);
	RecommendationSets.Context.ObjectStateManager.ChangeObjectState(set, System.Data.Entity.EntityState.Modified);
}
	
	
SaveChanges();