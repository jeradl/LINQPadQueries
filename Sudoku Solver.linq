<Query Kind="Statements">
  <NuGetReference>CommunityToolkit.HighPerformance</NuGetReference>
  <Namespace>CommunityToolkit.HighPerformance</Namespace>
  <Namespace>CommunityToolkit.HighPerformance.Buffers</Namespace>
  <Namespace>CommunityToolkit.HighPerformance.Enumerables</Namespace>
  <Namespace>CommunityToolkit.HighPerformance.Helpers</Namespace>
</Query>

sodSquare[,] board = new sodSquare[9,9];


SetupGame();
StartingNumbers();



//cycle through all 81 boxes to generate possible numbers
void CycleBoxes() 
{
	//Span2D<sodSquare> spanBoard = board;
	//spanBoard.GetColumn(0).ToArray().Where(w => w.number > 0).Contains(i)

	for (int row = 0; row < 9; row++)
	{
		for (int col = 0; col < 9; col++)
		{
			
		}
	}
}

int[] GetPossibleNumbers(sodSquare sq)
{
	Span2D<sodSquare> spanBoard = board;
	int[] OneToNine = Enumerable.Range(1,9).ToArray();
	int[] eliminatedNumbers = new int[9];
	
	var x = spanBoard.GetRow(sq.row).ToArray();
	var y = spanBoard.GetColumn(sq.col).ToArray();
	
	spanBoard.Slice(0,0,3,3).ToArray();
	
	return new int[0];
}

//establish game environment, instantiate variables
void SetupGame()
{
	for (int row = 0; row < 9; row++) 
	{
		for (int col = 0; col < 9; col++)
		{
			board[row,col] = new sodSquare
			{
				number = 0,
				possNumbers = new int[9],
				preset = false,
				parentSquare = CalcParentSquare(row, col),
				row = row,
				col = col
			};
		}
	}
}

//numbers to populate the board with - hook to UI in phase 2
void StartingNumbers()
{
	//hard code for now
	board[0,3].number = 6;
	board[0,6].number = 4;	
	
}

int CalcParentSquare(int row, int col)
{
	return (row, col) switch
	{
		(0 or 1 or 2, 0 or 1 or 2) => 1,
		(0 or 1 or 2, 3 or 4 or 5) => 2,
		(0 or 1 or 2, 6 or 7 or 8) => 3,
		(3 or 4 or 5, 0 or 1 or 2) => 4,
		(3 or 4 or 5, 3 or 4 or 5) => 5,
		(3 or 4 or 5, 6 or 7 or 8) => 6,
		(6 or 7 or 8, 0 or 1 or 2) => 7,
		(6 or 7 or 8, 3 or 4 or 5) => 8,
		(6 or 7 or 8, 6 or 7 or 8) => 9,
		_ => 0
	};
}

class sodSquare 
{
	public int number;
	public int[] possNumbers;
	public bool preset;
	public int parentSquare;
	public int row;
	public int col;
}

