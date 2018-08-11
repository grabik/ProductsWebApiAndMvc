using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCApp.Models
{
    public class MVCProductsModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage="This Field is Required")]
        public string Name { get; set; }
        public string Short_Description { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        //public virtual Categories Categories { get; set; }
    }
}