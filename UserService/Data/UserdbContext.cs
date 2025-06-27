using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using UserService.Model;

namespace UserService.Data
{
    public class UserdbContext : DbContext
    {
        public UserdbContext(DbContextOptions<UserdbContext> options)
      : base(options) { }

        public DbSet<EventPlanner> EventPlanners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        private void AddParametersToDbCommand(string commandText, object[] parameters, System.Data.Common.DbCommand cmd)
        {
            cmd.CommandText = commandText;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;

            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    if (p != null)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
            }
        }

        public EventPlanner ExecStoredProcedure(string commandText,int totalOutputParams, params object[] parameters)
        {
            using var connection = Database.GetDbConnection();
            EventPlanner result = null;

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using var cmd = connection.CreateCommand();
                AddParametersToDbCommand(commandText, parameters, cmd);

                using var reader = cmd.ExecuteReader();
                result = DataReaderMapToList<EventPlanner>(reader).FirstOrDefault();
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        public static IList<T> DataReaderMapToList<T>(IDataReader dr)
        {
            IList<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())     
                {
                    try
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    catch { continue; }
                }
                list.Add(obj);
            }
            return list;
        }



    }
}
