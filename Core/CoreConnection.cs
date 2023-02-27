// ***********************************************************************
// Assembly         : metrogas.api
// Author           : Full Conectados Ltda <software@fullconectados.cl>
// Created          : 09-17-2021
//
// Last Modified By : ginom
// Last Modified On : 11-02-2021
// ***********************************************************************
// <copyright file="CoreConnection.cs" company="Full Conectados Ltda">
//     Copyright (c) 2021. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace metrogas.api.Core
{
    /// <summary>
    /// Clase que crea una conexi�n SqlClient a una base de datos SQL Server
    /// </summary>
    /// <example>
    ///   <code>
    /// DataQuery q = new DataQuery(con); // "con" es la conexi�n a la base
    /// q.OpenCon();
    /// q.ExecuteCommand("delete from tabla");
    /// q.CloseCon();
    /// </code>
    /// </example>
    public class CoreConnection
    {
        /// <summary>
        /// The connection
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; }
        /// <summary>
        /// The con
        /// </summary>
        private SqlConnection con = null;
        /// <summary>
        /// The tran
        /// </summary>
        private SqlTransaction tran = null;
        /// <summary>
        /// The opened
        /// </summary>
        private bool opened = false;

        /// <summary>
        /// The decimal precision
        /// </summary>
        private const byte DECIMAL_PRECISION = 16;
        /// <summary>
        /// The decimal scale
        /// </summary>
        private const byte DECIMAL_SCALE = 4;


        /// <summary>
        /// Se crea una instancia de la clase
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public CoreConnection(IConfiguration configuration)
        {
            ConnectionString = Environment.GetEnvironmentVariable("METROGAS_DATABASE");
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                ConnectionString = configuration.GetConnectionString("DataBase");
            }
            //var useStringConexion = stringCon != null ? stringCon : Configuration.GetConnectionString("ConexionTest");
        }

        /// <summary>
        /// Abre la conexi�n a la base
        /// </summary>
        public void OpenCon()
        {
            OpenCon(false);
        }

        /// <summary>
        /// Abre la conexi�n a la base
        /// </summary>
        /// <param name="f">if set to <c>true</c> [f].</param>
        public void OpenCon(bool f)
        {
            if (!opened)
            {
                con = new SqlConnection(ConnectionString);
                con.Open();
                opened = true;
            }
            if (f)
            {
                tran = con.BeginTransaction();
            }
        }

        /// <summary>
        /// Cierra la conexi�n a la base
        /// </summary>
        public void CloseCon()
        {
            if (con != null) con.Close();
            opened = false;
        }

        /// <summary>
        /// Cierra la conexi�n a la base
        /// </summary>
        /// <param name="f">if set to <c>true</c> [f].</param>
        public void CloseCon(bool f)
        {
            if (tran != null)
            {
                if (f)
                    tran.Commit();
                else
                    tran.Rollback();
                tran = null;
            }
            CloseCon();
        }

        /// <summary>
        /// Ejecuta una consulta SQL
        /// </summary>
        /// <param name="sql">Instrucci�n SQL</param>
        /// <returns>DataSet de resultados</returns>
        public DataSet ExecuteQuery(string sql)
        {
            SqlCommand cmd;
            if (tran != null)
                cmd = new SqlCommand(sql, con, tran);
            else
                cmd = new SqlCommand(sql, con);
            cmd.CommandTimeout = 0;
            return FillAdapter(cmd, "ExecuteQuery");
        }

        /// <summary>
        /// Ejecuta un comando SQL
        /// </summary>
        /// <param name="sql">Instrucci�n SQL</param>
        public void ExecuteCommand(string sql)
        {
            SqlCommand cmd;
            if (tran != null)
                cmd = new SqlCommand(sql, con, tran);
            else
                cmd = new SqlCommand(sql, con);
            cmd.CommandTimeout = 0;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado
        /// </summary>
        /// <param name="procedure">Nombre del procedimiento</param>
        /// <returns>DataSet de resultados</returns>
        public DataSet ExecuteProcedure(string procedure)
        {
            return ExecuteProcedure(procedure, null);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado y retorna el ID
        /// </summary>
        /// <param name="procedure">Nombre del procedimiento</param>
        /// <param name="paramList">Lista de parametros</param>
        /// <param name="field">Campo a retornar</param>
        /// <returns>ID del registro</returns>
        public string ExecuteProcedure(string procedure, List<DataParam> paramList, string field)
        {
            DataSet ds = ExecuteProcedure(procedure, paramList);
            DataTable dt = ds.Tables[0];
            string id = "";
            foreach (DataRow dr in dt.Rows)
            {
                id = dr[field].ToString();
            }
            return id;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado
        /// </summary>
        /// <param name="procedure">Nombre del procedimiento</param>
        /// <param name="paramList">Lista de par�metros</param>
        /// <returns>DataSet de resultados</returns>
        public DataSet ExecuteProcedure(string procedure, List<DataParam> paramList)
        {
            return ExecuteProcedure(procedure, paramList, 120);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado
        /// </summary>
        /// <param name="procedure">Nombre del procedimiento</param>
        /// <param name="paramList">Lista de par�metros</param>
        /// <param name="timeout">Tiempo de espera</param>
        /// <returns>DataSet de resultados</returns>
        public DataSet ExecuteProcedure(string procedure, List<DataParam> paramList, int timeout)
        {
            SqlCommand cmd;
            if (tran != null)
                cmd = new SqlCommand(procedure, con, tran);
            else
                cmd = new SqlCommand(procedure, con);
            cmd.CommandTimeout = timeout;
            cmd.CommandType = CommandType.StoredProcedure;
            if (paramList != null)
            {
                for (int i = 0; i < paramList.Count; i++)
                {
                    DataParam param = paramList[i];
                    SqlDbType type = SqlDbType.VarChar;
                    switch (param.Type)
                    {
                        case DataParam.ParamType.DECIMAL: type = SqlDbType.Decimal; break;
                        case DataParam.ParamType.INT: type = SqlDbType.Int; break;
                        case DataParam.ParamType.BIGINT: type = SqlDbType.BigInt; break;
                        case DataParam.ParamType.TINYINT: type = SqlDbType.TinyInt; break;
                        case DataParam.ParamType.VARCHAR: type = SqlDbType.VarChar; break;
                        case DataParam.ParamType.CHAR: type = SqlDbType.Char; break;
                        case DataParam.ParamType.DATETIME:
                            type = SqlDbType.DateTime;
                            try
                            {
                                if (param.Value != null)
                                {
                                    DateTime dt = (DateTime)param.Value;
                                    // Comparar con DateTime.MinValue no funciona !!!
                                    if (dt.Year == 1 && dt.Month == 1 && dt.Day == 1) param.Value = null;
                                }
                            }
                            catch
                            {
                                param.Value = null;
                            }
                            break;
                        case DataParam.ParamType.SMALLDATETIME:
                            type = SqlDbType.SmallDateTime;
                            try
                            {
                                if (param.Value != null)
                                {
                                    DateTime dt = (DateTime)param.Value;
                                    // Comparar con DateTime.MinValue no funciona !!!
                                    if (dt.Year == 1 && dt.Month == 1 && dt.Day == 1) param.Value = null;
                                }
                            }
                            catch
                            {
                                param.Value = null;
                            }
                            break;
                        case DataParam.ParamType.BIT: type = SqlDbType.Bit; break;
                    }
                    SqlParameter sqlpar = new SqlParameter("@" + param.Name, type);
                    if (type == SqlDbType.Decimal)
                    {
                        sqlpar.Precision = DECIMAL_PRECISION;
                        sqlpar.Scale = DECIMAL_SCALE;
                    }
                    cmd.Parameters.Add(sqlpar).Value = (param.Value ?? DBNull.Value);
                }
            }
            return FillAdapter(cmd, procedure);
        }

        /// <summary>
        /// Fills the adapter.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <param name="table">The table.</param>
        /// <returns>DataSet.</returns>
        private DataSet FillAdapter(SqlCommand cmd, string table)
        {
            SqlDataAdapter data = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            data.Fill(ds, table);

            data.Dispose();
            cmd.Dispose();
            return ds;
        }
        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="field">The field.</param>
        /// <returns>System.Object.</returns>
        protected object GetField(DataRow row, string field)
        {
            return row.Table.Columns.Contains(field) ? row[field] : DBNull.Value;
        }
        /// <summary>
        /// Sets the base model.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="field">The field.</param>
        /// <returns>Models._BaseModel.</returns>
        protected Models._BaseModel SetBaseModel(DataRow row, string field)
        {
            var _id = field + "_id";
            var _nombre = field + "_nombre";
            return new Models._BaseModel
            {
                Id = row.Table.Columns.Contains(_id) ? (row[_id].ValidateInt32() ?? 0) : 
                    (row.Table.Columns.Contains(field) ? (row[field].ValidateInt32() ?? 0) : 
                    0),
                Nombre = row.Table.Columns.Contains(_nombre) ? (row[_nombre].ValidateString() ?? "") : ""
            };
        }

        /// <summary>
        /// Converts to datatable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns>DataTable.</returns>
        public DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }

    /// <summary>
    /// Clase que permite traspasar datos a un procedimiento almacenado o un webservice
    /// </summary>
    /// <example>
    ///   <code>
    /// DataParam p = new DataParam("Nombre", "Juan");
    /// </code>
    /// </example>
    [Serializable]
    public class DataParam
    {
        /// <summary>
        /// Tipo de datos
        /// </summary>
        public enum ParamType
        {
            /// <summary>
            /// The varchar
            /// </summary>
            VARCHAR,
            /// <summary>
            /// The character
            /// </summary>
            CHAR,
            /// <summary>
            /// The datetime
            /// </summary>
            DATETIME,
            /// <summary>
            /// The smalldatetime
            /// </summary>
            SMALLDATETIME,
            /// <summary>
            /// The int
            /// </summary>
            INT,
            /// <summary>
            /// The bigint
            /// </summary>
            BIGINT,
            /// <summary>
            /// The decimal
            /// </summary>
            DECIMAL,
            /// <summary>
            /// The bit
            /// </summary>
            BIT,
            /// <summary>
            /// The tinyint
            /// </summary>
            TINYINT
        }

        /// <summary>
        /// Se crea una nueva instancia del par�metro
        /// </summary>
        public DataParam() { }

        /// <summary>
        /// Se crea una nueva instancia del par�metro
        /// </summary>
        /// <param name="name">Nombre del par�metro</param>
        /// <param name="val">Valor del par�metro</param>
        public DataParam(string name, object val)
        {
            this.name = name;
            this.type = ParamType.VARCHAR;
            this.val = val;
        }

        /// <summary>
        /// Se crea una nueva instancia del par�metro
        /// </summary>
        /// <param name="name">Nombre del par�metro</param>
        /// <param name="type">Tipo del campo</param>
        /// <param name="val">Valor del par�metro</param>
        public DataParam(string name, ParamType type, object val)
        {
            this.name = name;
            this.type = type;
            this.val = val;
        }

        /// <summary>
        /// The name
        /// </summary>
        private string name;
        /// <summary>
        /// Nombre del par�metro
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            set { name = value; }
            get { return name; }
        }
        /// <summary>
        /// The type
        /// </summary>
        private ParamType type = ParamType.VARCHAR;
        /// <summary>
        /// Tipo del par�metro
        /// </summary>
        /// <value>The type.</value>
        public ParamType Type
        {
            set { type = value; }
            get { return type; }
        }
        /// <summary>
        /// The value
        /// </summary>
        private object val;
        /// <summary>
        /// Valor del par�metro
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            set { val = value; }
            get { return val; }
        }
    }
}
