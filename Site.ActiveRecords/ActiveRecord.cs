using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.ActiveRecords
{
    public static class ActiveRecord
    {
        public static string Insert<T>(T entity) where T : class
        {
            var t = entity.GetType();
            var columns = string.Empty;
            var values = string.Empty;

            var nomeTabela = (Tabela)Attribute.GetCustomAttribute(t, typeof(Tabela));

            foreach (var item in t.GetProperties().Where(item => item.Name != "Id"))
            {
                var campo = (Campo)Attribute.GetCustomAttribute(item, typeof(Campo));

                columns += string.Format(@"{0},", campo.Name);
                var property = t.GetProperty(item.Name);
                var i = property.GetValue(entity);
                values += string.Format(@"'{0}',", i);
            }

            columns = columns.Remove(columns.Count() - 1);
            values = values.Remove(values.Count() - 1);
            var query = string.Format(@"INSERT INTO {0} ({1}) VALUES ({2});SELECT LAST_INSERT_ID() AS LASTID;", nomeTabela.Name, columns, values);

            return query;
        }

        public static string QueryInsert<T>(this T entity) where T : class
        {
            return Insert(entity);
        }

        public static string Update<T>(T entity) where T : class
        {
            var t = entity.GetType();
            var columns = string.Empty;

            var nomeTabela = (Tabela)Attribute.GetCustomAttribute(t, typeof(Tabela));

            foreach (var item in t.GetProperties().Where(item => item.Name != "Id"))
            {
                var campo = (Campo)Attribute.GetCustomAttribute(item, typeof(Campo));

                var property = t.GetProperty(item.Name);
                var i = property.GetValue(entity);
                columns += string.Format(@"{0} = '{1}',", campo.Name, i);
            }

            columns = columns.Remove(columns.Length - 1);
            var identity = t.GetProperty("Id");
            var id = identity.GetValue(entity);
            var query = string.Format(@"UPDATE {0} SET {1} WHERE Id = {2}", nomeTabela.Name, columns, id);

            return query;
        }

        public static string Delete<T>(T entity) where T : class
        {
            var t = entity.GetType();
            var identity = t.GetProperty("Id");
            var id = identity.GetValue(entity);

            var nomeTabela = (Tabela)Attribute.GetCustomAttribute(t, typeof(Tabela));

            var query = string.Format(@"DELETE FROM {0} WHERE Id = {1}", nomeTabela.Name, id);

            return query;
        }

        public static string Select<T>() where T : class
        {
            var t = typeof(T);
            var nomeTabela = (Tabela)Attribute.GetCustomAttribute(t, typeof(Tabela));

            var query = string.Format(@"SELECT * FROM {0}", nomeTabela.Name);

            return query;
        }

        public static string SelectById<T>(int id) where T : class
        {
            var t = typeof(T);
            var nomeTabela = (Tabela)Attribute.GetCustomAttribute(t, typeof(Tabela));

            var query = string.Format(@"SELECT * FROM {0} WHERE Id = {1}", nomeTabela.Name, id);

            return query;
        }

        public static string DefaultSelect<T>() where T : class
        {
            var t = typeof(T);
            var columns = string.Empty;
            var tableName = (Tabela)Attribute.GetCustomAttribute(t, typeof(Tabela));

            foreach (var item in t.GetProperties())
            {
                var campo = (Campo)Attribute.GetCustomAttribute(item, typeof(Campo));
                columns += string.Format(@"{0} as {1},", tableName.Name + '.' + campo.Name, tableName.Name + '_' + campo.Name);
            }

            columns = columns.Remove(columns.Length - 1);
            var query = string.Format(@"SELECT {0} FROM {1}", columns, tableName.Name);

            return query;
        }

        public static string DefaultSelect<T>(int id) where T : class
        {
            var t = typeof(T);
            var columns = string.Empty;
            var tableName = (Tabela)Attribute.GetCustomAttribute(t, typeof(Tabela));

            foreach (var item in t.GetProperties())
            {
                var campo = (Campo)Attribute.GetCustomAttribute(item, typeof(Campo));
                columns += string.Format(@"{0} as {1},", tableName.Name + '.' + campo.Name, tableName.Name + '_' + campo.Name);
            }

            columns = columns.Remove(columns.Length - 1);
            var query = string.Format(@"SELECT {0} FROM {1} WHERE id = {2}", columns, tableName.Name, id);

            return query;
        }

        public static string Fields<T>() where T : class
        {
            var t = typeof(T);
            var columns = string.Empty;
            var tableName = (Tabela)Attribute.GetCustomAttribute(t, typeof(Tabela));

            foreach (var item in t.GetProperties())
            {
                var campo = (Campo)Attribute.GetCustomAttribute(item, typeof(Campo));
                columns += string.Format(@"{0} as {1},", tableName.Name + '.' + campo.Name, tableName.Name + '_' + campo.Name);
            }

            columns = columns.Remove(columns.Length - 1);

            return columns;
        }
    }
}
