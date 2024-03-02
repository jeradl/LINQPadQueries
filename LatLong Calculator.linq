<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var currentLat = 36.733861; //your current latitude
	var currentLong = -119.789820; //your current longitude
	var bearing = 274; //degrees - bearing to travel along
	var distance = 5000; //meters to destination
	
	GetPoint(currentLat, currentLong, distance, bearing);
}

// Define other methods and classes here
double ToRadians(double dec)
{
	return dec / (180 / Math.PI);
}

double ToDec(double rads)
{
	return rads * (180 / Math.PI);
}

void GetPoint(double lat, double lon, double distance, double bearing)
{
	double radBearing = ToRadians(bearing);
	double radLat = ToRadians(lat);
	double radLong = ToRadians(lon);
	int earthRadius = 6371000;
	double distFrac = distance / earthRadius;
	
	double latResult = Math.Asin(Math.Sin(radLat) * Math.Cos(distFrac) + Math.Cos(radLat) * Math.Sin(distFrac) * Math.Cos(radBearing));
	double a = Math.Atan2(Math.Sin(radBearing) * Math.Sin(distFrac) * Math.Cos(radLat), Math.Cos(distFrac) - Math.Sin(radLat) * Math.Sin(latResult));
	double lonResult = (radLong + a + 3 * Math.PI) % (2 * Math.PI) - Math.PI;

	Console.WriteLine($"Latitude: {ToDec(latResult)}, Longitude: {ToDec(lonResult)}");
}