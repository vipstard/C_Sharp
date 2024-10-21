using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace EAI_Auto_Script
{
    public class DbManager
    {
        private string dbTNS = string.Empty;

        OracleConnection DbConnection;
        OracleCommand DBcommand;

        public DbManager()
        {
            SetDbTns();
        }

        private void SetDbTns()
        {
            try
            {
                dbTNS = "";
                Console.WriteLine($"TNS : {dbTNS}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Oracle DB SELECT with a complex SQL query and parameters
        /// </summary>
        public List<string> Oracle_DB_Select(string tableName)
        {
            List<string> fieldList = new List<string>();

            using (DbConnection = new OracleConnection(dbTNS))
            {
                try
                {
                    DbConnection.Open();

                    // 테이블의 모든 필드명을 가져오는 쿼리 (스키마를 지정할 경우, schemaName.tableName 형식으로 쿼리를 작성)
                    string query = $"SELECT column_name FROM user_tab_columns WHERE table_name = '{tableName.ToUpper()}'";

                    DBcommand = new OracleCommand(query, DbConnection);

                    using (OracleDataReader reader = DBcommand.ExecuteReader())
                    {
                        // 결과가 없을 경우를 처리하기 위해
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("해당 테이블에서 필드명을 찾을 수 없습니다.");
                            return fieldList; // 빈 리스트 반환
                        }

                        while (reader.Read())
                        {
                            // 필드명 추가
                            fieldList.Add(reader.GetString(0));
                        }
                    }

                    return fieldList;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
                finally
                {
                    DbConnection.Close();
                }
            }
        }

    }
}
