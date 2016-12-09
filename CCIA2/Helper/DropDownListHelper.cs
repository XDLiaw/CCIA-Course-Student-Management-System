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
                    Text = "-- 審核階段 --",
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

        public static List<SelectListItem> getAppraiseGroupList(bool withSelectAllOption)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (withSelectAllOption)
            {
                list.Add(new SelectListItem()
                {
                    Text = "全部",
                    Value = "0"
                });
            }
            var items =
            (
                from tg in db.TableGroup
                select new SelectListItem
                {
                    Text = tg.GroupName,
                    Value = tg.sqno.ToString()
                }
            );
            list.AddRange(items);

            return list;
        }

        public static List<SelectListItem> getAppraiseGroupNameList(bool withSelectAllOption)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (withSelectAllOption)
            {
                list.Add(new SelectListItem()
                {
                    Text = "-- 組別 --",
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

        public static List<SelectListItem> getEnrollTypeList(bool withSelectAllOption)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (withSelectAllOption)
            {
                list.Add(new SelectListItem()
                {
                    Text = "-- 正/備取 --",
                    Value = ""
                });
            }

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

        public static List<SelectListItem> getCourseClassList(bool withSelectAllOption)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (withSelectAllOption)
            {
                list.Add(new SelectListItem()
                {
                    Text = "全部",
                    Value = "0"
                });
            }

            var items = 
            (
                from cc in db.CourseClass
                select new SelectListItem
                {
                    Text = cc.name,
                    Value = cc.sqno.ToString()
                }
            );
            list.AddRange(items);

            return list;
        }

        public static List<SelectListItem> getTeacherList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var items = 
            (
                from t in db.CourseTeacher
                orderby t.name
                select new SelectListItem
                {
                    Text = t.name,
                    Value = t.sqno.ToString()
                }
            );
            list.AddRange(items);

            return list;
        }

        public static List<SelectListItem> getMemberTypeList(bool withSelectAllOption)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (withSelectAllOption)
            {
                list.Add(new SelectListItem()
                {
                    Text = "全部",
                    Value = "0"
                });
            }

            var items =
            (
                from mt in db.TableMemberType
                select new SelectListItem
                {
                    Text = mt.membertypename,
                    Value = mt.sqno.ToString()
                }
            );
            list.AddRange(items);

            return list;
        }

        public static List<SelectListItem> getRoleList(bool withSelectAllOption)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (withSelectAllOption)
            {
                list.Add(new SelectListItem()
                {
                    Text = "全部",
                    Value = "0"
                });
            }

            list.Add(new SelectListItem()
            {
                Text = "社會人士",
                Value = "1"
            });
            list.Add(new SelectListItem()
            {
                Text = "學生",
                Value = "2"
            });

            return list;
        }

        public static List<SelectListItem> getEducationLevelList(bool withSelectAllOption)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (withSelectAllOption)
            {
                list.Add(new SelectListItem()
                {
                    Text = "全部",
                    Value = "0"
                });
            }
            list.Add(new SelectListItem()
            {
                Text = "博士",
                Value = "博士"
            });
            list.Add(new SelectListItem()
            {
                Text = "碩士",
                Value = "碩士"
            });
            list.Add(new SelectListItem()
            {
                Text = "大學",
                Value = "大學"
            }); list.Add(new SelectListItem()
            {
                Text = "專科",
                Value = "專科"
            }); list.Add(new SelectListItem()
            {
                Text = "高中職(含以下)",
                Value = "高中職"
            });

            return list;
        }

        // 產業別
        public static List<SelectListItem> getIndustryTypeList(bool withSelectAllOption)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (withSelectAllOption)
            {
                list.Add(new SelectListItem()
                {
                    Text = "全部",
                    Value = "0"
                });
            }

            list.Add(new SelectListItem()
            {
                Text = "文創產業",
                Value = "1"
            });
            list.Add(new SelectListItem()
            {
                Text = "其他產業",
                Value = "2"
            });

            return list;
        }

        // 產業
        public static List<SelectListItem> getCultureList(bool withSelectAllOption)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (withSelectAllOption)
            {
                list.Add(new SelectListItem()
                {
                    Text = "請選擇產業",
                    Value = "0"
                });
            }

            var items =
            (
                from c in db.TableCulture
                select new SelectListItem
                {
                    Text = c.CultureName,
                    Value = c.sqno.ToString()
                }
            );
            list.AddRange(items);

            return list;
        }

        // 是否出席
        public static List<SelectListItem> getWillComeList(bool withSelectAllOption)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (withSelectAllOption)
            {
                list.Add(new SelectListItem()
                {
                    Text = "全部",
                    Value = "0"
                });
            }

            list.Add(new SelectListItem()
            {
                Text = "出席",
                Value = "Y"
            });
            list.Add(new SelectListItem()
            {
                Text = "不出席",
                Value = "N"
            });

            return list;
        }

        public static List<SelectListItem> getAndOrList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "AND",
                Value = "AND"
            });
            list.Add(new SelectListItem()
            {
                Text = "OR",
                Value = "OR"
            });

            return list;
        }

    }
}