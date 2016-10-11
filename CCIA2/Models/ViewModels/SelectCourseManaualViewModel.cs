using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCIA2.Models.ViewModels
{
   public class SelectCourseManaualViewModel
   {
      public List<Course> CoursList { get; set; }
      public List<int> AlreadySelected { get; set; }
      public int mrSqno { get; set; }      
   }
}