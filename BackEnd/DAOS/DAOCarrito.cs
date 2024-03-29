﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using BackEnd.MODELOS;
using System.Data;


namespace BackEnd.DAOS
{
    public class DAOCarrito
    {
     
        /// <summary>
        /// Usaremos el metodo de eliminar para poder quitar elementos de nuestro carrito
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool delete(int id)
        {
            ConexionMySQL conexion = new ConexionMySQL();
            try
            {
                String SQL = "DELETE FROM carrito WHERE" + " idCarrito= '" + id + "';";
                MySqlCommand sqlcom = new MySqlCommand();
                sqlcom.CommandText = SQL;
                conexion.EjecutaSQLComando(sqlcom);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Obtendremos un carrito en base al usuario actual para traer todos los productos que ha escogido
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Carritos> getOne(int id)
        {
            ConexionMySQL con = null;
            try
            {
                List<Carritos> lista = new List<Carritos>();
                 con = new ConexionMySQL();

                DataSet dat = con.LLenaComboGrid("select c.idUsuario,dp.idproducto,c.NombreProd,c.FechaCompra,dp.precio," +
                    "dp.cantidad, dp.precio*Cantidad as total "+
                    "from carrito c join detalleproducto dp on c.idcarrito=dp.idcarrito"+
                    " where idusuario=" + id + " group by idproducto;");
                DataTable dt = dat.Tables[0];
                Carritos datos;
                foreach (DataRow r in dt.Rows)
                {
                    datos = new Carritos();
                    datos.idUsuario = (int)r.ItemArray[0];
                    datos.idProducto = (int)r.ItemArray[1];
                    datos.NombreProducto = (String)r.ItemArray[2];
                    datos.FechaCompra = (DateTime)r.ItemArray[3];
                    datos.Precio = (decimal)r.ItemArray[4];
                    datos.cantidad = (int)r.ItemArray[5];
                    datos.Total = (decimal)r.ItemArray[6];

                    lista.Add(datos);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo traer el carrito");
            }
        }

        /// <summary>
        /// Utilizaremos el metodo agregar para cada vez que el usuario agregue un producto al carrito desde la 
        /// pagina principal
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public int agregar(Carritos m)
        {
            MySqlConnection conexion = null;
            int afectados = 0;
            try
            {
                 conexion = new MySqlConnection(new ConexionMySQL().GetConnectionString());
                conexion.Open();
                String consulta = "INSERT INTO carrito "
                    + "VALUES (default,@idUsuario,@Nombreproducto,now())";
                MySqlCommand comando = new MySqlCommand();
                comando.Connection = conexion;
                comando.CommandText = consulta;
                comando.CommandType = System.Data.CommandType.Text;
                comando.Parameters.AddWithValue("@idUsuario", m.idUsuario);
                comando.Parameters.AddWithValue("@Nombreproducto", m.NombreProducto);
                afectados = comando.ExecuteNonQuery();
               
                return afectados;

            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo registrar el producto en carrito");
                return 0;
            }
            finally
            {
                if (afectados != 0)
                {
                    conexion.Close();
                }
            }

        }
        /// <summary>
        /// se utilizara este metodo para eliminar todo lo que se encuentra en el carrito para la proxima compra
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool deleteAll(int id)
        {
            ConexionMySQL conexion = new ConexionMySQL();
            try
            {
                String SQL = "DELETE FROM carrito WHERE" + " idusuario= " + id + ";";
                MySqlCommand sqlcom = new MySqlCommand();
                sqlcom.CommandText = SQL;
                conexion.EjecutaSQLComando(sqlcom);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Error al eliminar carrito");
            }

        }

    }

}
