using System;
using System.Collections.Generic;

namespace NTUBAdministrator.Services.Interface
{
    public interface IResult
    {
        Guid ID { get; }

        bool Success { get; set; }

        string Message { get; set; }

        Exception Exception { get; set; }

        List<IResult> InnerResults { get; }
    }
}
