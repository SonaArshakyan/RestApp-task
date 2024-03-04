namespace RestApp.Interfaces;
public class Logger : ILogger
{
    public void Error(Exception exception) => Console.WriteLine(exception.Message);

    public void Info(string message) => Console.WriteLine(message);

    public void Warn(string message) => Console.WriteLine(message);
}
