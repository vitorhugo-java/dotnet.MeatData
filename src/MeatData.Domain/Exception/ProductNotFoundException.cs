using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Domain.Exceptions
{
    public sealed class ProductNotFoundException : DomainException
    {
        public ProductNotFoundException(Guid id) : base($"Produto com ID '{id}' não encontrado.") { }
    }
}
