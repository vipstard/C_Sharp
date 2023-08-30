using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLITE_SQL.Query
{
	internal class Class1
	{
		public int GetAllMmsEventsCount(ExtendedRequestObject extendedRequestObject, List<Model.device.Device> scadaList)
		{

			try
			{
				int result = 0;

				using (var db = new MmsContext())
				{
					string scadaIpList = string.Join(",", scadaList.Select(scada => $"'{scada.ip}'"));
					string query =
						$@"SELECT *
                             FROM mms_event
                             WHERE (src_Ip NOT IN ({scadaIpList}) AND dst_Ip NOT IN ({scadaIpList}))";

					result = db.MmsEvents.FromSqlRaw(query).Count();
				}

				return result;
			}
			catch (SqlException /*e*/)
			{
				return -1;
			}
		}

		public int GetFilteredMmsEventsCount(ExtendedRequestObject Object, List<Model.device.Device> scadaList)
		{

			try
			{
				//  23.08.23 데이터 대용량으로 늘어나니 LINQ 속도 느려서 SQL로 변경
				using (var db = new MmsContext())
				{

					string scadaIpList = string.Join(",", scadaList.Select(scada => $"'{scada.ip}'"));
					string query = _mmsQuery.BuildQueryMmsCount(Object, scadaIpList);
					int result = db.MmsEvents.FromSqlRaw(query).Count();

					return result;
				}
			}
			catch (SqlException /*e*/)
			{
				return -1;
			}
		}

		public List<MmsEvent> GetMmsEvents(ExtendedRequestObject Object, List<Model.device.Device> scadaList)
		{
			try
			{
				
				using (var db = new MmsContext())
				{

					string scadaIpList = string.Join(",", scadaList.Select(scada => $"'{scada.ip}'"));

					string query = _mmsQuery.BuildQueryMms(Object, scadaIpList);
					List<MmsEvent> result = db.MmsEvents.FromSqlRaw(query).ToList();

					return result;
				}
			}
			catch (SqlException /*e*/)
			{
				return null;
			}
		}
	}
}
