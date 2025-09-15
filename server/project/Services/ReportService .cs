using System;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.Services.IServices;
using project.Repositories.IRepositories;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace project.Services
{
    public class ReportService : IReportService
    {
        private readonly IWinningRepository _winningRepository;

        public ReportService(IWinningRepository winningRepository)
        {
            _winningRepository = winningRepository;
        }

        public async Task<byte[]> GenerateWinnersReport()
        {
            var gifts = await _winningRepository.GetWinnersWithGiftsAsync();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Winners");

            worksheet.Cells[1, 4].Value = "שם הזוכה";
            worksheet.Cells[1, 3].Value = "מתנה";
            worksheet.Cells[1, 2].Value = "מחיר המתנה";
            worksheet.Cells[1, 1].Value = "תאריך הזכייה";

            int row = 2;
            foreach (var gift in gifts)
            {
                worksheet.Cells[row, 4].Value = gift.Winning?.LinkedUser?.UserName;   
                worksheet.Cells[row, 3].Value = gift.Name;                          
                worksheet.Cells[row, 2].Value = gift.Price;                          
                worksheet.Cells[row, 1].Value = gift.Winning?.Date.ToString("yyyy-MM-dd");
                row++;
            }

            return package.GetAsByteArray();
        }
        public async Task<byte[]> GenerateRevenueReportAsync()
        {
            var (revenues, totalRevenue) = await _winningRepository.GetGiftRevenuesAsync();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Revenue Report");

                worksheet.Cells[1, 1].Value = "מתנה";
                worksheet.Cells[1, 2].Value = "סך הכנסות";
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int row = 2;
                foreach (var revenue in revenues)
                {
                    worksheet.Cells[row, 1].Value = revenue.GiftName;
                    worksheet.Cells[row, 2].Value = revenue.Revenue;
                    row++;
                }

                worksheet.Cells[row, 1].Value = "סה\"כ";
                worksheet.Cells[row, 2].Value = totalRevenue;
                worksheet.Cells[row, 1, row, 2].Style.Font.Bold = true;
                worksheet.Cells[row, 1, row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, 1, row, 2].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

                worksheet.Column(1).AutoFit();
                worksheet.Column(2).AutoFit();

                return package.GetAsByteArray();
            }
        }
    }
}



