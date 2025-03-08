using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CursoMvc.Models.TableViewModels
{
    public class SpareTableViewModel
    {
        public int Id { get; set; }
        public string Partnumber { get; set; }
        public string Description { get; set; }
        public string Machine {  get; set; }
        public string Lpn { get; set; }
        public int? Quantity { get; set; }
        public byte[] Ilustration { get; set; }

    }
}