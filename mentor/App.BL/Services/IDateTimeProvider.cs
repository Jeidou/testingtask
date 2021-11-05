using System;

namespace App.BL.Services
{
    public interface IDateTimeProvider
    {
        DateTime DateTimeNow { get; }
    }
}