namespace LInq_Ex
{
    public class DisTinct
    {
        // Linq 중복제거 
        static void Main(string[] args)
        {
      
            List<Person> list = new List<Person>()
          {
              new Person("John", 24),
              new Person("John", 24),
              new Person("Park", 56),
              new Person("Park", 56),
              new Person("Lily", 23),
              new Person("Lily", 23),
              new Person("Roanldo", 30),
              new Person("Roanldo", 30)
          };

            // 익명타입 사용
          var queryResult = 
              (from A in list select A)
              .Select(A=>new {A.Name, A.age})
              .Distinct();

          foreach (var result in queryResult)
          {
              Console.WriteLine($"{result.Name} / {result.age}");
            }

          Console.WriteLine();

          var methodResult = list.Select(A=>new {A.Name, A.age}).Distinct();

          foreach (var result in methodResult)
          {
              Console.WriteLine($"{result.Name}  / {result.age}");
            }

			//  mms연결 get
			//public List<MmsConnection> GetMmsConnections()
			//{

			//	try
			//	{
			//		using (var db = new MmsContext())
			//		{
			//			if (db.MmsConnection != null)
			//			{
			//				List<MmsConnection> mmsConnections = new List<MmsConnection>();

			//				var mmsClients = db.MmsConnection
			//					.Select(m => new MmsClient(
			//						m.mmsClient.clientName,
			//						m.mmsClient.clientIp,
			//						m.mmsClient.clientPort,
			//						m.mmsClient.status))
			//					.Distinct()
			//					.ToList();

			//				var result =
			//					db.MmsConnection
			//						.Select(m => new MmsConnection(m.serverName, m.serverIp))
			//						.Distinct()
			//						.ToList();

			//				foreach (var rs in result)
			//				{
			//					rs.mmsClients.AddRange(mmsClients);
			//				}

			//				return result;

			//			}
			//			return null;
			//		}
			//	}
			//	catch (SqlException /*e*/)
			//	{
			//		return null;
			//	}
			//}

   //     }

	}
    }

    public class MmsConnection
    {
	    [Key]
	    public int id { get; set; }

	    [Column("server_name")]
	    public string serverName { get; set; }

	    [Column("server_ip")]
	    public string serverIp { get; set; }

	    public MmsClient mmsClient { get; set; }

	    [NotMapped]
	    public List<MmsClient> mmsClients { get; set; } = new List<MmsClient>();

	    public MmsConnection() { }
	    public MmsConnection(string serverName, string serverIp)
	    {
		    this.serverName = serverName;
		    this.serverIp = serverIp;

	    }
	    public MmsConnection(int id, string serverName, string serverIp, MmsClient mmsClient)
	    {
		    this.id = id;
		    this.serverName = serverName;
		    this.serverIp = serverIp;
		    this.mmsClient = mmsClient;
	    }
    }

    //[Owned]
    public class MmsClient
    {
	    [Column("client_name")]
	    public string clientName { get; set; }

	    [Column("client_ip")]
	    public string clientIp { get; set; }

	    [Column("client_port")]
	    public int clientPort { get; set; }

	    [Column("status")]
	    public int status { get; set; }

	    public MmsClient()
	    {

	    }

	    public MmsClient(string clientName, string clientIp, int clientPort, int status)
	    {
		    this.clientName = clientName;
		    this.clientIp = clientIp;
		    this.clientPort = clientPort;
		    this.status = status;
	    }
    }

}