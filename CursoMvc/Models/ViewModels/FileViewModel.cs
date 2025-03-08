using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CursoMvc.Models.ViewModels
{
    public class FileViewModel
    {
        [DisplayName("Archivos")]
        public HttpPostedFileBase File { get; set; }

        public int IdLocation { get; set; }

        public string Location { get; set; }
        public int IdMachines { get; set; }


        public string MachineName { get; set; }

        [Required]
        public string Partnumber { get; set; }
        [Required]
        public string Description { get; set; }

        
        public HttpPostedFileBase SpareImage { get; set; }

        [Required]
        public int Quantity { get; set; }

    }

}