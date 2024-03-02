<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	SqlConnection conn = new SqlConnection("");
	conn.InfoMessage += new SqlInfoMessageEventHandler(CaptureMessage);
	conn.FireInfoMessageEventOnUserErrors = true;
	conn.Open();
	
	SqlCommand cmd = new SqlCommand("spTestPrint", conn);
	cmd.CommandType = CommandType.StoredProcedure;
	cmd.CommandTimeout = 10000000;
		
	cmd.ExecuteNonQuery();
	
	cmd.Dispose();
	conn.Dispose();
}

// Define other methods and classes here
private static void CaptureMessage(object sender, SqlInfoMessageEventArgs args)
{
	args.Message.Dump();	
}