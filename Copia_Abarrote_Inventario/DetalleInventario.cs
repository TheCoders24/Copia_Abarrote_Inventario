﻿using CapaDatos;
using CapaNegocio;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Copia_Abarrote_Inventario
{
    public partial class DetalleInventario : Form
    {
        public DetalleInventario()
        {
            InitializeComponent();
            CargarProductosDesdeBD();
            CargarClientesDB();
            ConfigurarDataGridView();
            Fecha();
        }
        private void Fecha()
        {
            var Fecha = DateTime.Now;
            label10.Text = Fecha.ToShortDateString();
        }
        private async void CargarProductosDesdeBD()
        {
            SqlConnection connection = null; // Declarar la conexión fuera del bloque using
            try
            {
                connection = await Utilidades.ObtenerConexionAsync(); // Inicializar la conexión
                                                                      // connection.Open(); // No es necesario abrirla explícitamente, se maneja en Utilidades.Conexion()

                string query = "SELECT ID_Producto, Nombre, Precio, Descripcion FROM Producto";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable productosTable = new DataTable();
                adapter.Fill(productosTable);

                if (productosTable.Rows.Count > 0)
                {
                    comboboxProducto.DataSource = productosTable;
                    comboboxProducto.DisplayMember = "Nombre";  // Mostrar nombre del producto
                    comboboxProducto.ValueMember = "ID_Producto"; // Guardar ID del producto
                }
                else
                {
                    MessageBox.Show("No se encontraron productos.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close(); // Cerrar la conexión explícitamente
                }
            }
        }
        private async void CargarClientesDB()
        {
            SqlConnection connection = null; // Declarar la conexión fuera del bloque using
            try
            {
                connection = await Utilidades.ObtenerConexionAsync(); // Inicializar la conexión
                                                                      // connection.Open(); // No es necesario abrirla explícitamente, se maneja en Utilidades.Conexion()

                string query = "SELECT [ID_Cliente], [Nombre] FROM [AbarroteDB].[dbo].[Cliente]";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable clientesTable = new DataTable();
                adapter.Fill(clientesTable);

                if (clientesTable.Rows.Count > 0)
                {
                    comboBoxCliente.DataSource = clientesTable; // Asegúrate de tener comboboxCliente definido
                    comboBoxCliente.DisplayMember = "Nombre";  // Mostrar nombre del cliente
                    comboBoxCliente.ValueMember = "ID_Cliente"; // Guardar ID del cliente
                }
                else
                {
                    MessageBox.Show("No se encontraron clientes.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close(); // Cerrar la conexión explícitamente
                }
            }
        }
        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (comboboxProducto.SelectedItem == null || comboBoxCliente.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un Producto y un Cliente");
                return;
            }
            //Verificamos que la cantidad sea un numero valido
            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingresa una Cantidad Valida");
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precioUnitario) || precioUnitario <= 0)
            {
                MessageBox.Show("Ingrese un Precio Valido");
                return;
            }

            decimal importe = cantidad * precioUnitario;

            dataGridView1.Rows.Add(
                   comboboxProducto.Text, // Producto
                   cantidad,              // Cantidad
                   precioUnitario,       // Importe
                   importe                // Total
           );
            string nombreProducto = comboboxProducto.Text;
            string nombreCliente = comboBoxCliente.Text;

            CargarDetallesVenta(nombreProducto, nombreCliente, cantidad, precioUnitario);


            try
            {
                // Obtener los datos de la venta
                int idVenta = ObtenerIdVenta();  // Suponiendo que tienes un método para obtener el ID de venta
                UserControl1 userControl1 = new UserControl1();

                // Obtener el monto recibido desde el UserControl1
                decimal montoRecibido;
                if (!decimal.TryParse(userControl1.TxtMonto.Text, out montoRecibido))
                {
                    throw new Exception("El monto recibido no tiene un formato válido.");
                }

                // Obtener los datos de la venta (se puede hacer desde la vista o un DataTable pasado manualmente)
                DataTable datosVenta = ObtenerDatosVenta(idVenta);  // Método que obtiene los datos de la venta

                // Calcular el total de la venta
                decimal totalVenta = Convert.ToDecimal(datosVenta.Compute("SUM(Total)", string.Empty));

                // Calcular el cambio
                decimal cambio = montoRecibido - totalVenta;

                // Validar si el monto recibido es suficiente para cubrir la venta
                if (cambio < 0)
                {
                    throw new Exception("El monto recibido es insuficiente para cubrir el total de la venta.");
                }

                // Crear el ticket asincrónicamente
                tickets ticketGenerator = new tickets($"Server={Utilidades.SqlServer};Database={Utilidades.SqlDataBase};User Id={Utilidades.SqlUserId};Password={Utilidades.SqlPassword};Pooling=true;Min Pool Size=1;Max Pool Size=100;");

                // Generar el PDF del ticket
                string rutaTicket = ticketGenerator.GenerarTicketPDF(idVenta, datosVenta, montoRecibido, cambio);

                // Mostrar mensaje o realizar alguna acción luego de generar el ticket
                MessageBox.Show($"Ticket generado con éxito. Ubicación: {rutaTicket}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que ocurra
                MessageBox.Show($"Error al generar el ticket: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }
        private int ObtenerIdVenta()
        {
            // Obtener el ID de la venta de algún control, como un TextBox o ComboBox
            return Convert.ToInt32(resultado);  // Asumiendo que el ID de la venta se ingresa en un TextBox
        }

        private DataTable ObtenerDatosVenta(int idVenta)
        {
            // Este es el mismo método que ya tienes para obtener los datos de la venta.
            string query = "SELECT * FROM Vista_Ticket WHERE ID_Venta = @ID_Venta";
            DataTable datosVenta = new DataTable();

            using (var connection = new SqlConnection($"Server={Utilidades.SqlServer};Database={Utilidades.SqlDataBase};User Id={Utilidades.SqlUserId};Password={Utilidades.SqlPassword};Pooling=true;Min Pool Size=1;Max Pool Size=100;"))
            {
                connection.Open(); // Asegúrate de abrir la conexión
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar el parámetro del ID de venta
                    command.Parameters.AddWithValue("@ID_Venta", idVenta);

                    // Llenar el DataTable con los datos obtenidos
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(datosVenta);
                }
            }

            return datosVenta;

        }

        public int resultado;
        public async Task CargarDetallesVenta(string nombreProducto, string nombreCliente, int cantidad, decimal precioUnitario)
        {
            // Validaciones de los datos
            if (string.IsNullOrWhiteSpace(nombreProducto))
            {
                MessageBox.Show("El nombre del producto no puede estar vacío.");
                return;
            }

            if (string.IsNullOrWhiteSpace(nombreCliente))
            {
                MessageBox.Show("El nombre del cliente no puede estar vacío.");
                return;
            }

            if (cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor que cero.");
                return;
            }

            if (precioUnitario <= 0)
            {
                MessageBox.Show("El precio unitario debe ser mayor que cero.");
                return;
            }

            // Cálculo del subtotal
            decimal subtotal = cantidad * precioUnitario;
            string metodopago = comboBoxmetodopago.Text;
            txtsubtotal.Text = subtotal.ToString();
            txtTotal.Text = subtotal.ToString();

            try
            {
                using (SqlConnection connection = await Utilidades.ObtenerConexionAsync())
                {
                    if (connection == null)
                    {
                        MessageBox.Show("Error al establecer conexión con la base de datos.");
                        return;
                    }

                    using (SqlCommand command = new SqlCommand("usp_RegistrarVenta", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Parámetros para el procedimiento almacenado
                        command.Parameters.AddWithValue("@Fecha", DateTime.Now);
                        command.Parameters.AddWithValue("@Importe", subtotal);
                        command.Parameters.AddWithValue("@Iva", 0.16m * subtotal); // IVA del 16%
                        command.Parameters.AddWithValue("@Total", 1.16m * subtotal);
                        command.Parameters.AddWithValue("@Metodo_Pago", metodopago); // Cambia según el método de pago
                        command.Parameters.Add("@ID_Cliente", SqlDbType.Int).Value = await ObtenerIdClientePorNombre(nombreCliente) ?? (object)DBNull.Value;
                        command.Parameters.Add("@ID_Producto", SqlDbType.Int).Value = await ObtenerIdProductoPorNombre(nombreProducto) ?? (object)DBNull.Value;
                        command.Parameters.AddWithValue("@Cantidad", cantidad);
                        command.Parameters.AddWithValue("@Precio_Unitario", precioUnitario);
                        // Parámetro de salida
                        SqlParameter resultadoParam = new SqlParameter("@Resultado", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(resultadoParam);

                        // Ejecutar el procedimiento
                        await command.ExecuteNonQueryAsync();
                         resultado = (int)command.Parameters["@Resultado"].Value;
                        if (resultado == -1)
                        {
                            MessageBox.Show("No hay suficiente inventario para completar la venta.");
                        }
                        else if (resultado == 0)
                        {
                            MessageBox.Show("Ocurrió un error al registrar la venta.");
                        }
                        else
                        {
                            MessageBox.Show($"Venta registrada exitosamente con ID: {resultado}");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Error de base de datos: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message);
            }
        }
        private void ConfigurarDataGridView()
        {
            dataGridView1.Columns.Add("ID_Producto", "ID_Producto");
            dataGridView1.Columns.Add("Cantidad", "Cantidad");
            dataGridView1.Columns.Add("PrecioUnitario", "Precio Unitario");
            dataGridView1.Columns.Add("Total", "Total");
        }
        // limpiamos los controles del windows forms 
        public void LimpiarControles()
        {
            comboBoxCliente.SelectedIndex = -1;
            comboboxProducto.SelectedIndex = -1;
            txtCantidad.Clear();
            txtsubtotal.Clear();
            txtTotal.Clear();
            txtiva.Clear();
        }
        public async Task<int?> ObtenerIdClientePorNombre(string nombreCliente)
        {
            string query = "SELECT ID_Cliente FROM Cliente WHERE Nombre = @NombreCliente";

            using (SqlConnection connection = await Utilidades.ObtenerConexionAsync())
            {
                if (connection == null)
                {
                    throw new Exception("Error al establecer conexión con la base de datos.");
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NombreCliente", nombreCliente);

                    object resultado = await command.ExecuteScalarAsync();
                    return resultado != null ? (int?)Convert.ToInt32(resultado) : null; // Retorna el ID o null si no se encuentra
                }
            }
        }

        public async Task<int?> ObtenerIdProductoPorNombre(string nombreProducto)
        {
            string query = "SELECT ID_Producto FROM Producto WHERE Nombre = @NombreProducto";

            using (SqlConnection connection = await Utilidades.ObtenerConexionAsync())
            {
                if (connection == null)
                {
                    throw new Exception("Error al establecer conexión con la base de datos.");
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NombreProducto", nombreProducto);

                    object resultado = await command.ExecuteScalarAsync();
                    return resultado != null ? (int?)Convert.ToInt32(resultado) : null; // Retorna el ID o null si no se encuentra
                }
            }
        }

        private void comboboxProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboboxProducto.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)comboboxProducto.SelectedItem;
                decimal precio = Convert.ToDecimal(selectedRow["Precio"]);
                txtPrecio.Text = precio.ToString("F2"); // Muestra el precio con dos decimales

                // Reinicia el campo de cantidad
                txtCantidad.Text = "1"; // Valor predeterminado

            }
        }
    }
}
