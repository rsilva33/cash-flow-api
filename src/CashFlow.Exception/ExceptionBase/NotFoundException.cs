﻿namespace CashFlow.Exception.ExceptionBase;

public class NotFoundException : CashFlowException
{
    public NotFoundException(string message)
        : base(message) { }

    public override int StatusCode => 
        (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors() =>
        [Message];
        //new List<string>() { Message };
}
