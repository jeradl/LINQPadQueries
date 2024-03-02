<Query Kind="Statements" />

var date = new DateOnly(2024, 02, 29);
GetMoonPhase(date).Dump();

string GetMoonPhase(DateOnly date)
{
	//Express the date as Y = year, M = Month, D = day
	int Y = date.Year, M = date.Month, D = date.Day;

	//Julian date 1/1/2014 at 11:14:00 (new moon)
	var SM = 2456658.96806;

	//If the month is January or February, subtract 1 from the year and add 12 to the month.
	if (M == 1 || M == 2)
	{
		Y -= 1;
		M += 12;
	}

	//A = Y/100
	double A = Y / 100;

	//B = A/4
	double B = A / 4;

	//C = 2 - A + B
	double C = 2 - A + B;

	//E = 365.25 * (Y + 4716)
	double E = 365.25 * (Y + 4716);

	//F = 30.6001 * (M + 1)
	double F = 30.6001 * (M + 1);
	
	//Current Julian day
	//JD = C + D + E + F - 1524.5
	var JD = C + D + E + F - 1524.5;

	//Number of days since new moon on 1/1/2014
	var DSN = JD - SM;
	
	//Number of new moons since 1/1/2014
	var NM = DSN / 29.53;

	//Drop whole number. Fraction represents current cycle of moon
	var X = (NM - Math.Truncate(NM));
	
	//Get number of days we are into current cycle
	var Q = (int)(X * 29.53);

	Phases phase = Q switch
	{
		int i when i == 0 => Phases.New,
		int i when i >= 1 && i <= 3 => Phases.WaxingCrescentFirstHalf,
		int i when i >= 4 && i <= 6 => Phases.WaxingCrescentSecondHalf,
		int i when i == 7 => Phases.FirstQuarter,
		int i when i >= 8 && i <= 10 => Phases.WaxingGibbousFirstHalf,
		int i when i >= 11 && i <= 14 => Phases.WaxingGibbousSecondHalf,
		int i when i == 15 => Phases.Full,
		int i when i >= 16 && i <= 18 => Phases.WaningGibbousFirstHalf,
		int i when i >= 19 && i <= 21 => Phases.WaningGibbousSecondHalf,
		int i when i == 22 => Phases.LastQuarter,
		int i when i >= 23 && i <= 26 => Phases.WaningCrescentFirstHalf,
		int i when i >= 27 && i <= 30 => Phases.WaningCrescentSecondHalf,
		_ => Phases.Unknown
	};

	return phase.ToString();	
}

enum Phases
{
	Unknown,
	New,
	WaxingCrescentFirstHalf,
	WaxingCrescentSecondHalf,
	FirstQuarter,
	WaxingGibbousFirstHalf,
	WaxingGibbousSecondHalf,
	Full,
	WaningGibbousFirstHalf,
	WaningGibbousSecondHalf,
	LastQuarter,
	WaningCrescentFirstHalf,
	WaningCrescentSecondHalf
}