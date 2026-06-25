using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Domain.Entities
{
    public class ExternalFoodMapping
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public string ExternalId { get; private set; }
        public string ExternalSource { get; private set; }
        public string SearchQuery { get; private set; }
        public DateTime MappedAt { get; private set; }

        private ExternalFoodMapping() { }
    }
}
