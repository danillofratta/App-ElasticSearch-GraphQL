using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.Application.Product.Elastic.EventElastic.Create
{
    public class ProductDeleteEvent : INotification
    {
        public Guid Id { get; set; }

        public ProductDeleteEvent(Guid id)
        {
            Id = id;
        }
    }
}
