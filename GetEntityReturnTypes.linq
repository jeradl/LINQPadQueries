<Query Kind="Program" />

void Main()
{
	Assembly pf = typeof(PersonFacade).Assembly;
	Type[] types = pf.GetTypes();//.Dump();
	string[] filter = { "Equals", "GetHashCode", "ToString", "GetType" };
	List<MethodInfo> mi = new List<MethodInfo>();
	foreach (Type t in types)
	{
		var m = t.GetMethods().Where(w => w.IsPublic && !filter.Contains(w.Name) && (w.ReturnType.BaseType == typeof(EntityObject) || w.ReturnType.GenericTypeArguments.Any(a => a.BaseType == typeof(EntityObject)))); 
		mi.AddRange(m);
	}

	//mi.Dump();

	mi.Select(m => new AtlasMethodInfo
	{
		Name = m.Name,
		Parent = m.DeclaringType.Name,
		ReturnType = m.ReturnType.Name,
		Types = String.Join(", ", m.ReturnType.GenericTypeArguments.Select(gta => gta.Name))
	}).Dump();
}

public class AtlasMethodInfo
{
	public string Name;
	public string Parent;
	public string ReturnType;
	public string Types;
}
