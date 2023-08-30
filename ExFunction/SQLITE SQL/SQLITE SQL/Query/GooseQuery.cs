using MDAS.DAL.Datatables;
using MDAS.IDAL.Query;
using MDAS.Model.datatables;
using MDAS.Model.goose;
using Microsoft.EntityFrameworkCore;
using static MDAS.DAL.Query.MmsQuery;

namespace MDAS.DAL.Query
{
	public class GooseQuery : IGooseQuery
	{
		public string BuildQueryGoose(ExtendedRequestObject Object)
		{
			// 시간필터
			long minDateString = GetDateString(Object, DateTypEnum.MIN);
			long maxDateString = GetDateString(Object, DateTypEnum.MAX);

			// 정렬
			string orderByColumn = Object.columns[Object.order[0].column].name;
			string orderByDirection = Object.order[0].dir == "desc" ? "DESC" : "";

			// 상태
			string statusCondition = GetFilterCondition(Object, 1, "status");

			using (var db = new DataTableContext())
			{
				string orderBySecondary = "";

				if (orderByColumn == "recv_sec")
				{
					orderBySecondary = ", recv_msec " + orderByDirection;
				}

				string query = 
					$@"SELECT * 
               FROM GOOSE_EVENT
			   WHERE 
					( recv_sec >= '{minDateString}' AND recv_sec <= '{maxDateString}')
					AND address LIKE '%{Object.columns[2].search.value}%'
                    AND timestamp LIKE '%{Object.columns[3].search.value}%'
                    AND st_num LIKE '%{Object.columns[4].search.value}%'
                    AND ied_name LIKE '%{Object.columns[5].search.value}%'
					{statusCondition}
               ORDER BY {orderByColumn} {orderByDirection} {orderBySecondary}
               LIMIT {Object.length} OFFSET {Object.start}";

				return query;
			}
		}

		public string BuildQueryGooseCount(ExtendedRequestObject Object)
		{
			// 시간필터
			long minDateString = GetDateString(Object, DateTypEnum.MIN);
			long maxDateString = GetDateString(Object, DateTypEnum.MAX);

			// 상태
			string statusCondition = GetFilterCondition(Object, 1, "status");

			using (var db = new DataTableContext())
			{

				string query =
					$@"SELECT * 
               FROM GOOSE_EVENT
			   WHERE 
					( recv_sec >= '{minDateString}' AND recv_sec <= '{maxDateString}')
					AND address LIKE '%{Object.columns[2].search.value}%'
                    AND timestamp LIKE '%{Object.columns[3].search.value}%'
                    AND st_num LIKE '%{Object.columns[4].search.value}%'
                    AND ied_name LIKE '%{Object.columns[5].search.value}%'
					{statusCondition}";

				return query;
			}
		}

		/// <summary>
		/// 비고 필터링
		/// </summary>
		/// <param name="Object"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		private string GetStatus(ExtendedRequestObject Object, int column)
		{
			string extraInfo = "";

			if (Object.columns[column].search.values == null && Object.columns[column].search.value != "")
			{
				extraInfo = Object.columns[column].search.value;
			}
			else if (Object.columns[column].search.values != null)
			{
				extraInfo = string.Join(",", Object.columns[column].search.values);
			}

			return extraInfo;
		}

		/// <summary>
		/// 시간필터
		/// </summary>
		/// <param name="Object"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private long GetDateString(ExtendedRequestObject Object, DateTypEnum type)
		{
			long date = 0;

			if (type == DateTypEnum.MIN)
			{
				if (Object.startDate != null)
				{
					date = GetUnixTimestamp(Object.startDate);
				}
				else date = 946684800; //2000-01-01 00:00:00
				
			}
			else if (type == DateTypEnum.MAX)
			{
				// 시간대 정보를 한국 표준시(KST)로 가정하여 UTC로 변환
				if (Object.endDate != null)
				{
					date = GetUnixTimestamp(Object.endDate);
				}
				else date = 4102444800; // 2100-01-01 00:00:00
			}

			return date;
		}

		private long GetUnixTimestamp(string date)
		{
			DateTime dateTime = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm", null);
			dateTime = dateTime.AddHours(-9);
			DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan elapsedTime = dateTime - unixEpoch;
			return  (long)elapsedTime.TotalSeconds;
		}


		/// <summary>
		/// 상태, 비고 필터
		/// </summary>
		/// <param name="Object"></param>
		/// <param name="column"></param>
		/// <param name="columnName"></param>
		/// <returns></returns>
		private string GetFilterCondition(ExtendedRequestObject Object, int column, string columnName)
		{
			string filterValues = GetStatus(Object, column);

			if (!string.IsNullOrEmpty(filterValues))
			{
				return $"AND {columnName} IN ({filterValues})";
			}

			return "";
		}
	}
}


