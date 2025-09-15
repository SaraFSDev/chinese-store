using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.Services.IServices
{
    public interface IReportService
    {
        Task<byte[]> GenerateWinnersReport();
        Task<byte[]> GenerateRevenueReportAsync();
    }
}



