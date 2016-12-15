using CCIA2.Models;
using CCIA2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcPaging;

namespace CCIA2.Services
{
    public class MemberService
    {
        private CCIAContext db;

        public MemberService(CCIAContext db)
        {
            this.db = db;
        }

        public MemberSearchViewModel searchNPagging(MemberSearchViewModel model, SysUser user)
        {
            IQueryable<Member> memberQuery = db.Member.Where(m => m.mrMemberTypesqno == model.memberTypeNo && m.mrIsActive == "Y" && m.mrIsFinish == "Y");

            #region 經紀仲介學員
            if (model.memberTypeNo == 1) 
            {
                 if (model.searchText != null && model.searchText.Trim().Length > 0) //只要這內容不為空就忽略其他條件
                {
                    memberQuery = queryByText(memberQuery, model.searchText, model.operate, model.searchText2);
                    model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                }
                else
                {
                    memberQuery = queryProAndSkill(memberQuery, model.searchText2);
                    if (model.step == null) // 全部階段
                    {
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 0) // 待審核 
                    {
                        memberQuery = queryMemberAtStep0(memberQuery);
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 1)  // 通過資格審
                    {
                        memberQuery = queryMemberAtStep1(memberQuery, user);
                        memberQuery = queryFirstAssignGroup(memberQuery, model.group);
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 2) // 完成初審
                    {
                        memberQuery = queryMemberAtStep2(memberQuery, user);
                        memberQuery = queryFirstAssignGroup(memberQuery, model.group);
                        model.memberPagedList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 2).Average(res => res.AppraiseScore)) // 依初審平均分數由高到低排序
                            .ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 3) // 進行複審
                    {
                        memberQuery = queryMemberAtStep3(memberQuery, user);
                        model.memberPagedList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 2).Average(res => res.AppraiseScore)) // 依初審平均分數由高到低排序
                            .ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 4) // 完成複審
                    {
                        memberQuery = queryMemberAtStep4(memberQuery, user);
                        memberQuery = queryFirstAssignGroup(memberQuery, model.group);
                        model.memberPagedList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 4).Average(res => res.AppraiseScore)) // 依複審平均分數由高到低排序
                            .ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 5) // 正/備取名單
                    {
                        memberQuery = queryMemberAtStep5(memberQuery);
                        memberQuery = queryFinalGroup(memberQuery, model.group);
                        memberQuery = queryMemberEnrollType(memberQuery, model.enrollType);
                        model.memberPagedList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 4).Average(res => res.AppraiseScore)) // 依複審平均分數由高到低排序
                            .ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 6) //已通過
                    {
                        memberQuery = queryMemberAtStep6(memberQuery);
                        memberQuery = queryFinalGroup(memberQuery, model.group);
                        memberQuery = queryMemberEnrollType(memberQuery, model.enrollType);
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 7) //未通過
                    {
                        memberQuery = queryMemberAtStep7(memberQuery);
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                }

                // 重新排序各會員的歷史審查紀錄
                foreach (Member m in model.memberPagedList)
                {
                    m.MemberGroupResult = m.MemberGroupResult.OrderBy(r => r.AppraiseStep).ToList();
                }
            }
            #endregion
            #region 歷屆會員 or 一般會員
            else if (model.memberTypeNo == 2 || model.memberTypeNo == 3)
            {
                if (String.IsNullOrWhiteSpace(model.searchText) == false)
                {
                    memberQuery = queryByText(memberQuery, model.searchText, model.operate, model.searchText2);
                }
                else
                {
                    memberQuery = queryProAndSkill(memberQuery, model.searchText2);
                }
                model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
            }
            #endregion

            return model;
        }

        public List<Member> search(MemberSearchViewModel model, SysUser user)
        {
            IQueryable<Member> memberQuery = db.Member.Where(m => m.mrMemberTypesqno == model.memberTypeNo && m.mrIsActive == "Y" && m.mrIsFinish == "Y");
            List<Member> memberList = null;

            #region 經紀仲介學員
            if (model.memberTypeNo == 1)
            {
                if (model.searchText != null && model.searchText.Trim().Length > 0) //只要這內容不為空就忽略其他條件
                {
                    memberQuery = queryByText(memberQuery, model.searchText, model.operate, model.searchText2);
                    memberList = memberQuery.OrderBy(m => m.mrNumber).ToList();
                }
                else
                {
                    memberQuery = queryProAndSkill(memberQuery, model.searchText2);
                    if (model.step == null) // 全部階段
                    {
                        memberList = memberQuery.OrderBy(m => m.mrNumber).ToList();
                    }
                    else if (model.step == 0) // 待審核 
                    {
                        memberQuery = queryMemberAtStep0(memberQuery);
                        memberList = memberQuery.OrderBy(m => m.mrNumber).ToList();
                    }
                    else if (model.step == 1)  // 通過資格審
                    {
                        memberQuery = queryMemberAtStep1(memberQuery, user);
                        memberQuery = queryFirstAssignGroup(memberQuery, model.group);
                        memberList = memberQuery.OrderBy(m => m.mrNumber).ToList();
                    }
                    else if (model.step == 2) // 完成初審
                    {
                        memberQuery = queryMemberAtStep2(memberQuery, user);
                        memberQuery = queryFirstAssignGroup(memberQuery, model.group);
                        memberList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == model.step).Average(res => res.AppraiseScore)) // 依初審平均分數由高到低排序
                            .ToList();
                    }
                    else if (model.step == 3) // 進行複審
                    {
                        memberQuery = queryMemberAtStep3(memberQuery, user);
                        memberList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 2).Average(res => res.AppraiseScore)) // 依初審平均分數由高到低排序
                            .ToList();
                    }
                    else if (model.step == 4) // 完成複審
                    {
                        memberQuery = queryMemberAtStep4(memberQuery, user);
                        memberQuery = queryFirstAssignGroup(memberQuery, model.group);
                        memberList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 4).Average(res => res.AppraiseScore)) // 依複審平均分數由高到低排序
                            .ToList();
                    }
                    else if (model.step == 5) // 正/備取名單
                    {
                        memberQuery = queryMemberAtStep5(memberQuery);
                        memberQuery = queryFinalGroup(memberQuery, model.group);
                        memberQuery = queryMemberEnrollType(memberQuery, model.enrollType);
                        memberList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 4).Average(res => res.AppraiseScore)) // 依複審平均分數由高到低排序
                            .ToList();
                    }
                    else if (model.step == 6) //已通過
                    {
                        memberQuery = queryMemberAtStep6(memberQuery);
                        memberQuery = queryFinalGroup(memberQuery, model.group);
                        memberQuery = queryMemberEnrollType(memberQuery, model.enrollType);
                        memberList = memberQuery.OrderBy(m => m.mrNumber).ToList();
                    }
                    else if (model.step == 7) //未通過
                    {
                        memberQuery = queryMemberAtStep7(memberQuery);
                        memberList = memberQuery.OrderBy(m => m.mrNumber).ToList();
                    }
                }

                // 重新排序各會員的歷史審查紀錄
                foreach (Member m in memberList)
                {
                    m.MemberGroupResult = m.MemberGroupResult.OrderBy(r => r.AppraiseStep).ToList();
                }
            }
            #endregion
            #region 歷屆會員 or 一般會員
            else if (model.memberTypeNo == 2 || model.memberTypeNo == 3)
            {
                if (String.IsNullOrWhiteSpace(model.searchText) == false)
                {
                    memberQuery = queryByText(memberQuery, model.searchText, model.operate, model.searchText2);
                }
                else
                {
                    memberQuery = queryProAndSkill(memberQuery, model.searchText2);
                }
                memberList = memberQuery.OrderBy(m => m.mrNumber).ToList();
            }
            #endregion

            return memberList;
        }

        public IQueryable<Member> queryByText(IQueryable<Member> memberQuery, string searchText, string operate, string searchText2)
        {
            if (String.IsNullOrWhiteSpace(searchText2))
            {
                memberQuery = memberQuery
                   .Where(m => m.mrName.Contains(searchText)
                       || m.mrMainEmail.Equals(searchText)
                       || m.mrOtherEmail.Equals(searchText)
                       || m.mrNumber.Equals(searchText)
                       || m.mrId.Equals(searchText));
            }
            else
            {
                if (operate.Equals("AND"))
                {
                    memberQuery = memberQuery
                        .Where(m => m.mrName.Contains(searchText)
                            || m.mrMainEmail.Equals(searchText)
                            || m.mrOtherEmail.Equals(searchText)
                            || m.mrNumber.Equals(searchText)
                            || m.mrId.Equals(searchText));
                    memberQuery = memberQuery.Where(m => m.mrPro.Contains(searchText2) || m.mrSkill.Contains(searchText2));
                }
                else
                {
                    memberQuery = memberQuery.Where(m =>
                        (m.mrName.Contains(searchText) || m.mrMainEmail.Equals(searchText) || m.mrOtherEmail.Equals(searchText) || m.mrNumber.Equals(searchText) || m.mrId.Equals(searchText))
                        ||
                        (m.mrPro.Contains(searchText2) || m.mrSkill.Contains(searchText2)));
                }
            }
            return memberQuery;
        }

        public IQueryable<Member> queryProAndSkill(IQueryable<Member> memberQuery, String searchText2)
        {
            if (String.IsNullOrWhiteSpace(searchText2) == false)
            {
                memberQuery = memberQuery.Where(m => m.mrPro.Contains(searchText2) || m.mrSkill.Contains(searchText2));
            }
            return memberQuery;
        }

        // 待審核
        public IQueryable<Member> queryMemberAtStep0(IQueryable<Member> memberQuery)
        {
            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count() == 0);
            return memberQuery;
        }

        // 通過資格審
        public IQueryable<Member> queryMemberAtStep1(IQueryable<Member> memberQuery, SysUser user)
        {
            memberQuery = memberQuery
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 1) == 1) // 已有"通過資格審"資料一筆
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 1 && res.AppraiseNo == user.accountNo) == 0) // 表自己還沒評初審過                
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep >= 3) == 0); // 不能有正備取or通過or未通過or進入複審紀錄
            return memberQuery;
        }

        // 完成初審
        public IQueryable<Member> queryMemberAtStep2(IQueryable<Member> memberQuery, SysUser user)
        {
            memberQuery = memberQuery
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 2) == 0) // 還沒有下一階段"進行複審"的資料
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 2 && res.AppraiseNo == user.accountNo) == 1); // 有一筆自己審過的初審資料
            return memberQuery;
        }

        // 進行複審
        public IQueryable<Member> queryMemberAtStep3(IQueryable<Member> memberQuery, SysUser user)
        {
            memberQuery = memberQuery
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 3) == 1) // 已有一筆"進行複審"的資料 (已在複審待審核清單中)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 4 && res.AppraiseNo == user.accountNo) == 0) // 表示自己還沒評過複審
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep >= 5) == 0); // 不能有正備取or通過or未通過紀錄
            return memberQuery;
        }

        // 完成複審
        public IQueryable<Member> queryMemberAtStep4(IQueryable<Member> memberQuery, SysUser user)
        {
            memberQuery = memberQuery
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 4) == 0) // 還沒有下一階段的資料
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 4 && res.AppraiseNo == user.accountNo) == 1); // 已有一筆自己審過的複審資料
            return memberQuery;
        }

        // 正備取名單
        public IQueryable<Member> queryMemberAtStep5(IQueryable<Member> memberQuery)
        {
            memberQuery = memberQuery
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 5) == 0)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5) >= 1);
            return memberQuery;
        }

        //已通過
        public IQueryable<Member> queryMemberAtStep6(IQueryable<Member> memberQuery)
        {
            memberQuery = memberQuery
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 6) == 0)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 6) == 1);
            return memberQuery;
        }

        //未通過
        public IQueryable<Member> queryMemberAtStep7(IQueryable<Member> memberQuery)
        {
            memberQuery = memberQuery
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 7) == 0)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 7) == 1);
            return memberQuery;
        }

        public IQueryable<Member> queryFirstAssignGroup(IQueryable<Member> memberQuery, string group)
        {
            if (group != null && group.Trim().Length != 0)
            {
                memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 1 && res.AppraiseGroup == group) > 0);
            }
            return memberQuery;
        }

        public IQueryable<Member> queryFinalGroup(IQueryable<Member> memberQuery, string group)
        {
            if (group != null && group.Trim().Length != 0)
            {
                memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseGroup == group) > 0);
            }
            return memberQuery;
        }

        public IQueryable<Member> queryMemberEnrollType(IQueryable<Member> memberQuery, string enrollType)
        {
            if (enrollType != null && enrollType.Trim().Length != 0)
            {
                memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseResult == enrollType) > 0);
            }
            return memberQuery;
        }
    }
}