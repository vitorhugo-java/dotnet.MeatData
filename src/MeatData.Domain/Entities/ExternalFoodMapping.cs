using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Domain.Entities
{
    public class ExternalFoodMapping
    {
        Guid Id { get; set; }
        Guid ProductId { get; set; }
        string ExternalId { get; set; }
        string ExternalSource { get; set; }
        string SearchQuery { get; set; }
        DateTime MappedAt { get; set; }

        private ExternalFoodMapping() { }
    }
}
