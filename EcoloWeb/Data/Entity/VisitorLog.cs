using EcoloWeb.Data.Entity.Identity;
using System;

namespace EcoloWeb.Data.Entity
{
    public class VisitorLog: IEntity
    {
        public int Id { get; set; }

        public DateTime VisitDate { get; set; }

        public Topic Topic { get; set; }

        public ApplicationUser userId { get; set; }


    }

}
