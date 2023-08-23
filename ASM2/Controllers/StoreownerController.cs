using System.Drawing;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBOOK_STORE.Models;
using FPTBOOK_STORE.Areas.Identity.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Microsoft.AspNetCore.Authorization;

namespace FPTBOOK_STORE.Controllers
{
    [Authorize(Roles = "StoreOwner")]
    public class StoreownerController : Controller
    {
        private readonly FPTBOOK_STOREIdentityDbContext _context;

        public StoreownerController(FPTBOOK_STOREIdentityDbContext context)
        {
            _context = context;
        }
        private string Layout = "StoreownerLayout";
        public IActionResult Index()
        {

            ViewBag.Layout = Layout;
            return View();
        }
        public IActionResult Chart()
        {
            var data = _context.OrderDetail.Include(s => s.Book)
            .GroupBy(s => s.Book.Name)
            .Select(g => new { Name = g.Key, Total = g.Sum(s => s.Quantity) })
            .ToList();
            string[] labels = new string[data.Count()];
            string[] totals = new string[data.Count()];
            string[] rgbs = new string[data.Count()];
            Random rnd = new Random();
            int red = rnd.Next(0, 255);
            int blue = rnd.Next(0, 255);
            int green = rnd.Next(0, 255);
            for (int i = 0; i < data.Count(); i++)
            {
                labels[i] = data[i].Name;
                totals[i] = data[i].Total.ToString();

                rgbs[i] = ("'rgb(" + red.ToString() + "," + green.ToString() + "," + blue.ToString() + ")'");
            }


            ViewData["rgbs"] = String.Join(",", rgbs);
            ViewData["labels"] = String.Format("'{0}'", String.Join("','", labels));
            ViewData["totals"] = String.Join(",", totals);

            ViewBag.Layout = Layout;
            return View();
        }
        public IActionResult ExportExcel()
        {
            var exel = _context.OrderDetail.Include(o => o.Order).Include(b => b.Book).GroupBy(s => s.Book.Name)
                                                    .Select(g => new
                                                    {
                                                        Name = g.Key,
                                                        Price = g.Select(s => s.Book.Price),
                                                        TotalSum = g.Sum(s => s.Quantity * s.Book.Price * 1.1),
                                                        Quantity = g.Sum(s => s.Quantity)
                                                    })
                                                    .ToList(); ;
            // List<OrderDetail> orderDetails = exel;
            double total = 0;
            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Book Saling");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                const int startRow = 5;
                var row = startRow;

                //Create Headers and format them
                using (var r = worksheet.Cells["A1:C1"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                }

                worksheet.Cells["A2"].Value = "Book Name";
                worksheet.Cells["B2"].Value = "Book Price";
                worksheet.Cells["C2"].Value = "Quantity";
                worksheet.Cells["D2"].Value = "Total";
                worksheet.Cells["A2:D2"].Style.Font.Bold = true;
                worksheet.Cells["A2:D2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A2:D2"].Style.Fill.BackgroundColor.SetColor(Color.Azure);

                row = 3;
                foreach (var order in exel)
                {
                    total += order.TotalSum;
                    worksheet.Cells[row, 1].Value = order.Name;
                    worksheet.Cells[row, 2].Value = order.Price;
                    worksheet.Cells[row, 3].Value = order.Quantity;
                    worksheet.Cells[row, 4].Value = order.TotalSum;
                    row++;
                }
                worksheet.Cells[row, 1].Value = "Total";
                worksheet.Cells[row, 4].Value = total;
                String range = "A2:D" + row.ToString();
                var modelTable = worksheet.Cells[range];
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.AutoFitColumns();
                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "movies.xlsx");
        }

    }
}