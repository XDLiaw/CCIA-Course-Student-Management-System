using CCIA2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCIA2.Helper
{
    public class DropDownListHelper
    {
        private static CCIAContext db = new CCIAContext();

        public static List<SelectListItem> getApplyStepListWithAll()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "全部",
                Value = ""
            });

            var items = 
            (
                from tas in db.TableApplyStep 
                select new SelectListItem 
                {
                    Text = tas.name,
                    Value = tas.applystep.ToString()
                }
            );
            list.AddRange(items);

            return list;
        }




        
    }
}