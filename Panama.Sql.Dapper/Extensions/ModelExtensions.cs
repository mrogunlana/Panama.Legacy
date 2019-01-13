using Panama.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Panama.Sql.Dapper
{
    public static class ModelExtensions
    {
        /// <summary>
        /// Converts generic List/Enumerable of IModel to DataTable
        /// Reference: https://stackoverflow.com/questions/564366/convert-generic-list-enumerable-to-datatable
        /// </summary>
        /// <typeparam name="T">IModel</typeparam>
        /// <param name="data">Models</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this IList<T> data) where T: class, IModel
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
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
}
