namespace dpa.Library.Services;


//处理异常类

public interface IAlertService
{
    Task AlertAsync(string title, string message);
}