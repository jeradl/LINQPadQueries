<Query Kind="Statements">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

int[] q = new int[5];
q = Enumerable.Range(1, 70).OrderBy(e => RandomNumberGenerator.GetInt32(1,71)).Take(5).ToArray();
q.Dump();