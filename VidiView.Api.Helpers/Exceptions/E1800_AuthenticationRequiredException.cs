﻿namespace VidiView.Api.Exceptions;

public class E1800_AuthenticationRequiredException : VidiViewException
{
    public E1800_AuthenticationRequiredException(string message)
        : base(message)
    {
    }
}