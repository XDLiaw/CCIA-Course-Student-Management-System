using CCIA2.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCIA2.Helper.ExcelReport
{
    public class ActivitySignUpListReport
    {
        public IWorkbook create(List<ActivitySignUp> signUpList)
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
            cell.SetCellValue("姓名");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("Email(1)");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("手機");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("電話");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("用餐");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("是否出席");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("服務機關");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("學校名稱");

            #endregion

            #region 資料
            foreach (ActivitySignUp asu in signUpList)
            {
                frow = worksheet.CreateRow(++rowIndex);
                frow.HeightInPoints = rowHeigh;
                colIndex = -1;

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(asu.name);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(asu.email1);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(asu.mobile);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(asu.phone);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(asu.meal);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(asu.isCome);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(asu.socComp);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(asu.stuSchName);
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