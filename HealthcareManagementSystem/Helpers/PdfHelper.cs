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
                Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Fonts
                var titleFont = FontFactory.GetFont("Arial", 24, Font.BOLD);
                var boldFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                var regularFont = FontFactory.GetFont("Arial", 12);
                var smallFont = FontFactory.GetFont("Arial", 10);

                // Adding Invoice Header
                PdfPTable headerTable = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    SpacingAfter = 20
                };
                headerTable.SetWidths(new float[] { 70, 30 });

                headerTable.AddCell(GetCellWithLeading("INVOICE", PdfPCell.ALIGN_LEFT, titleFont, PdfPCell.NO_BORDER, 1));

                var invoiceDetails = new Phrase();
                invoiceDetails.Add(new Chunk("Invoice No: ", boldFont));
                invoiceDetails.Add(new Chunk($"{invoice.InvoiceNumber.Substring(0, 10)}\n", regularFont));
                invoiceDetails.Add(new Chunk("Invoice Date: ", boldFont));
                invoiceDetails.Add(new Chunk($"{invoice.Date.ToString("yyyy-MM-dd")}\n", regularFont));
                invoiceDetails.Add(new Chunk("Due Date: ", boldFont));
                invoiceDetails.Add(new Chunk($"{invoice.Date.AddDays(30).ToString("yyyy-MM-dd")}", regularFont));

                headerTable.AddCell(new PdfPCell(invoiceDetails)
                {
                    HorizontalAlignment = PdfPCell.ALIGN_RIGHT,
                    Border = PdfPCell.NO_BORDER,
                    Padding = 5
                });

                document.Add(headerTable);

                // Adding Billing Information
                PdfPTable billingTable = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    SpacingAfter = 10
                };

                billingTable.AddCell(GetCellWithLeading("Invoice from:", PdfPCell.ALIGN_LEFT, boldFont, PdfPCell.NO_BORDER));
                billingTable.AddCell(GetCellWithLeading("Billed to:", PdfPCell.ALIGN_RIGHT, boldFont, PdfPCell.NO_BORDER));

                billingTable.AddCell(GetCellWithLeading("NairobiGeneral\n" + "1234 Main St, Nairobi, KENYA\nEmail: s***@gmail.com\nPhone: 0708***430", PdfPCell.ALIGN_LEFT, regularFont, PdfPCell.NO_BORDER));
                billingTable.AddCell(GetCellWithLeading($"{invoiceResponse.PatientUsername}\n5678 RiverRoad, Nairobi, Kenya\ninfo@tajiriwango.com\n0712761212", PdfPCell.ALIGN_RIGHT, regularFont, PdfPCell.NO_BORDER));

                document.Add(billingTable);

                // Adding Itemized Services and Charges
                PdfPTable itemTable = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    SpacingAfter = 10
                };
                itemTable.SetWidths(new float[] { 40, 20, 20, 20 });

                PdfPCell itemHeaderCell = new PdfPCell(new Phrase("Item", boldFont))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                    Padding = 5,
                    Border = PdfPCell.BOX
                };
                PdfPCell quantityHeaderCell = new PdfPCell(new Phrase("Quantity", boldFont))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                    Padding = 5,
                    Border = PdfPCell.BOX
                };
                PdfPCell priceHeaderCell = new PdfPCell(new Phrase("Price", boldFont))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                    Padding = 5,
                    Border = PdfPCell.BOX
                };
                PdfPCell totalHeaderCell = new PdfPCell(new Phrase("Total", boldFont))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                    Padding = 5,
                    Border = PdfPCell.BOX
                };

                itemTable.AddCell(itemHeaderCell);
                itemTable.AddCell(quantityHeaderCell);
                itemTable.AddCell(priceHeaderCell);
                itemTable.AddCell(totalHeaderCell);

                foreach (var service in invoice.Services)
                {
                    itemTable.AddCell(GetCellWithLeading(service.Description, PdfPCell.ALIGN_LEFT, regularFont, PdfPCell.BOX));
                    itemTable.AddCell(GetCellWithLeading(service.Quantity.ToString(), PdfPCell.ALIGN_CENTER, regularFont, PdfPCell.BOX));
                    itemTable.AddCell(GetCellWithLeading(service.UnitPrice.ToString("C"), PdfPCell.ALIGN_CENTER, regularFont, PdfPCell.BOX));
                    itemTable.AddCell(GetCellWithLeading(service.Total.ToString("C"), PdfPCell.ALIGN_CENTER, regularFont, PdfPCell.BOX));
                }

                foreach (var charge in invoice.Charges)
                {
                    itemTable.AddCell(GetCellWithLeading(charge.Description, PdfPCell.ALIGN_LEFT, regularFont, PdfPCell.BOX));
                    itemTable.AddCell(GetCellWithLeading(charge.Quantity.ToString(), PdfPCell.ALIGN_CENTER, regularFont, PdfPCell.BOX));
                    itemTable.AddCell(GetCellWithLeading(charge.UnitPrice.ToString("C"), PdfPCell.ALIGN_CENTER, regularFont, PdfPCell.BOX));
                    itemTable.AddCell(GetCellWithLeading(charge.Total.ToString("C"), PdfPCell.ALIGN_CENTER, regularFont, PdfPCell.BOX));
                }

                itemTable.AddCell(GetCellWithLeading("Subtotal", PdfPCell.ALIGN_RIGHT, boldFont, PdfPCell.BOX, 3));
                itemTable.AddCell(GetCellWithLeading(invoice.Subtotal.ToString("C"), PdfPCell.ALIGN_CENTER, regularFont, PdfPCell.BOX));

                itemTable.AddCell(GetCellWithLeading("Tax", PdfPCell.ALIGN_RIGHT, boldFont, PdfPCell.BOX, 3));
                itemTable.AddCell(GetCellWithLeading(invoice.Tax.ToString("C"), PdfPCell.ALIGN_CENTER, regularFont, PdfPCell.BOX));

                itemTable.AddCell(GetCellWithLeading("Total", PdfPCell.ALIGN_RIGHT, boldFont, PdfPCell.BOX, 3));
                itemTable.AddCell(GetCellWithLeading(invoice.Total.ToString("C"), PdfPCell.ALIGN_CENTER, regularFont, PdfPCell.BOX));

                document.Add(itemTable);

                // Adding Payment Information
                PdfPTable paymentTable = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    SpacingAfter = 10
                };

                paymentTable.AddCell(GetCellWithLeading("Payment Information:", PdfPCell.ALIGN_LEFT, boldFont, PdfPCell.NO_BORDER));
                paymentTable.AddCell(GetCellWithLeading($"Payment Method: {invoice.PaymentMethod}\nBank Name: Absa Bank\nBranch: Main Branch\nAccount Number: 123456789", PdfPCell.ALIGN_LEFT, regularFont, PdfPCell.NO_BORDER));

                document.Add(paymentTable);

                // Adding Disclaimer
                Paragraph disclaimer = new Paragraph("This document is computer generated and therefore not signed. It is a valid document.", smallFont)
                {
                    Leading = 14 // Line height for the disclaimer
                };
                document.Add(disclaimer);

                document.Close();
                return memoryStream.ToArray();
            }
        }

        private static PdfPCell GetCellWithLeading(string text, int alignment, Font font, int border, int colspan = 1)
        {
            var phrase = new Phrase(text, font)
            {
                Leading = 14 // Set the leading on the Phrase, not on the PdfPCell
            };
            var cell = new PdfPCell(phrase)
            {
                Colspan = colspan,
                Border = border,
                HorizontalAlignment = alignment,
                Padding = 5
            };
            return cell;
        }
    }
}
