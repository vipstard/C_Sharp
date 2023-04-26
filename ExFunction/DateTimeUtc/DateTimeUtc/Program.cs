namespace DateTimeUtc
{
	internal class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				DateTimeOffset now = DateTimeOffset.UtcNow;
				long unixTimestampTicks = now.Ticks - DateTimeOffset.UnixEpoch.Ticks;
				double unixTimestampMicroseconds = (double)unixTimestampTicks / TimeSpan.TicksPerMillisecond / 1000;


				Console.WriteLine(unixTimestampMicroseconds);
			}
		
		}
	}
}