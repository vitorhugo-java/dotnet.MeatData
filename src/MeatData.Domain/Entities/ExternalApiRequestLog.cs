using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Domain.Entities
{
    public class ExternalApiRequestLog
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public string ApiName { get; private set; }
        public string Endpoint { get; private set; }
        public int StatusCode { get; private set; }
        public bool CacheHit { get; private set; }
        public int DurationMs { get; private set; }
        public DateTime RequestedAt { get; private set; }
    }
}
