using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class DGraficas
    {

        public async Task<List<(object XValue, object YValue)>> GetVentasPorMesAsync()
        {
            string query = "SELECT Mes, TotalVentasMes FROM Vista_VentasPorMes";
            return await EjecutarConsultaAsync(query);
        }

        public async Task<List<(object XValue, object YValue)>> GetVentasPorClienteAsync()
        {
            string query = "SELECT NombreCliente, TotalComprasCliente FROM Vista_TotalVentasPorCliente";
            return await EjecutarConsultaAsync(query);
        }

        public async Task<List<(object XValue, object YValue)>> GetProductosVendidosAsync()
        {
            string query = "SELECT NombreProducto, TotalProductosVendidos FROM Vista_ProductosVendidos";
            return await EjecutarConsultaAsync(query);
        }

        public async Task<List<(object XValue, object YValue)>> GetIngresoDiarioAsync()
        {
            string query = "SELECT FechaVenta, IngresoDiario FROM Vista_IngresoDiario";
            return await EjecutarConsultaAsync(query);
        }

        private async Task<List<(object XValue, object YValue)>> EjecutarConsultaAsync(string query)
        {
            var datos = new List<(object XValue, object YValue)>();
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                datos.Add((reader.GetValue(0), reader.GetValue(1)));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
                // Aquí se puede implementar un sistema de logging adicional si es necesario.
            }
            return datos;
        }
    }
}
