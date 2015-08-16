using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Linq;
using ProjectClassLibrary.Model;
using ProjectClassLibrary.Model.CustomAttributes;
using ProjectClassLibrary.Repository.Interface;
using ProjectClassLibrary.Repository.Utils;

namespace ProjectClassLibrary.Repository.BaseClasses
{

 
    public class BaseSqlRepository<T>:IRepository<T>
    {
        #region Public Properties
        public IConnectionInfo ConnectionDetails{get;set;}
      
        public string ExceptionMessage { get; set; }
         #endregion

        #region Constructors
        protected BaseSqlRepository()
        {
            
        }
        

      
        protected BaseSqlRepository(IConnectionInfo c)
        {
            this.ConnectionDetails = c;
        }
        #endregion

        #region Public Implemented Methods
        public virtual List<T> Get()
        {
            return GetDbResults();
        }

        public virtual void Add(T model)
        {
            var sql=GenerateInsertSql(model);
            ExecuteSqlNonReader(sql);
        }

        public virtual T Update(T model)
        {
            var sql = GenerateUpdateSql(model);
            ExecuteSqlNonReader(sql);
            return model;
        }

        public virtual void Delete(T model)
        {
            var sql = GenerateDeleteSql(model);
            ExecuteSqlNonReader(sql);
            //Console.WriteLine(sql);
        }
       #endregion

        #region Private Methods

        private List<T> GetDbResults()
        {
            var sql = GenerateGetSql();
            var lst = new List<T>();
            var t = typeof(T);
            using (var cn = new SqlConnection(ConnectionDetails.GetConnectionString())) {
                
                try
                {
                    dynamic a = Activator.CreateInstance(t);

                    var p =
                        t.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance |
                                        BindingFlags.NonPublic);

                    cn.Open();
                    var cmd = new SqlCommand() {Connection = cn, CommandText = sql};
                    var resultSet = cmd.ExecuteReader();
                    while (resultSet.Read())
                    {

                        foreach (var propertyInfo in p)
                        {
                            var primarykey = false;
                            var ignoreField = false;

                            var attributes = propertyInfo.GetCustomAttributes(false);
                            foreach (var attribute in attributes)
                            {
                                if (attribute.GetType() == typeof (IgnoreField))
                                {
                                    ignoreField = true;
                                }
                                if (attribute.GetType() == typeof (PrimaryKey))
                                {
                                    primarykey = true;
                                }


                            }
                            if (primarykey || !ignoreField)
                                propertyInfo.SetValue(a, resultSet[propertyInfo.Name]);
                        }

                        lst.Add(a);

                        a = Activator.CreateInstance(t);
                    }
                    a = null;
                }
                catch (InvalidOperationException ex)
                {
                    this.ExceptionMessage = ex.Message;
                }
                catch (SqlException ex)
                {
                    this.ExceptionMessage = ex.Message;
                    var p =
                       t.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance |
                                       BindingFlags.NonPublic);
                    dynamic m = Activator.CreateInstance(t);
                    foreach (var propertyInfo in p.Where(propertyInfo => propertyInfo.Name == "ErrorMessage"))
                    {
                        propertyInfo.SetValue(m, ex.Message);
                    }
                    lst.Add(m);
                }
                catch (ArgumentException ex)
                {
                    this.ExceptionMessage = ex.Message;
                }
                catch (Exception ex)
                {
                    
                    this.ExceptionMessage = ex.Message;
                }
                finally
                {

                    
                   
                }
        }

