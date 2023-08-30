using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDAS.IDAL.Query;
using MDAS.Model.datatables;

namespace MDAS.DAL.Query
{
    public class MmsQuery : IMmsQuery
    {
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

        private string GetDateString(ExtendedRequestObject Object, DateTypEnum type)
        {
            string date = "";

            if (type == DateTypEnum.MIN)
            {
                date = Object.startDate == null ? "2000-01-01 00:00:00" : Object.startDate;
            }
            else if (type == DateTypEnum.MAX)
            {
                date = Object.endDate == null ? "2100-01-01 00:00:00" : Object.endDate;
            }

            return date;
        }
        private string GetFilterCondition(ExtendedRequestObject Object, int column, string columnName)
        {
            string filterValues = GetStatus(Object, column);

            if (!string.IsNullOrEmpty(filterValues))
            {
                return $"AND {columnName} IN ({filterValues})";
            }

            return "";
        }

        public string BuildQueryMms(ExtendedRequestObject Object, string scadaIpList)
        {
            // 시간필터
            string minDateString = GetDateString(Object, DateTypEnum.MIN);
            string maxDateString = GetDateString(Object, DateTypEnum.MAX);

            // 정렬
            string orderByColumn = Object.columns[Object.order[0].column].name;
            string orderByDirection = Object.order[0].dir == "desc" ? "DESC" : "";

            // 상태, 비고 필터
            string statusCondition = GetFilterCondition(Object, 5, "status");
            string extraInfoCondition = GetFilterCondition(Object, 6, "extra_info");

            string query =
                $@"SELECT *
                     FROM mms_event
                     WHERE src_Ip NOT IN ({scadaIpList}) AND dst_Ip NOT IN ({scadaIpList})
                        {statusCondition}
                        {extraInfoCondition}
                        AND ( timestamp >= '{minDateString}' AND timestamp <= '{maxDateString}')
                        AND src_Name LIKE '%{Object.columns[1].search.value}%'
                        AND src_Ip LIKE '%{Object.columns[2].search.value}%'
                        AND dst_Name LIKE '%{Object.columns[3].search.value}%'
                        AND dst_Ip LIKE '%{Object.columns[4].search.value}%'
                     ORDER BY  {orderByColumn} {orderByDirection}
                     LIMIT {Object.length} OFFSET {Object.start};";

            return query;
        }
        public string BuildQueryMmsCount(ExtendedRequestObject Object, string scadaIpList)
        {
            string minDateString = GetDateString(Object, DateTypEnum.MIN);
            string maxDateString = GetDateString(Object, DateTypEnum.MAX);
            string orderByColumn = Object.columns[Object.order[0].column].name;
            string orderByDirection = Object.order[0].dir == "desc" ? "DESC" : "";

            string statusCondition = GetFilterCondition(Object, 5, "status");
            string extraInfoCondition = GetFilterCondition(Object, 6, "extra_info");

            string query =
                $@"SELECT *
                     FROM mms_event
                     WHERE src_Ip NOT IN ({scadaIpList}) AND dst_Ip NOT IN ({scadaIpList})
                        {statusCondition}
                        {extraInfoCondition}
                        AND ( timestamp >= '{minDateString}' AND timestamp <= '{maxDateString}')
                        AND src_Name LIKE '%{Object.columns[1].search.value}%'
                        AND src_Ip LIKE '%{Object.columns[2].search.value}%'
                        AND dst_Name LIKE '%{Object.columns[3].search.value}%'
                        AND dst_Ip LIKE '%{Object.columns[4].search.value}%'";

            return query;
        }

        public string BuildQueryScadaMms(ExtendedRequestObject Object, string scadaIpList)
        {
            string minDateString = GetDateString(Object, DateTypEnum.MIN);
            string maxDateString = GetDateString(Object, DateTypEnum.MAX);
            string orderByColumn = Object.columns[Object.order[0].column].name;
            string orderByDirection = Object.order[0].dir == "desc" ? "DESC" : "";

            string statusCondition = GetFilterCondition(Object, 5, "status");
            string extraInfoCondition = GetFilterCondition(Object, 6, "extra_info");

            string query =
                $@"SELECT *
                     FROM mms_event
                     WHERE (src_Ip IN ({scadaIpList}) OR dst_Ip IN ({scadaIpList}))
                        {statusCondition}
                        {extraInfoCondition}
                        AND ( timestamp >= '{minDateString}' AND timestamp <= '{maxDateString}')
                        AND src_Name LIKE '%{Object.columns[1].search.value}%'
                        AND src_Ip LIKE '%{Object.columns[2].search.value}%'
                        AND dst_Name LIKE '%{Object.columns[3].search.value}%'
                        AND dst_Ip LIKE '%{Object.columns[4].search.value}%'
                     ORDER BY  {orderByColumn} {orderByDirection}
                     LIMIT {Object.length} OFFSET {Object.start};";

            return query;
        }
        public string BuildQueryScadaMmsCount(ExtendedRequestObject Object, string scadaIpList)
        {
            string minDateString = GetDateString(Object, DateTypEnum.MIN);
            string maxDateString = GetDateString(Object, DateTypEnum.MAX);
            string orderByColumn = Object.columns[Object.order[0].column].name;
            string orderByDirection = Object.order[0].dir == "desc" ? "DESC" : "";

            string statusCondition = GetFilterCondition(Object, 5, "status");
            string extraInfoCondition = GetFilterCondition(Object, 6, "extra_info");

            string query =
                $@"SELECT *
                     FROM mms_event
                     WHERE (src_Ip IN ({scadaIpList}) OR dst_Ip IN ({scadaIpList}))
                        {statusCondition}
                        {extraInfoCondition}
                        AND ( timestamp >= '{minDateString}' AND timestamp <= '{maxDateString}')
                        AND src_Name LIKE '%{Object.columns[1].search.value}%'
                        AND src_Ip LIKE '%{Object.columns[2].search.value}%'
                        AND dst_Name LIKE '%{Object.columns[3].search.value}%'
                        AND dst_Ip LIKE '%{Object.columns[4].search.value}%'";

            return query;
        }



        public enum DateTypEnum
        {
            MIN,
            MAX
        }

    }
}
