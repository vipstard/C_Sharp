namespace DateTimeUtc
{
	internal class Program
	{
		static void Main(string[] args)
		{
			
				DateTimeOffset now = DateTimeOffset.UtcNow.AddHours(9);
				long unixTimestampTicks = now.Ticks - DateTimeOffset.UnixEpoch.Ticks;
				double unixTimestampMicroseconds = (double)unixTimestampTicks / TimeSpan.TicksPerMillisecond / 1000;

				var timstamp = unixTimestampMicroseconds.ToString().Split(".");

				Console.WriteLine(unixTimestampMicroseconds);
				Console.WriteLine($"{timstamp[0]}-{timstamp[1]}");

				Console.WriteLine(now.AddMinutes(1));
		
		}
	}
}