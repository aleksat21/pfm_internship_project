﻿namespace PersonalFinanceManagement.API.Models.Exceptions
{
    public class NotFoundException : Exception
    {
        protected NotFoundException(string message)
            : base(message)
        {
        }
    }
}
