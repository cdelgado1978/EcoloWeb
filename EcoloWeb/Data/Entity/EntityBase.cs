using System;
using System.Linq;
using System.Threading.Tasks;

namespace EcoloWeb.Data.Entity
{
    public abstract class EntityBase : IEntity
    {

        public int Id { get; set; }

        public bool Inactive { get; set; }

        public DateTime CreateDate { get; set; }

        public Guid CreateBy { get; set; }

        public DateTime LastModificationDate { get; set; }

        public Guid ModifiedBy { get; set; }

    }


}
