using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class DProveedores
    {
        public int IDProveedor { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        public DProveedores() { }

        public DProveedores(int idproveedor, string nombre, string telefono, string direccion)
        {
            IDProveedor = idproveedor;
            Nombre = nombre;
            Telefono = telefono;
            Direccion = direccion;
        }

        //#region MetodoInsertar
        //public async Task<string> InsertarAsync(DProveedores proveedor)
        //{
        //    string respuesta = "";

        //    try
        //    {
        //        using (var conexionSql = await Utilidades.ObtenerConexionAsync())
        //        {
        //            if (conexionSql == null)
        //            {
        //                throw new InvalidOperationException("No se pudo establecer la conexión a la base de datos.");
        //            }
        //            var transaccion = conexionSql.BeginTransaction();

        //            try
        //            {
        //                string consultaSql = "INSERT INTO Proveedor (Nombre, Telefono, Direccion) VALUES (@nombre, @telefono, @direccion)";
        //                using (var comandoSql = new SqlCommand(consultaSql, conexionSql, transaccion))
        //                {
        //                    comandoSql.Parameters.Add("@nombre", SqlDbType.VarChar).Value = proveedor.Nombre;
        //                    comandoSql.Parameters.Add("@telefono", SqlDbType.VarChar).Value = proveedor.Telefono;
        //                    comandoSql.Parameters.Add("@direccion", SqlDbType.VarChar).Value = proveedor.Direccion;
        //                    transaccion.Commit();
        //                    respuesta = await comandoSql.ExecuteNonQueryAsync() == 1 ? "Ok" : "No se pudo insertar el registro";
        //                }
        //            }
        //            catch (Exception ex) 
        //            {
        //                transaccion.Rollback();
        //                throw new Exception("No se puedo realizar correctametne la insercion" + ex.Message);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        respuesta = "Ocurrió un error: " + ex.Message; // Manejo de excepciones
        //    }

        //    return respuesta;
        //}
        //#endregion

        //#region MetodoEditar
        //public async Task<string> EditarAsync(DProveedores proveedor)
        //{
        //    string respuesta = "";

        //    try
        //    {
        //        using (var conexionSql = await Utilidades.ObtenerConexionAsync())
        //        {
        //            if (conexionSql == null)
        //            {
        //                throw new InvalidOperationException("No se pudo establecer la conexión a la base de datos.");
        //            }

        //            var transaccion = conexionSql.BeginTransaction();

        //            try
        //            {
        //                string consultaSql = "UPDATE Proveedor SET Nombre = @nombre, Telefono = @telefono, Dirección = @direccion WHERE ID_Proveedor = @idproveedor";
        //                using (var comandoSql = new SqlCommand(consultaSql, conexionSql, transaccion))
        //                {
        //                    comandoSql.Parameters.Add("@idproveedor", SqlDbType.Int).Value = proveedor.IDProveedor;
        //                    comandoSql.Parameters.Add("@nombre", SqlDbType.VarChar).Value = proveedor.Nombre;
        //                    comandoSql.Parameters.Add("@telefono", SqlDbType.VarChar).Value = proveedor.Telefono;
        //                    comandoSql.Parameters.Add("@direccion", SqlDbType.VarChar).Value = proveedor.Direccion;

        //                    transaccion.Commit();
        //                    respuesta = await comandoSql.ExecuteNonQueryAsync() == 1 ? "Ok" : "No se pudo editar el registro";
        //                }
        //            }catch (Exception ex)
        //            {
        //                transaccion.Rollback();
        //                throw new Exception("No se puedo realizar la transaccion" + ex.Message);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        respuesta = "Ocurrió un error: " + ex.Message; // Manejo de excepciones
        //    }

        //    return respuesta;
        //}
        //#endregion

        //#region MetodoEliminar
        //public async Task<string> EliminarAsync(DProveedores proveedor)
        //{
        //    string respuesta = "";

        //    try
        //    {
        //        using (var conexionSql = await Utilidades.ObtenerConexionAsync())
        //        {
        //            if (conexionSql == null)
        //            {
        //                throw new InvalidOperationException("No se pudo establecer la conexión a la base de datos.");
        //            }
        //            var transaccion = conexionSql.BeginTransaction();

        //            try
        //            {
        //                string consultaSql = "DELETE FROM Proveedor WHERE ID_Proveedor = @idproveedor";
        //                using (var comandoSql = new SqlCommand(consultaSql, conexionSql, transaccion))
        //                {
        //                    comandoSql.Parameters.AddWithValue("@idproveedor", proveedor.IDProveedor);
        //                    respuesta = await comandoSql.ExecuteNonQueryAsync() == 1 ? "Ok" : "No se pudo eliminar el registro";
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                conexionSql.BeginTransaction();
        //                throw new Exception("No se realizo la transaccion correctamente" + ex.Message);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        respuesta = "Ocurrió un error: " + ex.Message; // Manejo de excepciones
        //    }

        //    return respuesta;
        //}
        //#endregion

        #region MetodoInsertar
        public async Task<string> InsertarAsync(DProveedores proveedor)
        {
            string respuesta = "";

            try
            {
                using (var conexionSql = await Utilidades.ObtenerConexionAsync())
                {
                    if (conexionSql == null)
                    {
                        throw new InvalidOperationException("No se pudo establecer la conexión a la base de datos.");
                    }

                    var transaccion = conexionSql.BeginTransaction();

                    try
                    {
                        using (var comandoSql = new SqlCommand("usp_AltaProveedor", conexionSql, transaccion))
                        {
                            comandoSql.CommandType = CommandType.StoredProcedure;
                            comandoSql.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                            comandoSql.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                            comandoSql.Parameters.AddWithValue("@Direccion", proveedor.Direccion);

                            var resultadoParam = new SqlParameter("@Resultado", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };
                            comandoSql.Parameters.Add(resultadoParam);

                            await comandoSql.ExecuteNonQueryAsync();
                            int resultado = (int)resultadoParam.Value;

                            if (resultado == 1)
                            {
                                respuesta = "Proveedor agregado exitosamente";
                            }
                            else
                            {
                                respuesta = "Error al agregar el proveedor";
                            }

                            transaccion.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback(); // Hacer rollback en caso de error
                        throw new Exception("Error al insertar proveedor: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = "Ocurrió un error: " + ex.Message; // Manejo de excepciones
            }

            return respuesta;
        }
        #endregion
        #region MetodoEditar
        public async Task<string> EditarAsync(DProveedores proveedor)
        {
            string respuesta = "";

            try
            {
                using (var conexionSql = await Utilidades.ObtenerConexionAsync())
                {
                    if (conexionSql == null)
                    {
                        throw new InvalidOperationException("No se pudo establecer la conexión a la base de datos.");
                    }

                    var transaccion = conexionSql.BeginTransaction();

                    try
                    {
                        using (var comandoSql = new SqlCommand("usp_ActualizarProveedor", conexionSql, transaccion))
                        {
                            comandoSql.CommandType = CommandType.StoredProcedure;
                            comandoSql.Parameters.AddWithValue("@ID_Proveedor", proveedor.IDProveedor);
                            comandoSql.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                            comandoSql.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                            comandoSql.Parameters.AddWithValue("@Direccion", proveedor.Direccion);

                            var resultadoParam = new SqlParameter("@Resultado", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };
                            comandoSql.Parameters.Add(resultadoParam);

                            await comandoSql.ExecuteNonQueryAsync();
                            int resultado = (int)resultadoParam.Value;

                            if (resultado == 1)
                            {
                                respuesta = "Proveedor actualizado exitosamente";
                            }
                            else
                            {
                                respuesta = "Error al actualizar el proveedor";
                            }

                            transaccion.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback(); // Hacer rollback en caso de error
                        throw new Exception("Error al actualizar proveedor: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = "Ocurrió un error: " + ex.Message; // Manejo de excepciones
            }

            return respuesta;
        }
        #endregion
        #region MetodoEliminar
        public async Task<string> EliminarAsync(DProveedores proveedor)
        {
            string respuesta = "";

            try
            {
                using (var conexionSql = await Utilidades.ObtenerConexionAsync())
                {
                    if (conexionSql == null)
                    {
                        throw new InvalidOperationException("No se pudo establecer la conexión a la base de datos.");
                    }

                    var transaccion = conexionSql.BeginTransaction();

                    try
                    {
                        using (var comandoSql = new SqlCommand("usp_BajaProveedor", conexionSql, transaccion))
                        {
                            comandoSql.CommandType = CommandType.StoredProcedure;
                            comandoSql.Parameters.AddWithValue("@ID_Proveedor", proveedor.IDProveedor);

                            var resultadoParam = new SqlParameter("@Resultado", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };
                            comandoSql.Parameters.Add(resultadoParam);

                            await comandoSql.ExecuteNonQueryAsync();
                            int resultado = (int)resultadoParam.Value;

                            if (resultado == 1)
                            {
                                respuesta = "Proveedor eliminado exitosamente";
                            }
                            else
                            {
                                respuesta = "Error al eliminar el proveedor";
                            }

                            transaccion.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback(); // Hacer rollback en caso de error
                        throw new Exception("Error al eliminar proveedor: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = "Ocurrió un error: " + ex.Message; // Manejo de excepciones
            }

            return respuesta;
        }
        #endregion


        #region MetodoMostrar
        public async Task<DataTable> MostrarAsync()
        {
            var resultadoTabla = new DataTable("Proveedor");

            try
            {
                using (var conexionSql = await Utilidades.ObtenerConexionAsync())
                {
                    if (conexionSql == null || conexionSql.State != ConnectionState.Open)
                    {
                        throw new InvalidOperationException("No se pudo establecer la conexión a la base de datos.");
                    }

                    string consultaSql = "SELECT Nombre, Telefono, Direccion FROM Proveedor";
                    using (var comandoSql = new SqlCommand(consultaSql, conexionSql))
                    {
                        using (var sqlDat = new SqlDataAdapter(comandoSql))
                        {
                            await Task.Run(() => sqlDat.Fill(resultadoTabla)); // Llena la tabla con los datos obtenidos de la consulta
                        }
                    }
                }
            }
            catch (Exception)
            {
                resultadoTabla = null; // Retorna null si ocurre un error
            }

            return resultadoTabla; // Devuelve la tabla con los resultados
        }
        #endregion

        #region MetodoBuscarNombre
        public async Task<DataTable> BuscarNombreAsync(string nombre)
        {
            var resultadoTabla = new DataTable("Proveedor");

            try
            {
                using (var conexionSql = await Utilidades.ObtenerConexionAsync())
                {
                    if (conexionSql == null)
                    {
                        throw new InvalidOperationException("No se pudo establecer la conexión a la base de datos.");
                    }

                    string consultaSql = "SELECT ID_Proveedor, Nombre, Telefono, Dirección FROM Proveedor WHERE Nombre LIKE @textobuscar + '%'";
                    using (var comandoSql = new SqlCommand(consultaSql, conexionSql))
                    {
                        comandoSql.Parameters.Add("@textobuscar", SqlDbType.VarChar).Value = nombre;

                        using (var sqlDat = new SqlDataAdapter(comandoSql))
                        {
                            await Task.Run(() => sqlDat.Fill(resultadoTabla)); // Llena la tabla con los datos obtenidos de la consulta
                        }
                    }
                }
            }
            catch (Exception)
            {
                resultadoTabla = null; // Retorna null en caso de error
            }

            return resultadoTabla; // Devuelve la tabla con los resultados
        }
        #endregion
    }
}
