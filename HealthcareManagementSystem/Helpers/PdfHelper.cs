using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using HealthcareManagementSystem.DTOs; // Assuming InvoiceResponse is in DTOs namespace

namespace HealthcareManagementSystem.Helpers
{
    public class PdfHelper
    {
        public static byte[] GenerateInvoicePdf(InvoiceResponse invoiceResponse)
        {
            var invoice = invoiceResponse.Invoice;

            using (var memoryStream = new MemoryStream())
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                document.Add(new Paragraph($"Invoice Number: {invoice.InvoiceNumber}"));
                document.Add(new Paragraph($"Patient Username: {invoiceResponse.PatientUsername}"));
                document.Add(new Paragraph($"Doctor Username: {invoiceResponse.DoctorUsername}"));
                document.Add(new Paragraph($"Date: {invoice.Date.ToString("yyyy-MM-dd")}"));
                document.Add(new Paragraph($"Subtotal: {invoice.Subtotal:C}"));
                document.Add(new Paragraph($"Tax: {invoice.Tax:C}"));
                document.Add(new Paragraph($"Total: {invoice.Total:C}"));
                document.Add(new Paragraph("Services:"));

                foreach (var service in invoice.Services)
                {
                    document.Add(new Paragraph($"- {service.Description} ({service.Code}): {service.Quantity} x {service.UnitPrice:C} = {service.Total:C}"));
                }

                document.Add(new Paragraph("Charges:"));

                foreach (var charge in invoice.Charges)
                {
                    document.Add(new Paragraph($"- {charge.Description} ({charge.Code}): {charge.Quantity} x {charge.UnitPrice:C} = {charge.Total:C}"));
                }

                document.Add(new Paragraph($"Payment Method: {invoice.PaymentMethod}"));
                document.Add(new Paragraph($"Payment Date: {invoice.PaymentDate.ToString("yyyy-MM-dd")}"));
                document.Add(new Paragraph($"Amount Paid: {invoice.AmountPaid:C}"));

                document.Close();
                return memoryStream.ToArray();
            }
        }
    }
}
