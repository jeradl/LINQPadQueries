<Query Kind="Statements" />

var q = "f4,ff,ff,ff,00,00,00,00,00,00,00,00,00,00,00,00,90,01,00,00,00,00,00,01,00,00,00,00,53,00,65,00,67,00,6f,00,65,00,20,00,55,00,49,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00".Replace(",","");

var bytes = new byte[q.Length / 2];
for (var i = 0; i < bytes.Length; i++) 
{
	bytes[i] = Convert.ToByte(q.Substring(i * 2, 2), 16);	
}

Encoding.Unicode.GetString(bytes).Dump();