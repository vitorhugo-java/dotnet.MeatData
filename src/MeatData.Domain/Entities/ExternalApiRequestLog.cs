using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Domain.Entities
{
    public class ExternalApiRequestLog
    {
        Guid Id { get; set; }
        Guid ProductId { get; set; }
        string ApiName { get; set; }
        string Endpoint { get; set; }
        int StatusCode { get; set; }
        bool CacheHit { get; set; }
        int DurationMs { get; set; }
        DateTime RequestedAt { get; set; }
    }
}
