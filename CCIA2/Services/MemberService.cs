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
            if (model.memberTypeNo == 1) //經紀仲介學員
            {
                IQueryable<Member> memberQuery = db.Member.Where(m => m.mrMemberTypesqno == model.memberTypeNo && m.mrIsActive == "Y" && m.mrIsFinish == "Y");
                if (model.searchText != null && model.searchText.Trim().Length > 0) //只要這內容不為空就忽略其他條件
                {
                    memberQuery = memberQuery
                        .Where(m => m.mrName.Contains(model.searchText)
                            || m.mrMainEmail.Equals(model.searchText)
                            || m.mrOtherEmail.Equals(model.searchText)
                            || m.mrNumber.Equals(model.searchText)
                            || m.mrId.Equals(model.searchText));
                    model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                }
                else
                {
                    if (model.step == null) // 全部階段
                    {
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 0) // 待審核 
                    {
                        memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count() == 0);
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 1)  // 通過資格審
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step && res.AppraiseNo == user.accountNo) == 0) // 表自己還沒評初審過
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1) // 已有"通過資格審"資料一筆
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep >= 3) == 0); // 不能有正備取or通過or未通過or進入複審紀錄
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 1 && res.AppraiseGroup == model.group) > 0);
                        }
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 2) // 完成初審
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0) // 還沒有下一階段"進行複審"的資料
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step && res.AppraiseNo == user.accountNo) == 1); // 有一筆自己審過的初審資料
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 1 && res.AppraiseGroup == model.group) > 0);
                        }
                        model.memberPagedList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 2).Average(res => res.AppraiseScore)) // 依初審平均分數由高到低排序
                            .ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 3) // 進行複審
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step && res.AppraiseNo == user.accountNo) == 0) // 表示自己還沒評過複審
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1) // 已有一筆"進行複審"的資料 (已在複審待審核清單中)
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep >= 5) == 0); // 不能有正備取or通過or未通過紀錄
                        model.memberPagedList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 2).Average(res => res.AppraiseScore)) // 依初審平均分數由高到低排序
                            .ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 4) // 完成複審
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0) // 還沒有下一階段的資料
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step && res.AppraiseNo == user.accountNo) == 1); // 已有一筆自己審過的複審資料
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 1 && res.AppraiseGroup == model.group) > 0);
                        }
                        model.memberPagedList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 4).Average(res => res.AppraiseScore)) // 依複審平均分數由高到低排序
                            .ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 5) // 正/備取名單
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0)
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1);
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseGroup == model.group) > 0);
                        }
                        if (model.enrollType != null && model.enrollType.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseResult == model.enrollType) > 0);
                        }

                        model.memberPagedList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 4).Average(res => res.AppraiseScore)) // 依複審平均分數由高到低排序
                            .ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 6) //已通過
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0)
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1);
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseGroup == model.group) > 0);
                        }
                        if (model.enrollType != null && model.enrollType.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseResult == model.enrollType) > 0);
                        }
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 7) //未通過
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0)
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1);
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                }

                // 重新排序各會員的歷史審查紀錄
                foreach (Member m in model.memberPagedList)
                {
                    m.MemberGroupResult = m.MemberGroupResult.OrderBy(r => r.AppraiseStep).ToList();
                }
            }
            else if (model.memberTypeNo == 2 || model.memberTypeNo == 3) //歷屆會員 or 一般會員
            {
                model.memberPagedList = db.Member
                    .Where(m => m.mrMemberTypesqno == model.memberTypeNo)
                    .Where(m => model.searchText == null ? true : m.mrName.Contains(model.searchText)
                        || model.searchText == null ? true : m.mrMainEmail.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrOtherEmail.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrNumber.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrId.Equals(model.searchText))
                    .OrderBy(m => m.mrNumber)
                    .ToPagedList(model.pageNumber - 1, model.pageSize);
            }

            return model;
        }

        public List<Member> search(MemberSearchViewModel model, SysUser user)
        {
            List<Member> memberList = null;
            if (model.memberTypeNo == 1) //經紀仲介學員
            {
                IQueryable<Member> memberQuery = db.Member.Where(m => m.mrMemberTypesqno == model.memberTypeNo && m.mrIsActive == "Y" && m.mrIsFinish == "Y");
                if (model.searchText != null && model.searchText.Trim().Length > 0) //只要這內容不為空就忽略其他條件
                {
                    memberQuery = memberQuery
                        .Where(m => m.mrName.Contains(model.searchText)
                            || m.mrMainEmail.Equals(model.searchText)
                            || m.mrOtherEmail.Equals(model.searchText)
                            || m.mrNumber.Equals(model.searchText)
                            || m.mrId.Equals(model.searchText));
                    memberList = memberQuery.OrderBy(m => m.mrNumber).ToList();
                }
                else
                {
                    if (model.step == null) // 全部階段
                    {
                        memberList = memberQuery.OrderBy(m => m.mrNumber).ToList();
                    }
                    else if (model.step == 0) // 待審核 
                    {
                        memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count() == 0);
                        memberList = memberQuery.OrderBy(m => m.mrNumber).ToList();
                    }
                    else if (model.step == 1)  // 通過資格審
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step && res.AppraiseNo == user.accountNo) == 0) // 表自己還沒評初審過
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1) // 已有"通過資格審"資料一筆
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep >= 3) == 0); // 不能有正備取or通過or未通過or進入複審紀錄
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 1 && res.AppraiseGroup == model.group) > 0);
                        }
                        memberList = memberQuery.OrderBy(m => m.mrNumber).ToList();
                    }
                    else if (model.step == 2) // 完成初審
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0) // 還沒有下一階段"進行複審"的資料
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step && res.AppraiseNo == user.accountNo) == 1); // 有一筆自己審過的初審資料
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 1 && res.AppraiseGroup == model.group) > 0);
                        }
                        memberList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == model.step).Average(res => res.AppraiseScore)) // 依初審平均分數由高到低排序
                            .ToList();
                    }
                    else if (model.step == 3) // 進行複審
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step && res.AppraiseNo == user.accountNo) == 0) // 表示自己還沒評過複審
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1) // 已有一筆"進行複審"的資料 (已在複審待審核清單中)
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep >= 5) == 0); // 不能有正備取or通過or未通過紀錄
                        memberList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 2).Average(res => res.AppraiseScore)) // 依初審平均分數由高到低排序
                            .ToList();
                    }
                    else if (model.step == 4) // 完成複審
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0) // 還沒有下一階段的資料
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step && res.AppraiseNo == user.accountNo) == 1); // 已有一筆自己審過的複審資料
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 1 && res.AppraiseGroup == model.group) > 0);
                        }
                        memberList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 4).Average(res => res.AppraiseScore)) // 依複審平均分數由高到低排序
                            .ToList();
                    }
                    else if (model.step == 5) // 正/備取名單
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0)
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1);
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseGroup == model.group) > 0);
                        }
                        if (model.enrollType != null && model.enrollType.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseResult == model.enrollType) > 0);
                        }

                        memberList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 4).Average(res => res.AppraiseScore)) // 依複審平均分數由高到低排序
                            .ToList();
                    }
                    else if (model.step == 6) //已通過
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0)
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1);
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseGroup == model.group) > 0);
                        }
                        if (model.enrollType != null && model.enrollType.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseResult == model.enrollType) > 0);
                        }
                        memberList = memberQuery.OrderBy(m => m.mrNumber).ToList();
                    }
                    else if (model.step == 7) //未通過
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0)
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1);
                        memberList = memberQuery.OrderBy(m => m.mrNumber).ToList();
                    }
                }

                // 重新排序各會員的歷史審查紀錄
                foreach (Member m in memberList)
                {
                    m.MemberGroupResult = m.MemberGroupResult.OrderBy(r => r.AppraiseStep).ToList();
                }
            }
            else if (model.memberTypeNo == 2 || model.memberTypeNo == 3) //歷屆會員 or 一般會員
            {
                memberList = db.Member
                    .Where(m => m.mrMemberTypesqno == model.memberTypeNo)
                    .Where(m => model.searchText == null ? true : m.mrName.Contains(model.searchText)
                        || model.searchText == null ? true : m.mrMainEmail.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrOtherEmail.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrNumber.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrId.Equals(model.searchText))
                    .OrderBy(m => m.mrNumber)
                    .ToList();
            }

            return memberList;
        }
    }
}