namespace ServiceController
{
    internal class Program
    {
        static void Main(string[] args)
        {
           ServiceControl control = new ServiceControl();
           control.RestartService();
        }

    }
}