using System;
using System.Collections.Generic;
using System.IO;

namespace EAI_Auto_Script
{
    public class Auto_Script
    {
        DbManager dbManager = new DbManager();

        public Auto_Script()
        {
            Console.Write("테이블을 입력하세요 : ");
        }

        public void Sk()
        {
            string table;
            string scripts;

            List<string> inputFields = new List<string>(); // 줄별로 입력된 값들을 저장할 리스트

            table = Console.ReadLine();

            if (string.IsNullOrEmpty(table))
            {
                Console.WriteLine("테이블을 입력하셔야 합니다.");
                return;
            }

            inputFields = dbManager.Oracle_DB_Select(table);

            if (inputFields.Count > 0)
            {
                scripts = string.Empty;
                scripts += ScriptEditor1(inputFields, table);
                scripts += ScriptEditor2(inputFields, table);

                SaveScriptToFile(scripts, table);

            }



            // 입력된 줄들을 결과로 출력
            //ResultSk(inputFields);
        }

        /// <summary>
        /// RDB Reader Script
        /// </summary>
        /// <param name="inputFields"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private string ScriptEditor1(List<string> inputFields, string table)
        {

            string temp;
            int idx = 0;
            int whiteSpaceLength = 0;

            string script1 = "var SQL = \"\";\n\n"; // 시작부분
            string startScript1 = $"SQL = SQL + \" SELECT {inputFields[0].PadRight(30)}\\n\";\n";
            int indexStartLineLength = startScript1.IndexOf('\\'); // \n의 인덱스 찾기

            script1 += startScript1;

            // 나머지 필드들을 ,로 시작하게 하고, 위치를 맞추기 위해 공백을 추가
            for (int i = 1; i < inputFields.Count; i++)
            {

                temp = $"SQL = SQL + \"{new string(' ', 5)} , {inputFields[i]}";
                idx = temp.Length;
                whiteSpaceLength = indexStartLineLength - idx;
                temp += $"{new string(' ', whiteSpaceLength)}\\n\";\n";
                script1 += temp;

            }

            // FROM 절과 이후의 구문 추가
            temp = $"SQL = SQL + \" FROM {table}";
            temp += $"{new string(' ', indexStartLineLength - temp.Length)}\\n\";\n";
            script1 += temp;

            temp = "SQL = SQL + \" WHERE IF_CODE = 'N'";
            temp += $"{new string(' ', indexStartLineLength - temp.Length)}\\n\";\n";
            script1 += temp;

            temp = "SQL = SQL + \" ORDER BY IF_SEQ";
            temp += $"{new string(' ', indexStartLineLength - temp.Length)}\\n\";\n\n";
            script1 += temp;

            script1 += "return SQL; \n\n\n";

            return script1;
        }

        /// <summary>
        /// Script Runner
        /// </summary>
        /// <param name="inputFields"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private string ScriptEditor2(List<string> inputFields, string table)
        {
            string temp;
            string script = string.Empty;

            // 시작부분
            string startScript1 = $"var {inputFields[0].PadRight(25)}= getData(\"/{table}/{inputFields[0]}\");\n";
            int indexStartLineLength = startScript1.IndexOf('='); // \n의 인덱스 찾기

            script += startScript1;

            //ex) var IF_SEQ = getData("/table/IF_SEQ");
            for (int i = 1; i < inputFields.Count; i++)
            {
                temp = $"var {inputFields[i]}";
                temp += $"{new string(' ', indexStartLineLength - temp.Length - 1)} = getData(\"/{table}/{inputFields[i]}\"); \n";
                script += temp;
            }
            script += "\n\n";

            //ex) setData("SK.v.CHECK_DATE", CHECK_DATE)
            for (int i = 1; i < inputFields.Count; i++)
            {

                script += $"setData(\"SK.v.{inputFields[i]}\", {inputFields[i]}); \n";
            }
            return script;
        }




        /// <summary>
        /// 파일저장 : 경로 실행 PC Download 폴더
        /// </summary>
        /// <param name="script"></param>
        /// <param name="fileName"></param>
        public void SaveScriptToFile(string script, string fileName)
        {
            fileName += ".txt";

            // 사용자의 Download 폴더 경로
            string downloadFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // 파일 경로 생성
            string filePath = Path.Combine(downloadFolderPath, fileName);

            try
            {
                // 파일에 내용을 저장
                File.WriteAllText(filePath, script);
                Console.WriteLine($"Script saved to {filePath}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing to file: " + e.Message);
            }
            finally
            {
            }
        }


        /// <summary>
        /// 테스트 출력용
        /// </summary>
        /// <param name="inputFields"></param>
        private void ResultSk(List<string> inputFields)
        {
            Console.WriteLine("입력한 필드들:");

            // 저장된 모든 필드(줄)들을 순차적으로 출력
            foreach (string field in inputFields)
            {
                Console.WriteLine(field);
            }
        }
    }
}
