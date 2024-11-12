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
            string query = "SELECT NombreProducto, TotalUnidadesVendidas FROM Vista_TotalProductosVendidos";
            return await EjecutarConsultaAsync(query);
        }

        //public async Task<List<(object XValue, object YValue)>> GetIngresoDiarioAsync()
        //{
        //    string query = "SELECT * FROM Vista_IngresoDiario;";
        //    return await EjecutarConsultaAsync(query);
        //}

        public async Task<List<(object XValue, object YValue)>> GetIngresoDiarioAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            string query = @"
        SELECT FechaVenta, IngresoTotalDiario 
        FROM Vista_IngresoDiario
        WHERE FechaVenta BETWEEN @FechaInicio AND @FechaFin";

            var resultado = new List<(object XValue, object YValue)>();

            using (var connection = await Utilidades.ObtenerConexionAsync())
            {
                

                using (var command = new SqlCommand(query, connection))
                {
                    // Definir los parámetros de la consulta
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var fechaVenta = reader["FechaVenta"];
                            var ingresoTotalDiario = reader["IngresoTotalDiario"];
                            resultado.Add((fechaVenta, ingresoTotalDiario));
                        }
                    }
                }
            }

            return resultado;
        }


        private async Task<List<(object XValue, object YValue)>> EjecutarConsultaAsync(string query)
        {
            var datos = new List<(object XValue, object YValue)>();
            try
            {
                using (SqlConnection connection = await Utilidades.ObtenerConexionAsync())
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
