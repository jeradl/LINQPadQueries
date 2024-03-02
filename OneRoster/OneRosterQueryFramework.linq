<Query Kind="Program">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.Dynamic.dll</Reference>
  <Namespace>System.Dynamic</Namespace>
</Query>

void Main()
{
	var classQuery = Sections.Where(w => w.SchoolTerm.CurrentTermFlag).Select(s => new OneRosterClass
	{
		Title = s.SchoolCourse.CourseMaster.FullCourseName,
		ClassCode = s.SchoolCourse.CourseMaster.CourseNumber,
		ClassType = ClassType.Scheduled,
		Location = s.SectionInstructionalSpaces.Select(sis => sis.InstructionalSpace.SpaceNameOrNumber).FirstOrDefault(),
		CourseId = s.SchoolCourse.CourseMaster.CourseID,
		SchoolId = s.SchoolCourse.School.SchoolID,
		TermIds = s.SchoolTerm.SchoolTermGradingPeriods.Select(st => st.ID).ToList(),
		Periods = s.SectionPeriods.Select(sp => sp.SchoolPeriod.Description).ToList(),
		Id = s.SectionID.ToString(),
		Status = StatusType.Active,
		UpdatedAt = s.ModifiedDate ?? s.CreatedDate.Value
	});
	
	classQuery.ToList().Dump();

	var classes = ApplyBinding(classQuery);

	classes.ForEach(f => f.Course = BuildGuidRef(GuidType.Course, "courses/{0}", f.CourseId.ToString()));
	classes.ForEach(f => f.School = BuildGuidRef(GuidType.Org, "schools/{0}", f.SchoolId.ToString()));
	classes.ForEach(f => f.Terms = BuildGuidRefList(GuidType.Term, "terms/{0}", f.TermIds));
	
	JsonOk(classes, "classes").Dump();
}

// Define other methods and classes here
private static bool StringEquals(string a, string b)
{
	return a.Equals(b, StringComparison.OrdinalIgnoreCase);
}

private static bool StringNotEquals(string a, string b)
{
	return !StringEquals(a, b);
}

private static bool StringGreaterThan(string a, string b)
{
	return String.Compare(a, b, true) > 0;
}

private static bool StringLessThan(string a, string b)
{
	return String.Compare(a, b, true) < 0;
}

private static bool StringGTE(string a, string b)
{
	return StringGreaterThan(a, b) || StringEquals(a, b);
}

private static bool StringLTE(string a, string b)
{
	return StringLessThan(a, b) || StringEquals(a, b);
}

private static bool StringContains(string a, string b)
{
	return a.ToLower().Contains(b);
}

private static Dictionary<string, Func<string, string, bool>> StringComparatorMap = new Dictionary<string, Func<string, string, bool>>
{
	["="] = StringEquals,
	["!="] = StringNotEquals,
	[">"] = StringGreaterThan,
	[">="] = StringGTE,
	["<="] = StringLessThan,
	["<="] = StringLTE,
	["~"] = StringContains
};

private static Dictionary<string, string> ComparatorMap = new Dictionary<string, string>
{
	["="] = "op_Equality",
	["!="] = "op_Inequality",
	[">"] = "op_GreaterThan",
	[">="] = "op_GreaterThanOrEqual",
	["<"] = "op_LessThan",
	["<="] = "op_LessThanOrEqual",
	["~"] = "Contains"
};

private static Regex filterMatcher = new Regex("(.*)(=|!=|>|>=|<|<=|~)'(.*)'");

