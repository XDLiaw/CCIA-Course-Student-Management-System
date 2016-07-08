using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.SS.UserModel;
using CCIA2.Models;
using NPOI.HSSF.UserModel;

namespace CCIA2.Helper.ExcelReport
{
    public class MemberReport
    {
        public IWorkbook create(List<Member> memberList)
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
            cell.SetCellValue("報名組別1");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("報名組別2");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("報名組別3");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("資格審組別");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("最終組別");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("初審平均分數");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("複審平均分數");

            cell = frow.CreateCell(++colIndex);
            cell.CellStyle = headerStyle;
            cell.SetCellValue("狀態");

            #endregion

            #region 資料

            foreach (Member member in memberList)
            {
                frow = worksheet.CreateRow(++rowIndex);
                frow.HeightInPoints = rowHeigh;
                colIndex = -1;

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(member.mrNumber);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(member.mrName);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(member.MemberGroupApply.Count == 0 ? "" : member.MemberGroupApply.ElementAt<MemberGroupApply>(0).First);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(member.MemberGroupApply.Count == 0 ? "" : member.MemberGroupApply.ElementAt<MemberGroupApply>(0).Second);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(member.MemberGroupApply.Count == 0 ? "" : member.MemberGroupApply.ElementAt<MemberGroupApply>(0).Third);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;

                cell.SetCellValue(member.FirstAssignGroup);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(member.FinalGroup);

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(member.numberOfFristTrailScore != 0 ? (member.firstTrailScore.Value.ToString("0.0") + "(" + member.numberOfFristTrailScore + ")") : "");

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(member.numberOfSecondTrailScore != 0 ? (member.secondTrailAvgScore.Value.ToString("0.0") + "(" + member.numberOfSecondTrailScore + ")") : "");

                cell = frow.CreateCell(++colIndex);
                cell.CellStyle = contentStyle;
                cell.SetCellValue(member.currentState);
            }

            #endregion

            return workbook;
        }

    }
}