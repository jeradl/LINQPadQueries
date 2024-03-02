<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Options</NuGetReference>
  <NuGetReference>MongoDB.Driver</NuGetReference>
  <Namespace>Microsoft.Extensions.Options</Namespace>
  <Namespace>MongoDB.Bson</Namespace>
  <Namespace>MongoDB.Bson.Serialization.Attributes</Namespace>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var save = new Save();
	save.Post();
}

public class Game
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string id { get; set; }
	public int[] NumbersDrawn { get; set; }
	public int MegaDrawn { get; set; }
}

public class Settings
{
	public string ConnectionString { get; set; } = "mongodb://localhost:27017";
	public string DatabaseName { get; set; } = "LotterySimTest";
	public string GameCollectionName { get; set; } = "Game";
}

public class GameService
{
	private readonly IMongoCollection<Game> _gamesCollection;
	
	public GameService(Settings settings)
	{
		var mongoClient = new MongoClient(settings.ConnectionString);
		var mongoDatabase = mongoClient.GetDatabase(settings.DatabaseName);
		
		_gamesCollection = mongoDatabase.GetCollection<Game>(settings.GameCollectionName);
	}
	
	public async Task CreateAsync(Game newGame) =>
		await _gamesCollection.InsertOneAsync(newGame);
}

public class Save
{
	public async void Post()
	{
		var _settings = new Settings();
		var _service = new GameService(_settings);
		var game = new Game
		{
			NumbersDrawn = new int[5] { 1, 2, 3, 4, 5},
			MegaDrawn = 42
		};
		
		await _service.CreateAsync(game);
	}
}
