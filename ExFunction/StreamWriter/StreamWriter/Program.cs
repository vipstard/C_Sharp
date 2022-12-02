
namespace StreamWriter
{
    using System.IO;
    internal class Program
    {
        static void Main(string[] args)
        {
            StreamWriter writer;
            writer = File.AppendText("경로"); // 이어쓰기
            //writer = File.CreateText("경로");  덮어쓰기

            writer.WriteLine("content");

            writer.Close();
    }
}