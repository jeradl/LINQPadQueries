<Query Kind="Statements">
  <RuntimeVersion>6.0</RuntimeVersion>
</Query>

var x = Console.ReadLine();
long num = Convert.ToInt64(x);
int i = 1;

while (num > 1)
{
	if (num % 2 == 0)
		num /= 2;
	else
		num = (num * 3) + 1;

	Console.WriteLine($"{i++}: {num}");
}