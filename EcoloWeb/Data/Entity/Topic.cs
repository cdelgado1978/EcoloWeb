using System.Collections.Generic;

namespace EcoloWeb.Data.Entity
{
    public class Topic : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }
        
        public virtual ICollection<Video> Videos { get; set; } = new List<Video>();

               
    }

}
