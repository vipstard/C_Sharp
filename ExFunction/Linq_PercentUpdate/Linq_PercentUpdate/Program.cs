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
