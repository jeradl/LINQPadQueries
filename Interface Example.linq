<Query Kind="Program" />

public interface IFoo
{
	int CheckId();
}

public class A : IFoo
{
	private int _id;
	public virtual int CheckId()
	{
		_id = 12;	
		return _id;
	}
}

public class B : A
{
	private int _id;
	public override int CheckId()
	{ 
		_id = 24;		
		return _id;
	}
}

public class Test
{
	public int GetThis(A num)
	{
		return num.CheckId();
	}
}

public static void Main()
{
	Test t = new Test();
	t.GetThis(new B()).Dump();
}