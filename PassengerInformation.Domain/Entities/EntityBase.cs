using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerInformation.Domain.Entities
{
    public class EntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool IsActive { get; set; } = true;
        protected EntityBase()
        {
        }
    }
}

