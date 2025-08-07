using Microsoft.AspNetCore.Http;

namespace Semanix.Domain.AuditTrail;

public class ErrorLogger
{
    public ErrorLogger()
    {
        
    }
    public ErrorLogger(Exception exdb, HttpContext context)
    {
        ExceptionMsg = exdb.Message;
        ExceptionType = exdb.GetType().Name;
        ExceptionSource = exdb.StackTrace;
        ExceptionUrl = context.Request.Path;
    }
    public string? ExceptionMsg { get; set; }
    public string? ExceptionType{ get; set; }   
    public string? ExceptionSource{ get; set; }  
    public string? ExceptionUrl{ get; set; }
    public DateTime LogDate { get; set; } = DateTime.Now;
}