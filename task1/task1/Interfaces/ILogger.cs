namespace RestApp.Interfaces;
public interface ILogger
{
    void Error(Exception exception);
    void Info(string message);
    void Warn(string message);
}
