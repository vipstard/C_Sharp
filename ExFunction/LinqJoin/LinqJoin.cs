using System;
using System.Data;
using System.Linq;

namespace JoinFunction
{
    class LinqJoin
    {
        /// <summary>
        /// LINQ 구문 사용하여 LINE, LANE, INDEX_PARA, INOUT, BANK, SLOT, RNK 기준으로 DataTable 조인시킨다. 
        /// dt1 : 현재변경점(VALUE_1, VALUE_2, VALUE_3, 기간, LOT_ID, INDEX_COMMENT), 이전변경점(기간, LOT_ID, INDEX_COMMENT) -> 기간은 한번에 dt1에서 가져옴
        /// dt2 : 이전변경점(VALUE_1, VALUE_2, VALUE_3)
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public DataTable MergeDataTable(DataTable dt1, DataTable dt2)
        {
            DataTable mergeDt = new DataTable();

            #region DataTable 표시할 컬럼 추가
            string[] columns = { "SHOP", "PROD_TYPE", "LINE", "LINE_NM", "LANE", "LANE_NM", "INDEX_PARA",
                                     "INDEX_PARA_NM", "INDEX_INOUT", "INDEX_INOUT_NM", "BANK", "SLOT", "RNK",
                                     "VALUE_1", "VALUE_2", "VALUE_3", "VALUE_4", "VALUE_5", "FROM_TO_TIME", "LOT_ID", "INDEX_COMMENT",
                                     "PRE_VALUE_1", "PRE_VALUE_2", "PRE_VALUE_3", "PRE_VALUE_4", "PRE_VALUE_5", "PRE_FROM_TO_TIME", "PRE_LOT_ID",
                                     "PRE_INDEX_COMMENT" };

            foreach (string column in columns)
            {
                mergeDt.Columns.Add(column);
            }
            #endregion

            var result = from t1 in dt1.AsEnumerable()
                         join t2 in dt2.AsEnumerable()
                         on new
                         {
                             PROD_TYPE = t1.Field<string>("PROD_TYPE"),
                             LINE = t1.Field<string>("LINE"),
                             LANE = t1.Field<string>("LANE"),
                             INDEX_PARA = t1.Field<string>("INDEX_PARA"),
                             INDEX_INOUT = t1.Field<string>("INDEX_INOUT"),
                             BANK = t1.Field<string>("BANK"),
                             SLOT = t1.Field<string>("SLOT"),
                             RNK = t1.Field<string>("RNK")
                         }
                         equals new
                         {
                             PROD_TYPE = t2.Field<string>("PROD_TYPE"),
                             LINE = t2.Field<string>("LINE"),
                             LANE = t2.Field<string>("LANE"),
                             INDEX_PARA = t2.Field<string>("INDEX_PARA"),
                             INDEX_INOUT = t2.Field<string>("INDEX_INOUT"),
                             BANK = t2.Field<string>("BANK"),
                             SLOT = t2.Field<string>("SLOT"),
                             RNK = t2.Field<string>("RNK")
                         }
                         into Group
                         from t2 in Group.DefaultIfEmpty()
                         select new
                         {
                             SHOP = t1.Field<string>("SHOP"),
                             PROD_TYPE = t1.Field<string>("PROD_TYPE"),
                             LINE = t1.Field<string>("LINE"),
                             LINE_NM = t1.Field<string>("LINE_NM"),
                             LANE = t1.Field<string>("LANE"),
                             LANE_NM = t1.Field<string>("LANE_NM"),
                             INDEX_PARA = t1.Field<string>("INDEX_PARA"),
                             INDEX_PARA_NM = t1.Field<string>("INDEX_PARA_NM"),
                             INDEX_INOUT = t1.Field<string>("INDEX_INOUT"),
                             INDEX_INOUT_NM = t1.Field<string>("INDEX_INOUT_NM"),
                             BANK = t1.Field<string>("BANK"),
                             SLOT = t1.Field<string>("SLOT"),
                             RNK = t1.Field<string>("RNK"),
                             VALUE_1 = t1.Field<Decimal>("VALUE_1"),
                             VALUE_2 = t1.Field<Decimal>("VALUE_2"),
                             VALUE_3 = t1.Field<Decimal>("VALUE_3"),
                             VALUE_4 = t1.Field<Decimal>("VALUE_4"),
                             VALUE_5 = t1.Field<Decimal>("VALUE_5"),
                             FROM_TO_TIME = t1.Field<string>("FROM_TO_TIME"),
                             LOT_ID = t1.Field<string>("LOT_ID"),
                             INDEX_COMMENT = t1.Field<string>("INDEX_COMMENT"),
                             // t2가 null일 경우 기본값으로 처리
                             PRE_VALUE_1 = t2 != null ? t2.Field<Decimal>("VALUE_1") : 0,
                             PRE_VALUE_2 = t2 != null ? t2.Field<Decimal>("VALUE_2") : 0,
                             PRE_VALUE_3 = t2 != null ? t2.Field<Decimal>("VALUE_3") : 0,
                             PRE_VALUE_4 = t2 != null ? t2.Field<Decimal>("VALUE_4") : 0,
                             PRE_VALUE_5 = t2 != null ? t2.Field<Decimal>("VALUE_5") : 0,
                             PRE_FROM_TO_TIME = t1.Field<string>("PRE_FROM_TO_TIME"),
                             PRE_LOT_ID = t1.Field<string>("PRE_LOT_ID"),
                             PRE_INDEX_COMMENT = t1.Field<string>("PRE_INDEX_COMMENT")
                         };



            foreach (var row in result)
            {
                mergeDt.Rows.Add(row.SHOP, row.PROD_TYPE, row.LINE, row.LINE_NM, row.LANE, row.LANE_NM,
                                 row.INDEX_PARA, row.INDEX_PARA_NM, row.INDEX_INOUT, row.INDEX_INOUT_NM, row.BANK, row.SLOT, row.RNK,
                                 row.VALUE_1, row.VALUE_2, row.VALUE_3, row.VALUE_4, row.VALUE_5,
                                 row.FROM_TO_TIME, row.LOT_ID, row.INDEX_COMMENT,
                                 row.PRE_VALUE_1, row.PRE_VALUE_2, row.PRE_VALUE_3, row.PRE_VALUE_4, row.PRE_VALUE_5,
                                 row.PRE_FROM_TO_TIME, row.PRE_LOT_ID, row.PRE_INDEX_COMMENT);
            }

            return mergeDt;
        }
    }
}
