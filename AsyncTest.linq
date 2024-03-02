<Query Kind="Program">
  <Output>DataGrids</Output>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	AsyncTask1();
	AsyncTask2();
}

// Define other methods and classes here
async Task AsyncTask1()
{
	Console.WriteLine("1:1");
	await Task.Delay(5000);
	Console.WriteLine("1:2");
}

async Task AsyncTask2()
{
	Console.WriteLine("2:1");
	Task.Delay(5000);
	Console.WriteLine("2:2");
}