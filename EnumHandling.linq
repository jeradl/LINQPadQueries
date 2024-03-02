<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var pattern = new Regex("(?<=[a-z])([A-Z])([a-z])");
	
//	var q = (from name in (Command[])Enum.GetValues(typeof(Command)) select new { Parameter = "/" + pattern.Replace(name.ToString(), "-$1$2").ToLower(), Command = name }).ToDictionary(key => key.Parameter, value => value.Command);
	var q = Enum.GetNames(typeof(Command));
	q.Dump();
}

// Define other methods and classes here
public enum Command
{
	Concurrent,
	ProcessSubscription,
	SyncReports,
	SyncApex,
	SyncApex2,
	SyncApexProgress,
	SyncApexProgress2,
	SaveAILetters
}