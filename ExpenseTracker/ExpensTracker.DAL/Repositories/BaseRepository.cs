using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpensTracker.DAL
{
     public abstract class BaseRepository<C, T> :
        IBaseRepository<T>
        where T : class
        where C : DbContext, new()
    {
        private bool disposed;
        protected C Context { get; set; } = new C();

        public BaseRepository()
        {
            //this is to avoid  transaction  of the entity frameworks so that trasaction of the procedure execute and roll back properly.
            Context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
        }
        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = Context.Set<T>();
            return query;
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var query = Context.Set<T>().Where(predicate);
            return query;
        }

        public virtual void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void SaveChanges()
        {
            Context.SaveChanges();
        }


        #region for Reporting

        public IEnumerable<Dictionary<string, object>> GetDynamicData(string procName, Dictionary<object, object> parameters)
        {
            var finalList = new List<Dictionary<string, object>>();
            var datatable = GetPivotDatatable(procName, parameters);
            string type = datatable.Columns[3].DataType.Name;

            var resultset = ConvertToDictionary(GetPivotDatatable(procName, parameters));
            foreach (var emprow in resultset)
            {
                var row = (IDictionary<string, object>)new ExpandoObject();
                Dictionary<string, object> eachRow = (Dictionary<string, object>)emprow;

                foreach (KeyValuePair<string, object> keyValuePair in eachRow)
                {
                    row.Add(keyValuePair);
                }
                finalList.Add(new Dictionary<string, object>(row));
            }
            return finalList.AsEnumerable();

        }
        public virtual DataTable GetPivotDatatable(string procName, Dictionary<object, object> parameters)
        {
            DataTable dt = new DataTable();

            var conn = Context.Database.Connection;
            var connectionState = conn.State;
            try
            {
                using (Context)
                {
                    if (connectionState != ConnectionState.Open)
                        conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = procName;
                        foreach (var item in parameters)
                        {
                            if (item.Value == null)
                                cmd.Parameters.Add(new SqlParameter(item.Key.ToString(), DBNull.Value));
                            else
                                cmd.Parameters.Add(new SqlParameter(item.Key.ToString(), item.Value.ToString()));
                        }

                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                            if (dt.Columns.Contains("Production Cycle1"))
                                dt.Columns.Remove("Production Cycle1");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connectionState == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }

        private List<IDictionary> ConvertToDictionary(DataTable dtObject)
        {
            var columns = dtObject.Columns.Cast<DataColumn>();

            var dictionaryList = dtObject.AsEnumerable()
                .Select(dataRow => columns
                    .Select(column =>
                        new { Column = column.ColumnName, Value = dataRow[column] })
                             .ToDictionary(data => data.Column, data => data.Value)).ToList().ToArray();

            return dictionaryList.ToList<IDictionary>();
        }

        #endregion


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    Context.Dispose();

            disposed = true;
        }
    }
}
