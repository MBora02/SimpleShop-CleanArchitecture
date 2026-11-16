using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        // Kaydın oluşturulma tarihi
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Güncellenme tarihi (isteğe bağlı, başta null olabilir)
        public DateTime? UpdatedDate { get; set; }

        // Soft Delete: Veriyi veritabanından kalıcı olarak silmek yerine IsDeleted = true yaparız
        public bool IsDeleted { get; set; } = false;
    }
}
