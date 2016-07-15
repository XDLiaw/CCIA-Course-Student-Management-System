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
            cell.SetCellValue("是否出席");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("附檔名稱");

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
                cell.SetCellValue(student.IsAttendString);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(student.attachFilesString);
            }

            #endregion

            return workbook;
        }
    }
}