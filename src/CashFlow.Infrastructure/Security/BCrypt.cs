﻿using CashFlow.Domain.Abstractions.Security.Cryptograpy;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infrastructure.Security;

public class BCrypt : IPasswordEncripter
{
    public string Encrypt(string password)
    {
        string passwordHash = BC.HashPassword(password);

        return passwordHash;
    }
}
