namespace Semanix.Application.Utilities;

public static class Constants
{
    public const string ExceptionCommonWords = "unexpected characted,unexpected end,object reference,exception occured,sqlexception,ioexception,runtimeexception,index out of bound, index out of range, sql server";

    public const string DefaultExceptionFriendlyMessage = "Unable to process your request at the moment, please try again later!";
    public const string SuccessMessage = "Operation performed successfully";
    public const string FailedMessage = "Operation cannot be completed";
    public const string DuplicateMessage = "Duplicate record found";
    public const string NotFoundMessage = "Record not found";
    public const string InvalidMessage = "Invalid operation, kindly check submitted data and try again";
    public const string NoRecords = "No records available";

    public const string SuccessCode = "00";
    public const string FailureCode = "99";
}