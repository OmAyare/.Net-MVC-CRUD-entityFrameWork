using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRUD_entityFrameWork.Models
{
    public class DepartmentTotals
    {
        [Key]
        public int DepartmentId { get; set; } // Assuming an integer primary key
        public string Name { get; set; }
        public int Totals { get; set; }
    }

}