private static Func<T, bool> TranslateFilter<T>(string filter)
{
	Type modelType = typeof(T);

	var match = filterMatcher.Match(filter);

	if (!match.Success)
	{
		throw new Exception();
	}

	var dataFieldRaw = match.Groups[1].Value;
	var dataField = dataFieldRaw.ToLower();
	var predicate = match.Groups[2].Value;
	var value = match.Groups[3].Value.ToLower();
	
	var lookupMethod = modelType.GetMethod("LookupProperty", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

	var modelProp = (PropertyInfo)lookupMethod.Invoke(null, new object[] { modelType, dataField });

	var fieldType = modelProp.PropertyType;
	var getter = modelProp.GetMethod;
	var comparisonValue = fieldType.IsEnum ? value : Convert.ChangeType(value, fieldType);

	var comparator = fieldType.IsEnum ? typeof(string).GetMethod(ComparatorMap[predicate]) : fieldType.GetMethod(ComparatorMap[predicate]);

	if (fieldType == typeof(string))
	{
		var stringComparator = StringComparatorMap[predicate];

		return u => stringComparator.Invoke((string)getter.Invoke(u, null), (string)comparisonValue);
	}
	else if (!fieldType.IsEnum)
	{
		if (comparator.IsStatic)
			return u => (bool)comparator.Invoke(null, new object[] { getter.Invoke(u, null), comparisonValue });

		return u => (bool)comparator.Invoke(getter.Invoke(u, null), new object[] { comparisonValue });
	}
	else
	{
		if (comparator.IsStatic)
			return u => (bool)comparator.Invoke(null, new object[] { Enum.GetName(fieldType, getter.Invoke(u, null)).ToLower(), comparisonValue });

		return u => (bool)comparator.Invoke(Enum.GetName(fieldType, getter.Invoke(u, null)).ToLower(), new object[] { comparisonValue });
	}
}

private static IQueryable<T> ApplyFilter<T>(IQueryable<T> modelQuery, string filter)
{
	Type modelType = typeof(T);

	var query = modelQuery;

	if (!String.IsNullOrEmpty(filter))
	{
		//var filter = filterValues.First();

		string logicalOperator = null;
		string left = null;
		string right = null;

		if (filter.Contains(" AND "))
		{
			logicalOperator = "AND";
			var pieces = filter.Split(new[] { " AND " }, StringSplitOptions.None);
			left = pieces[0];
			right = pieces[1];
		}
		else if (filter.Contains(" OR "))
		{
			logicalOperator = "OR";
			var pieces = filter.Split(new[] { " OR " }, StringSplitOptions.None);
			left = pieces[0];
			right = pieces[1];
		}
		else
		{
			left = filter;
		}

		if (logicalOperator == null)
		{
			return query.Where(TranslateFilter<T>(left)).AsQueryable<T>();
		}

		var leftQuery = query.Where(TranslateFilter<T>(left)).AsQueryable<T>();
		var rightQuery = query.Where(TranslateFilter<T>(right)).AsQueryable<T>();

		if (logicalOperator == "AND")
		{
			return leftQuery.Intersect(rightQuery);
		}
		else
		{
			return leftQuery.Union(rightQuery);
		}
	}

	return query;
}

private static Func<T, Object> TranslateSort<T, Object>(string fieldName)
{
	Type modelType = typeof(T);

	var lookupMethod = modelType.GetMethod("LookupProperty", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
	var modelProp = (PropertyInfo)lookupMethod.Invoke(null, new object[] { modelType, fieldName });

	var getter = modelProp.GetMethod;

	return u => (Object)getter.Invoke(u, null);

}

private static IQueryable<T> ApplySort<T>(IQueryable<T> modelQuery, IList<string> datafields, IList<string> orderBy)
{
	if (datafields.Count < 1 || datafields.FirstOrDefault() == null)
	{
		datafields.Clear();
		datafields.Add("Id");
	}

	var dataField = datafields.First();

	dataField = dataField.Replace("'", "");

	if (orderBy.Count > 0 && orderBy.First() != null && orderBy.First().Replace("'", "").ToLower() == "asc")
		return modelQuery.OrderBy(TranslateSort<T, Object>(dataField)).AsQueryable();

	return modelQuery.OrderByDescending(TranslateSort<T, Object>(dataField)).AsQueryable();
}

private IQueryable<T> ApplyPaging<T>(IQueryable<T> modelQuery) => modelQuery.Skip(0).Take(100);

public List<T> ApplyBinding<T>(IQueryable<T> modelQuery)
{
	var query = modelQuery;
	try
	{
		query = ApplyFilter(query, "status='active'");
	}
	catch (Exception ex)
	{
		throw ex;
	}

	//int responseCount = query.Count();

	try
	{
		query = ApplySort(query, new List<string> { "id" }, new List<string> { "desc" });
	}
	catch (Exception ex)
	{
		throw ex;
	}

	query = ApplyPaging(query);
	
	var queryList = query.ToList();
	
	int count = queryList.Count;
	
	return queryList;
}

private GuidRef BuildGuidRef(GuidType guidType, string endpoint, string id)
{
	var guid = new GuidRef(guidType);

	string ep = String.Format(endpoint, id);

	guid.RefId = id.ToString();
	guid.Href = new Uri($"localhost:27291/{ep}");

	return guid;
}

private IList<GuidRef> BuildGuidRefList(GuidType guidType, string endpoint, IList<int> ids)
{
	var refList = ids.Select(s => BuildGuidRef(guidType, endpoint, s.ToString())).ToList();

	return refList;
}

private string ConvertJson<T>(T model, string root)
{
	object obj;

	if (!String.IsNullOrEmpty(root))
	{
		dynamic expando = new ExpandoObject();
		IDictionary<string, object> dic = expando;
		dic[root] = model;
		obj = dic;
	}
	else
	{
		obj = model;
	}

	return JsonConvert.SerializeObject(obj);
}

private string JsonOk<T>(T obj, string root)
{
	return ConvertJson(obj, root);
}