<Query Kind="Program" />

public delegate void MyEventHandler(string foo);

void Main()
{
	NewStuff stuff = new NewStuff();
}

public abstract class OldStuff
{
	public delegate void CustomEventHandler(string foo);
	public event CustomEventHandler OldStuffEvent;
	public void FireOldEvent()
	{
		OldStuffEvent("Test");
	}
	
}

public class NewStuff : OldStuff
{
	public NewStuff()
	{
		OldStuffEvent += OnFireEvent;
		FireOldEvent();
	}

	public void OnFireEvent(string foo)
	{
		MyEventHandler handler = base.FireOldEvent
		foo.Dump();
	}
}

// Define other methods and classes here
