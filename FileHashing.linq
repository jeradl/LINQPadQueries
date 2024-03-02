<Query Kind="Program">
  <Output>DataGrids</Output>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
	List<Image> images = new List<Image>();
	
	string[] paths = {@"C:\Users\1067119\Pictures\Test Student Pics\682434.1.png", 
				 	  @"C:\Users\1067119\Pictures\Test Student Pics\682434.2.png",
				 	  @"C:\Users\1067119\Pictures\Test Student Pics\682434.3.png"};

	foreach (var path in paths)
	{
		var image = new Image();
		
		image.path = path;
		image.bits = File.ReadAllBytes(path);
		
		SHA1 sha = new SHA1CryptoServiceProvider();
		//sha.ComputeHash(image.bits).Length.Dump();
		image.hash = sha.ComputeHash(image.bits);
		image.base64 = Convert.ToBase64String(image.hash);
		images.Add(image);
		sha.Dispose();
	}
				  
	images.Select(s => s).Dump();				  	
}

// Define other methods and classes here
public class Image
{
	public string path;
	public string base64;
	public byte[] bits;
	public byte[] hash;
}