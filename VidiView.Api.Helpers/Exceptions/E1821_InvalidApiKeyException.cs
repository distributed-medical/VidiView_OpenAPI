﻿namespace VidiView.Api.Exceptions;

public class E1821_InvalidApiKeyException : VidiViewException
{
    public E1821_InvalidApiKeyException(string message)
        : base(message)
    {
    }
}