<Query Kind="Statements">
  <Output>DataGrids</Output>
</Query>

string[] potentialFiles = Directory.GetFiles(@"");
List<string> files = new List<string>();
foreach (var file in potentialFiles)
{
	string text = File.ReadAllText(file);
	if (text.IndexOf("DATETIME2") == -1 && (text.IndexOf("datetime") > -1 || text.IndexOf("DATETIME") > -1 || text.IndexOf("DateTime") > -1))
		files.Add(file);
}

foreach (var file in files)
{
	string text = File.ReadAllText(file, Encoding.UTF8);
	text = text.Replace("datetime", "datetime2");
	text = text.Replace("DATETIME", "DATETIME2");
	text = text.Replace("DateTime", "DateTime2");
	File.WriteAllText(file, text, Encoding.UTF8);
}