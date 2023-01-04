//try
//{
//    using (var db = new PurgeDbContext())
//    {
//        if (db.MmsEvents != null)
//        {
//            List<MmsEvent> mmsEventTemp = db.MmsEvents.ToList();

//            // 원하는 비율로 DB에 먼저 입력된 데이터순으로 삭제 (rownum 순)
//            List<MmsEvent> result = mmsEventTemp
//                .Where((item, Index) =>
//                    (Index == 0) || (Index * percent / 100) > ((Index - 1) * percent / 100))
//                .ToList();

//            db.MmsEvents.RemoveRange(result);
//            db.SaveChanges();

//        }
//    }
//}

//catch (SqlException /*e*/)
//{
//}


// SQLITE 사용 Nuget - System.Data.SQLite(1.0.117)
//public void deleteLastRecordsGooseEvent(int percent)
//{
//    try
//    {
//        using (SQLiteConnection sqlConnection = new SQLiteConnection(@"Data Source=.."))
//        {
//            string sql = $"DELETE From goose_event  WHERE rowid In (Select rowid from goose_event Limit (Select Round(COUNT(*) * {percent} / 100) from goose_event))";

//            using (SQLiteCommand sqlCmd = new SQLiteCommand(sql, sqlConnection))
//            {
//                sqlCmd.Connection = sqlConnection;
//                sqlConnection.Open();
//                sqlCmd.ExecuteNonQuery();
//            }

//        }
//    }

//    catch (SqlException /*e*/)
//    {
//    }
//}

