using CapaDatos;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CapaNegocio
{
    public class tickets
    {
        //    private readonly int[] _denominaciones = new int[] { 1000, 500, 200, 100, 50, 20, 10, 5, 2, 1 };

        //    public string GenerarTicketPDF(int idVenta, DataTable datosVenta, decimal total, decimal montoRecibido, decimal cambio, Dictionary<int, int> desgloseBilletes)
        //    {
        //        string nombreArchivo = $"TicketVenta_{idVenta}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        //        string rutaCompleta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nombreArchivo);
        //        Document doc = new Document(PageSize.A7, 10, 10, 10, 10);

        //        using (FileStream fs = new FileStream(rutaCompleta, FileMode.Create, FileAccess.Write, FileShare.None))
        //        {
        //            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
        //            doc.Open();

        //            var fontTitulo = FontFactory.GetFont(FontFactory.COURIER_BOLD, 10);
        //            var fontSubtitulo = FontFactory.GetFont(FontFactory.COURIER, 8);
        //            var fontTexto = FontFactory.GetFont(FontFactory.COURIER, 8);

        //            doc.Add(new Paragraph("RESTAURANTE MODELO, SA DE CV", fontTitulo) { Alignment = Element.ALIGN_CENTER });
        //            doc.Add(new Paragraph("Dirección ejemplo\nCiudad, Estado\nRFC ABC123456", fontSubtitulo) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 10f });

        //            PdfPTable table = new PdfPTable(3);
        //            table.WidthPercentage = 100;
        //            table.SetWidths(new float[] { 2, 1, 1 });

        //            table.AddCell(new PdfPCell(new Phrase("Producto", fontTexto)) { HorizontalAlignment = Element.ALIGN_CENTER });
        //            table.AddCell(new PdfPCell(new Phrase("Cant", fontTexto)) { HorizontalAlignment = Element.ALIGN_CENTER });
        //            table.AddCell(new PdfPCell(new Phrase("Importe", fontTexto)) { HorizontalAlignment = Element.ALIGN_CENTER });

        //            foreach (DataRow row in datosVenta.Rows)
        //            {
        //                table.AddCell(new PdfPCell(new Phrase(row["NombreProducto"].ToString(), fontTexto)));
        //                table.AddCell(new PdfPCell(new Phrase(row["Cantidad"].ToString(), fontTexto)) { HorizontalAlignment = Element.ALIGN_RIGHT });
        //                table.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(row["Total"]).ToString("C"), fontTexto)) { HorizontalAlignment = Element.ALIGN_RIGHT });
        //            }

        //            doc.Add(table);

        //            doc.Add(new Paragraph($"Total: {total:C}\nMonto Recibido: {montoRecibido:C}\nCambio: {cambio:C}", fontSubtitulo) { SpacingBefore = 10f });

        //            if (desgloseBilletes != null)
        //            {
        //                doc.Add(new Paragraph("Desglose de cambio:", fontSubtitulo));
        //                foreach (var billete in desgloseBilletes)
        //                {
        //                    if (billete.Value > 0)
        //                    {
        //                        doc.Add(new Paragraph($"{billete.Value} x {billete.Key:C}", fontTexto));
        //                    }
        //                }
        //            }

        //            doc.Close();
        //        }

        //        return rutaCompleta;
        //    }

        //    public Dictionary<int, int> CalcularDesgloseBilletes(decimal monto)
        //    {
        //        var desglose = new Dictionary<int, int>();
        //        int montoInt = (int)Math.Round(monto);

        //        foreach (int billete in _denominaciones)
        //        {
        //            if (montoInt >= billete)
        //            {
        //                int cantidad = montoInt / billete;
        //                desglose[billete] = cantidad;
        //                montoInt %= billete;
        //            }
        //        }

        //        return desglose;
        //    }


        private string _connectionString;
        private readonly int[] _denominaciones = { 500, 200, 100, 50, 20, 10, 5, 1 };

        public tickets(string connectionString)
        {
            _connectionString = connectionString;
        }

        public tickets() { }

        public string GenerarTicketPDF(int idVenta, DataTable datosVentaManual, decimal montoRecibido, decimal montoEntregado)
        {
            // Obtener datos desde la vista si no se proporcionan manualmente
            DataTable datosVenta = datosVentaManual ?? ObtenerDatosDesdeVista(idVenta);

            if (datosVenta.Rows.Count == 0)
            {
                throw new Exception("No se encontraron datos para el ID de venta especificado.");
            }

            // Calcular valores
            decimal total = Convert.ToDecimal(datosVenta.Compute("SUM(Total)", string.Empty));
            decimal cambio = montoEntregado - total;

            if (cambio < 0)
            {
                throw new Exception("El monto entregado es insuficiente para cubrir el total.");
            }

            Dictionary<int, int> desgloseBilletes = CalcularDesgloseBilletes(cambio);

            // Crear ticket en PDF
            string nombreArchivo = $"TicketVenta_{idVenta}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string rutaCompleta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nombreArchivo);
            Document doc = new Document(PageSize.A7, 10, 10, 10, 10);

            using (FileStream fs = new FileStream(rutaCompleta, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                var fontTitulo = FontFactory.GetFont(FontFactory.COURIER_BOLD, 10);
                var fontSubtitulo = FontFactory.GetFont(FontFactory.COURIER, 8);
                var fontTexto = FontFactory.GetFont(FontFactory.COURIER, 8);

                doc.Add(new Paragraph("RESTAURANTE MODELO, SA DE CV", fontTitulo) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph("Dirección ejemplo\nCiudad, Estado\nRFC ABC123456", fontSubtitulo) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 10f });

                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2, 1, 1 });

                table.AddCell(new PdfPCell(new Phrase("Producto", fontTexto)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Cant", fontTexto)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Importe", fontTexto)) { HorizontalAlignment = Element.ALIGN_CENTER });

                foreach (DataRow row in datosVenta.Rows)
                {
                    table.AddCell(new PdfPCell(new Phrase(row["NombreProducto"].ToString(), fontTexto)));
                    table.AddCell(new PdfPCell(new Phrase(row["Cantidad"].ToString(), fontTexto)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(row["Total"]).ToString("C"), fontTexto)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                }

                doc.Add(table);

                doc.Add(new Paragraph($"Total: {total:C}\nMonto Recibido: {montoEntregado:C}\nCambio: {cambio:C}", fontSubtitulo) { SpacingBefore = 10f });

                if (desgloseBilletes != null)
                {
                    doc.Add(new Paragraph("Desglose de cambio:", fontSubtitulo));
                    foreach (var billete in desgloseBilletes)
                    {
                        if (billete.Value > 0)
                        {
                            doc.Add(new Paragraph($"{billete.Value} x {billete.Key:C}", fontTexto));
                        }
                    }
                }

                doc.Close();
            }

            return rutaCompleta;
        }

        private DataTable ObtenerDatosDesdeVista(int idVenta)
        {
            string query = "SELECT * FROM Vista_Ticket WHERE ID_Venta = @ID_Venta";
            DataTable datosVenta = new DataTable();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Venta", idVenta);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(datosVenta);
                }
            }

            return datosVenta;
        }

        public Dictionary<int, int> CalcularDesgloseBilletes(decimal monto)
        {
            var desglose = new Dictionary<int, int>();
            int montoInt = (int)Math.Round(monto);

            foreach (int billete in _denominaciones)
            {
                if (montoInt >= billete)
                {
                    int cantidad = montoInt / billete;
                    desglose[billete] = cantidad;
                    montoInt %= billete;
                }
            }

            return desglose;
        }

    }

}
