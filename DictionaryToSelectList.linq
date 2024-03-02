<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Reference>S:\Projects\PortfolioSearcher\PortfolioSearcher\bin\System.Web.Mvc.dll</Reference>
  <Namespace>System.Web.Mvc</Namespace>
</Query>

Dictionary<int,string> dic = new Dictionary<int,string>
{
	{1, "Abc"},
	{2, "Def"},
	{3, "Ghi"}
};
dic.Select(d => new SelectList(dic, "Key", "Value")).Dump();