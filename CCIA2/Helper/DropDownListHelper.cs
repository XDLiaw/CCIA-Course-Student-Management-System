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
            SysUser user = HttpContext.Current.Session[SessionKey.USER] as SysUser;
            if (user.role == 1 || user.role == 3)
            {
                list.Add(new SelectListItem()
                {
                    Text = "全部",
                    Value = ""
                });
            }

            var items = 
            (
                from tas in db.TableApplyStep 
                where user.role == 1 ? tas.role1Auth : true
                where user.role == 2 ? tas.role2Auth : true
                where user.role == 3 ? tas.role3Auth : true
                select new SelectListItem 
                {
                    Text = tas.name,
                    Value = tas.applystep.ToString()
                }
            );
            list.AddRange(items);

            return list;
        }

        public static List<SelectListItem> getAppraiseResultList(int AppraiseStep)
        {
            //TODO 可能要取消掉這個method
            List<SelectListItem> list = new List<SelectListItem>();
            if (AppraiseStep == 1) //資格審
            {
                list.Add(new SelectListItem()
                {
                    Text = "通過",
                    Value = "1"
                });
                list.Add(new SelectListItem()
                {
                    Text = "未通過",
                    Value = "0"
                });
            }
            else if (AppraiseStep == 2) //外評初審
            {
                // nothing;
            }


            else if (AppraiseStep == 3)
            {
                list.Add(new SelectListItem()
                {
                    Text = "正取",
                    Value = "1"
                });
                list.Add(new SelectListItem()
                {
                    Text = "備取",
                    Value = "2"
                });
                list.Add(new SelectListItem()
                {
                    Text = "未錄取",
                    Value = "0"
                });
            }
            else if (AppraiseStep == 4)
            {
                list.Add(new SelectListItem()
                {
                    Text = "已繳",
                    Value = "1"
                });
                list.Add(new SelectListItem()
                {
                    Text = "未繳",
                    Value = "0"
                });
            }

            return list;
        }

        public static List<SelectListItem> getAppraiseGroupNameList(bool withSelectAllOption)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (withSelectAllOption)
            {
                list.Add(new SelectListItem()
                {
                    Text = "全部",
                    Value = ""
                });
            }
            var items =
            (
                from tg in db.TableGroup
                select new SelectListItem
                {
                    Text = tg.GroupName,
                    Value = tg.GroupName
                }
            );
            list.AddRange(items);

            return list;
        }

        public static List<SelectListItem> getEnrollTypeList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "全部",
                Value = ""
            });
            list.Add(new SelectListItem()
            {
                Text = "正取",
                Value = "1"
            });
            list.Add(new SelectListItem()
            {
                Text = "備取",
                Value = "2"
            });

            return list;
        }
    }
}