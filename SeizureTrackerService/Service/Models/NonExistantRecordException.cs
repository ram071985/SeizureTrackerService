namespace SeizureTrackerService.Service.Models;

public class NonExistantRecordException : Exception
{
    public NonExistantRecordException() { }

    public NonExistantRecordException(string message) 
        : base(message) { }

    public NonExistantRecordException(string message, Exception inner) 
        : base(message, inner) { }
}