using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using GestorComercial_API.Data;
using MailKit.Net.Smtp;
using MimeKit;

namespace GestorComercial_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ReportesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("stock")]
        public async Task<IActionResult> DescargarReporteStock()
        {
            var productos = await _context.Productos.ToListAsync();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text("Reporte de Stock Actual")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Darken2);

                    page.Content().PaddingVertical(1, Unit.Centimetre).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.ConstantColumn(100);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Producto").Bold();
                            header.Cell().Text("Cantidad").Bold();
                        });

                        foreach (var item in productos)
                        {
                            table.Cell().Text(item.Nombre); // Asume que tu modelo tiene una propiedad Nombre
                            table.Cell().Text(item.Stock.ToString());
                        }
                    });
                });
            }).GeneratePdf();

            return File(pdf, "application/pdf", "ReporteStock.pdf");



        }

        [HttpPost("enviar-stock")]
        public async Task<IActionResult> EnviarReporteStock([FromQuery] string correoDestino)
        {
            // 1. Generar el PDF en memoria
            var productos = await _context.Productos.ToListAsync();
            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));
                    page.Header().Text("Reporte de Stock").SemiBold().FontSize(20);

                    page.Content().PaddingVertical(1, Unit.Centimetre).Table(table =>
                    {
                        table.ColumnsDefinition(c => { c.RelativeColumn(); c.ConstantColumn(100); });
                        foreach (var item in productos)
                        {
                            table.Cell().Text(item.Nombre);
                            table.Cell().Text(item.Stock.ToString());
                        }
                    });
                });
            }).GeneratePdf();

            // 2. Configurar el correo y el adjunto
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Gestor Comercial", "tucorreo@gmail.com"));
            mensaje.To.Add(new MailboxAddress("Destinatario", correoDestino));
            mensaje.Subject = "Reporte de Stock Actual";

            var bodyBuilder = new BodyBuilder { TextBody = "Adjunto el reporte de stock solicitado." };
            bodyBuilder.Attachments.Add("ReporteStock.pdf", pdfBytes, new ContentType("application", "pdf"));
            mensaje.Body = bodyBuilder.ToMessageBody();

            // 3. Conectar al servidor SMTP y enviar
            using var client = new SmtpClient();
            try
            {
                // Se utiliza el puerto 587 con StartTls para Gmail
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("exe.bustos5@gmail.com", "xnlkffcaxhiaituy");
                await client.SendAsync(mensaje);
                await client.DisconnectAsync(true);

                return Ok("Reporte enviado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al enviar el correo: {ex.Message}");
            }
        }
    }
}