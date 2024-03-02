<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Data.dll</Reference>
  <NuGetReference>FastMember</NuGetReference>
  <NuGetReference>NPOI</NuGetReference>
  <Namespace>FastMember</Namespace>
  <Namespace>NPOI.HSSF.UserModel</Namespace>
  <Namespace>NPOI.SS.UserModel</Namespace>
  <Namespace>NPOI.XSSF.UserModel</Namespace>
  <Namespace>System.Configuration</Namespace>
  <Namespace>System.Data.Common</Namespace>
  <Namespace>System.Data.SqlClient</Namespace>
</Query>

var sproc = "";

using (SqlConnection conn = new SqlConnection())
using (SqlCommand cmd = new SqlCommand())
{
	conn.ConnectionString = @"";
	cmd.CommandType = CommandType.StoredProcedure;
	cmd.CommandText = sproc;
	cmd.Connection = conn;

	conn.Open();
	var data = (DbDataReader)cmd.ExecuteReader();
	
	var dts = new List<DataTable>();

	do
	{
		var dt = new DataTable();

		string[] cols = new string[data.FieldCount];

		for (int i = 0; i < data.FieldCount; i++)
		{
			cols[i] = data.GetName(i);
		}

		foreach (var name in cols)
		{
			dt.Columns.Add(name);
		}

		while (data.Read())
		{
			DataRow row = dt.NewRow();
			int i = 0;

			foreach (var name in cols)
			{
				row[name] = data[i];
				i++;
			}

			dt.Rows.Add(row);
		}
		
		dts.Add(dt);
	} while (data.NextResult());
	
	var wbs = new List<IWorkbook>();
	
	foreach (var dt in dts)
	{
		IWorkbook wb = new XSSFWorkbook();		
		ISheet sht = wb.CreateSheet();
		IRow head = sht.CreateRow(0);
		
		for (int i = 0; i < dt.Columns.Count; i++)
		{
			head.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
		}
		
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			IRow row = sht.CreateRow(i + 1);
			
			for (int j = 0; j < dt.Columns.Count; j++)
			{
				row.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
			}
		}
		
		for (int i = 0; i < dt.Columns.Count; i++)
		{
			sht.AutoSizeColumn(i);
		}
		
		wbs.Add(wb);
	}
	
	List<RosterFileExportModel> rom = new List<RosterFileExportModel> 
	{
		new RosterFileExportModel 
		{
			RosterID = 1,
			ExportPath = @"C:\",
			FileName = "group_import",
			FileExtention = "xlsx",
			ExportOrder = 1
		},
		new RosterFileExportModel
		{
			RosterID = 1,
			ExportPath = @"C:\",
			FileName = "student_import",
			FileExtention = "xlsx",
			ExportOrder = 2
		},
		new RosterFileExportModel
		{
			RosterID = 1,
			ExportPath = @"C:\",
			FileName = "user_import",
			FileExtention = "xlsx",
			ExportOrder = 3
		}
	};
	
	var rosteringId = 1;
	var query = "SELECT * FROM RosterFiles WHERE RosterID = @rostId";
	var args = new List<SqlParameter>
	{
		new SqlParameter("rostId", rosteringId)
	};
	
	Func<SqlDataReader, RosterFileExportModel> actionPerRow = (reader) => 
	{
		var export = new RosterFileExportModel
		{
			ExportPath = @"C:\",//reader.GetString(2),
			FileName = reader.GetString(3),
			FileExtention = reader.GetString(4),
			ExportOrder = reader.GetInt32(5)
		};
		
		return export;
	};
	
	var exports = SqlHelper.ExecuteListWithParams(query, actionPerRow, args);
	
	foreach (var export in exports)
	{
		using (var file = new FileStream($@"{export.ExportPath}\{export.FileName}.{export.FileExtention}", FileMode.Create))
		{
			wbs[export.ExportOrder - 1].Write(file);
		}
	}
}