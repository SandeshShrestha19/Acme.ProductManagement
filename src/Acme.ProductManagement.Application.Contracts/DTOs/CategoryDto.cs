using System;
using Volo.Abp.Application.Dtos;

namespace Acme.ProductManagement.DTOs
{
    public class CategoryDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
