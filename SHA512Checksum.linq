<Query Kind="Statements">
  <Namespace>System.Security.Cryptography</Namespace>
  <RuntimeVersion>7.0</RuntimeVersion>
</Query>

byte[] SHA512Checksum(string filePath)
{
	using (SHA512 SHA = SHA512.Create())
	{
		using (FileStream fileStream = File.OpenRead(filePath))
		{
			return SHA512.HashData(fileStream);
		}
	}
}

var filePath = @"D:\Downloads\Microsoft Installers\dotnet-hosting-7.0.5-win.exe";
var hash = SHA512Checksum(filePath);
Convert.ToHexString(hash).Dump();