    return lst;




        }
        private string  GenerateGetSql()
        {
            var t = typeof(T);
            var tableName = t.Name;
            var sb = new StringBuilder();
            try
            {
                PropertyInfo[] propertyInfos =
                    t.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance |
                                    BindingFlags.NonPublic);

                sb.Append("SELECT ");
              
                foreach (var propertyInfo in propertyInfos)
                {
                   

                    var ignoreField = false;
                    var primarykey = false;
                    var attributes = propertyInfo.GetCustomAttributes(false);
                    foreach (var attribute in attributes)
                    {
                        if (attribute.GetType() == typeof(IgnoreField))
                        {
                            ignoreField = true;
                        }
                        if (attribute.GetType() == typeof(PrimaryKey))
                        {
                            primarykey = true;
                        }

                       
                    }
                 
                    if (primarykey || !ignoreField)
                    {
                       
                            sb.Append(propertyInfo.Name + ",");
                        
                    }
                }
                
                sb.Append(" FROM " + tableName);
                
            }
            catch (Exception ex)
            {
                this.ExceptionMessage = ex.Message;
            }
            return sb.Replace(", FROM", " FROM").ToString();
          
        }
        private static string  GenerateInsertSql(T model)
        {
            var t = model.GetType();
            var tableName = t.Name;
            var properties = new StringBuilder("");
            var values = new StringBuilder(" values(");
            var propertyInfos = t.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
            var sb = new StringBuilder();
            sb.Append("Insert Into " + tableName + "(");
          
            foreach (var propertyInfo in propertyInfos)
            {
               
                var ignoreField = false;
                var attributes = propertyInfo.GetCustomAttributes(false);
                foreach (var attribute in attributes.Where(attribute => attribute.GetType() == typeof(IgnoreField)))
                {
                    ignoreField = true;
                }
                if (ignoreField) continue;
                properties.Append(propertyInfo.Name + ",");
                values.Append("'" + propertyInfo.GetValue(model).ToString() + "',");
            }

            properties.Append(")");
            values.Append(")");
            sb.Append(properties);
            sb.Append(values);
            return sb.Replace(",)",")").ToString();
        }
        private static string  GenerateUpdateSql(T model)
        {
            Type t = model.GetType();
            var tableName = t.Name;
            StringBuilder whereClause = new StringBuilder("");
            PropertyInfo[] propertyInfos = t.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
            StringBuilder sb = new StringBuilder("Update " + tableName + " Set ");
          
          

            foreach (var propertyInfo in propertyInfos)
            {
               
                var ignoreField = false;
                var attributes = propertyInfo.GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    if (attribute.GetType() == typeof(IgnoreField))
                    {
                        ignoreField = true;
                    }
                    if (attribute.GetType() == typeof (PrimaryKey))
                    {
                        whereClause.Append(" Where " + propertyInfo.Name + "='" + propertyInfo.GetValue(model)+"'");
                    }
                }
                if (ignoreField) continue;
                sb.Append(propertyInfo.Name+"='" + propertyInfo.GetValue(model).ToString() + "',");
            }
            
            sb.Append(whereClause);
            return sb.Replace(", Where"," Where").ToString();
        }
        private static string  GenerateDeleteSql(T model)
        {
            var t = model.GetType();
            var tableName = t.Name;
            var whereClause = new StringBuilder("");
            var propertyInfos = t.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
            var sb = new StringBuilder("Delete From " + tableName);

            

            foreach (var propertyInfo in from propertyInfo in propertyInfos let attributes = propertyInfo.GetCustomAttributes(false) where attributes.Any(attribute => attribute.GetType() == typeof (PrimaryKey)) select propertyInfo)
            {
                sb.Append(" Where " + propertyInfo.Name + "='" + propertyInfo.GetValue(model) + "'");
            }


            return sb.ToString();
        }
        private int  ExecuteSqlNonReader(string sql)
        {
            var result = 0;
            using (var cn = new SqlConnection(ConnectionDetails.GetConnectionString()))
            {
                try
                {
                    cn.Open();
                    var cmd=new SqlCommand(){Connection = cn,CommandText = sql};
                    result=cmd.ExecuteNonQuery();
                }
                catch (InvalidOperationException ex)
                {
                    this.ExceptionMessage = ex.Message;
                }
                catch (SqlException ex)
                {
                    this.ExceptionMessage = ex.Message;
                }
                catch (ArgumentException ex)
                {
                    this.ExceptionMessage = ex.Message;
                }
                catch (Exception ex)
                {
                    this.ExceptionMessage = ex.Message;
                }
            }
            return result;
        }
      
      
        #endregion
    }
}
