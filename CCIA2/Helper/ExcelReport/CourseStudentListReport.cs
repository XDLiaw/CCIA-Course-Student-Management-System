using CCIA2.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCIA2.Helper.ExcelReport
{
    public class CourseStudentListReport
    {
        public IWorkbook create(List<MemberCourse> memberCourseList)
        {
            NPOI.SS.UserModel.IWorkbook workbook = new HSSFWorkbook();
            var worksheet = workbook.CreateSheet();
            IRow frow;
            ICell cell;
            int rowIndex = -1, colIndex = -1;
            float rowHeigh = 16.5F;

            #region 樣式

            IFont defaultFont = workbook.CreateFont();
            defaultFont.FontHeightInPoints = 9;

            ICellStyle headerStyle = workbook.CreateCellStyle();
            headerStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            headerStyle.SetFont(defaultFont);
            headerStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            headerStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            headerStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            headerStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            ICellStyle contentStyle = workbook.CreateCellStyle();
            contentStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            contentStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            contentStyle.SetFont(defaultFont);

            #endregion

            #region 標題

            frow = worksheet.CreateRow(++rowIndex);
            frow.HeightInPoints = rowHeigh;

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("會員編號");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("姓名");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("社會人士/學生");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("最高學歷");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("服務機關");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("職稱");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("錄取組別");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("會員角色");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("是否出席");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("附檔名稱");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("電話");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("手機");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("Email1");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("Email2");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("地址");

            #endregion

            #region 資料
            foreach (MemberCourse student in memberCourseList)
            {
                frow = worksheet.CreateRow(++rowIndex);
                frow.HeightInPoints = rowHeigh;
                colIndex = -1;

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(student.member.mrNumber);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(student.member.mrName);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(student.member.mrRole == "1" ? "社會人士" : "學生");

                if (student.member.mrRole == "1")
                {
                    cell = frow.CreateCell(++colIndex);
                    cell.CellStyle = contentStyle;
                    cell.SetCellValue(student.member.mrSocialEdu);

                    cell = frow.CreateCell(++colIndex);
                    cell.CellStyle = contentStyle;
                    cell.SetCellValue(student.member.mrSocialComp);

                    cell = frow.CreateCell(++colIndex);
                    cell.CellStyle = contentStyle;
                    cell.SetCellValue(student.member.mrSocialTitle);
                }
                else
                {
                    cell = frow.CreateCell(++colIndex);
                    cell.CellStyle = contentStyle;
                    cell.SetCellValue(student.member.mrStuSchool);

                    cell = frow.CreateCell(++colIndex);
                    cell.CellStyle = contentStyle;
                    cell.SetCellValue(student.member.mrStuDept);

                    cell = frow.CreateCell(++colIndex);
                    cell.CellStyle = contentStyle;
                    cell.SetCellValue(student.member.mrStuYear);
                }

                if (student.member.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseResult == "1") == 1)
                {
                    cell = frow.CreateCell(++colIndex);
                    cell.CellStyle = contentStyle;
                    cell.SetCellValue(student.member.MemberGroupResult.Where(res => res.AppraiseStep == 5 && res.AppraiseResult == "1").FirstOrDefault().AppraiseGroup);
                }
                else
                {
                    cell = frow.CreateCell(++colIndex);
                    cell.CellStyle = contentStyle;
                    cell.SetCellValue("");
                }

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                if (student.member.mrMemberTypesqno == 3 && student.member.MemberGroupResult.Count(res => res.AppraiseStep == 7) == 1)
                {
                    cell.SetCellValue(student.member.memberType.membertypename + "(本屆未通過)");
                }
                else
                {
                    cell.SetCellValue(student.member.memberType.membertypename);
                }                

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(student.IsAttendString);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(student.attachFilesString);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(student.member.mrTel);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(student.member.mrMobile);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(student.member.mrMainEmail);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(student.member.mrOtherEmail);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(student.member.mrAddress);
            }

            #endregion

            for (int i = 0; i < worksheet.GetRow(0).LastCellNum; i++)
            {
                worksheet.AutoSizeColumn(i);
            }
            worksheet.CreateFreezePane(0, 1);

            return workbook;
        }
    }
}