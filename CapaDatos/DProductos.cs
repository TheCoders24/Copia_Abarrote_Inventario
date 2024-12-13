using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class DProductos
    {
        public DProductos(int idProducto, string nombre, decimal precio, string descripcion, int idProveedor)
        {
            IdProducto = idProducto;
            Nombre = nombre;
            Precio = precio;
            Descripcion = descripcion;
            IdProveedor = idProveedor;
        }

        #region Propiedades
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public int IdProveedor { get; set; }
        #endregion

        //#region MetodoInsertar
        //public async Task<string> InsertarAsync(DProductos producto)
        //{
        //    string respuesta;
        //    using (var conexionSql = await Utilidades.ObtenerConexionAsync())
        //    {

        //        if (conexionSql == null)
        //        {
        //            throw new Exception("No se Puedo Establecer la Conexion");
        //        }

        //        using (var transaccion = conexionSql.BeginTransaction())
        //        {
        //            try
        //            {
        //                string consultaSql = "INSERT INTO Producto (Nombre, Precio, Descripcion) VALUES (@nombre, @precio, @descripcion)";
        //                using (var comandoSql = new SqlCommand(consultaSql, conexionSql, transaccion))
        //                {

        //                    comandoSql.Parameters.Add("@nombre", SqlDbType.NVarChar).Value = producto.Nombre;
        //                    comandoSql.Parameters.Add("@precio", SqlDbType.Decimal).Value = producto.Precio;
        //                    comandoSql.Parameters.Add("@descripcion", SqlDbType.NVarChar).Value = producto.Descripcion;
        //                    comandoSql.Parameters.Add("@idProveedor", SqlDbType.Int).Value = producto.IdProveedor;
        //                    transaccion.Commit();
        //                    respuesta = await comandoSql.ExecuteNonQueryAsync() == 1 ? "Ok" : "No se pudo insertar el registro";
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                transaccion.Rollback();
        //                respuesta = ex.Message; // Consider logging the exception here.
        //            }
        //        }
        //    }

        //    return respuesta;
        //}
        //#endregion

        //#region MetodoEditar
        //public async Task<string> EditarAsync(DProductos producto)
        //{
        //    string respuesta;
        //    using (var conexionSql = await Utilidades.ObtenerConexionAsync())
        //    {
        //        if (conexionSql == null)
        //            throw new Exception("No se Puedo Establecer la Conexion");
        //        using (var transaccion = conexionSql.BeginTransaction())
        //        {
        //            try
        //            {
        //                string consultaSql = "UPDATE Producto SET Nombre = @nombre, Precio = @precio, Descripcion = @descripcion, ID_Proveedor = @idProveedor WHERE ID_Producto = @idProducto";
        //                using (var comandoSql = new SqlCommand(consultaSql, conexionSql, transaccion))
        //                {
        //                    comandoSql.Parameters.AddWithValue("@idProducto", producto.IdProducto);
        //                    comandoSql.Parameters.AddWithValue("@nombre", producto.Nombre);
        //                    comandoSql.Parameters.AddWithValue("@precio", producto.Precio);
        //                    comandoSql.Parameters.AddWithValue("@descripcion", producto.Descripcion);
        //                    comandoSql.Parameters.AddWithValue("@idProveedor", producto.IdProveedor);

        //                    transaccion.Commit();
        //                    respuesta = await comandoSql.ExecuteNonQueryAsync() == 1 ? "Ok" : "No se pudo editar el registro";
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                transaccion.Rollback();
        //                respuesta = ex.Message; // Consider logging the exception here.
        //            }

        //        }
        //    }
        //    return respuesta;
        //}
        //#endregion

        //#region MetodoEliminar
        //public async Task<string> EliminarAsync(DProductos producto)
        //{
        //    string respuesta;
        //    using (var conexionSql = await Utilidades.ObtenerConexionAsync())
        //    {
        //        if (conexionSql == null)
        //            throw new Exception("No se puedo Establer la Conexion");
        //        var transaccion = conexionSql.BeginTransaction();   
        //        try
        //        {
        //            string consultaSql = "DELETE FROM Producto WHERE ID_Producto = @idProducto";
        //            using (var comandoSql = new SqlCommand(consultaSql, conexionSql, transaccion))
        //            {
        //                comandoSql.Parameters.AddWithValue("@idProducto", producto.IdProducto);

        //                transaccion.Commit();
        //                respuesta = await comandoSql.ExecuteNonQueryAsync() == 1 ? "Ok" : "No se pudo eliminar el registro";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            transaccion.Rollback();
        //            respuesta = ex.Message; // Consider logging the exception here.
        //        }
        //    }

        //    return respuesta;
        //}
        //#endregion
        #region MetodoInsertar
        public async Task<string> InsertarAsync(DProductos producto)
        {
            string respuesta;
            using (var conexionSql = await Utilidades.ObtenerConexionAsync())
            {
                if (conexionSql == null)
                {
                    throw new Exception("No se Pudo Establecer la Conexion");
                }

                using (var transaccion = conexionSql.BeginTransaction())
                {
                    try
                    {
                        string consultaSql = "usp_AltaProducto"; // Nombre del procedimiento almacenado
                        using (var comandoSql = new SqlCommand(consultaSql, conexionSql, transaccion))
                        {
                            comandoSql.CommandType = CommandType.StoredProcedure;

                            comandoSql.Parameters.Add("@nombre", SqlDbType.NVarChar).Value = producto.Nombre;
                            comandoSql.Parameters.Add("@precio", SqlDbType.Decimal).Value = producto.Precio;
                            comandoSql.Parameters.Add("@descripcion", SqlDbType.NVarChar).Value = producto.Descripcion;
                            comandoSql.Parameters.Add("@idProveedor", SqlDbType.Int).Value = producto.IdProveedor;
                            var parametroResultado = new SqlParameter("@Resultado", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };
                            comandoSql.Parameters.Add(parametroResultado);

                            await comandoSql.ExecuteNonQueryAsync();
                            transaccion.Commit();
                            respuesta = parametroResultado.Value.ToString() == "1" ? "Ok" : "No se pudo insertar el registro";
                        }
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback();
                        respuesta = ex.Message; // Consider logging the exception here.
                    }
                }
            }
            return respuesta;
        }
        #endregion

        #region MetodoEditar
        public async Task<string> EditarAsync(DProductos producto)
        {
            string respuesta;
            using (var conexionSql = await Utilidades.ObtenerConexionAsync())
            {
                if (conexionSql == null)
                    throw new Exception("No se Pudo Establecer la Conexion");

                using (var transaccion = conexionSql.BeginTransaction())
                {
                    try
                    {
                        string consultaSql = "usp_ActualizarProducto"; // Nombre del procedimiento almacenado
                        using (var comandoSql = new SqlCommand(consultaSql, conexionSql, transaccion))
                        {
                            comandoSql.CommandType = CommandType.StoredProcedure;

                            comandoSql.Parameters.AddWithValue("@idProducto", producto.IdProducto);
                            comandoSql.Parameters.AddWithValue("@nombre", producto.Nombre);
                            comandoSql.Parameters.AddWithValue("@precio", producto.Precio);
                            comandoSql.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                            comandoSql.Parameters.AddWithValue("@idProveedor", producto.IdProveedor);
                            var parametroResultado = new SqlParameter("@Resultado", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };
                            comandoSql.Parameters.Add(parametroResultado);

                            await comandoSql.ExecuteNonQueryAsync();
                            transaccion.Commit();
                            respuesta = parametroResultado.Value.ToString() == "1" ? "Ok" : "No se pudo editar el registro";
                        }
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback();
                        respuesta = ex.Message; // Consider logging the exception here.
                    }
                }
            }
            return respuesta;
        }
        #endregion

        #region MetodoEliminar
        public async Task<string> EliminarAsync(DProductos producto)
        {
            string respuesta;
            using (var conexionSql = await Utilidades.ObtenerConexionAsync())
            {
                if (conexionSql == null)
                    throw new Exception("No se pudo Establecer la Conexion");

                using (var transaccion = conexionSql.BeginTransaction())
                {
                    try
                    {
                        string consultaSql = "usp_BajaProducto"; // Nombre del procedimiento almacenado
                        using (var comandoSql = new SqlCommand(consultaSql, conexionSql, transaccion))
                        {
                            comandoSql.CommandType = CommandType.StoredProcedure;

                            comandoSql.Parameters.AddWithValue("@idProducto", producto.IdProducto);
                            var parametroResultado = new SqlParameter("@Resultado", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };
                            comandoSql.Parameters.Add(parametroResultado);

                            await comandoSql.ExecuteNonQueryAsync();
                            transaccion.Commit();
                            respuesta = parametroResultado.Value.ToString() == "1" ? "Ok" : "No se pudo eliminar el registro";
                        }
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback();
                        respuesta = ex.Message; // Consider logging the exception here.
                    }
                }
            }
            return respuesta;
        }
        #endregion


        #region MetodoMostrar
        public async Task<DataTable> MostrarAsync()
        {
            var resultadoTabla = new DataTable("producto");
            using (var conexionSql = await Utilidades.ObtenerConexionAsync())
            {
                try
                {
                    string consultaSql = "SELECT TOP (1000) [Nombre], [Precio], [Descripcion], [ID_Proveedor] FROM [AbarroteDB].[dbo].[Producto]";
                    using (var comandoSql = new SqlCommand(consultaSql, conexionSql))
                    {
                        using (var sqlDat = new SqlDataAdapter(comandoSql))
                        {
                            sqlDat.Fill(resultadoTabla);
                        }
                    }
                }
                catch (Exception)
                {
                    resultadoTabla = null;
                }
            }

            return resultadoTabla;
        }
        #endregion

        #region MetodoBuscarNombre
        public async Task<DataTable> BuscarNombreAsync(string nombre)
        {
            var resultadoTabla = new DataTable("producto");
            using (var conexionSql = await Utilidades.ObtenerConexionAsync())
            {
                try
                {
                    string consultaSql = "SELECT TOP (1000) [ID_Producto], [Nombre], [Precio], [Descripcion], [ID_Proveedor] FROM [AbarroteDB].[dbo].[Producto] WHERE Nombre LIKE @nombre + '%'";
                    using (var comandoSql = new SqlCommand(consultaSql, conexionSql))
                    {
                        comandoSql.Parameters.AddWithValue("@nombre", nombre);
                        using (var sqlDat = new SqlDataAdapter(comandoSql))
                        {
                            sqlDat.Fill(resultadoTabla);
                        }
                    }
                }
                catch (Exception)
                {
                    resultadoTabla = null;
                }
            }

            return resultadoTabla;
        }
        #endregion

    }
}
