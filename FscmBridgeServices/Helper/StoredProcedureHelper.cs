using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

public static class StoredProcedureHelper
{
   
    public static async Task<T> ExecuteStoredProcedureFirstOrDefaultAsync<T>(
        this DbContext context,
        string storedProcedureName,
        params SqlParameter[] parameters)
        where T : class, new()
    {
        using (var command = context.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;

          
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            await context.Database.OpenConnectionAsync();

            using (var result = await command.ExecuteReaderAsync())
            {
                if (await result.ReadAsync())
                {
                    var entity = new T();
                    
                    MapDataToEntity(result, entity);
                    return entity;
                }
            }
        }

        return null; 
    }

    
    public static async Task<List<T>> ExecuteStoredProcedureListAsync<T>(
        this DbContext context,
        string storedProcedureName,
        params SqlParameter[] parameters)
        where T : class, new()
    {
        if (context == null) throw new ArgumentNullException(nameof(context), "DbContext tidak boleh null.");
        if (string.IsNullOrEmpty(storedProcedureName)) throw new ArgumentException("Stored procedure name tidak boleh kosong.", nameof(storedProcedureName));

        var results = new List<T>();

        using (var command = context.Database.GetDbConnection()?.CreateCommand())
        {
            if (command == null) throw new NullReferenceException("Database connection tidak tersedia. Pastikan DbContext dikonfigurasi dengan benar.");

            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null && parameters.Length > 0)
            {
   
                command.Parameters.AddRange(parameters.Select(p => new SqlParameter(p.ParameterName, p.Value)).ToArray());
            }

            await context.Database.OpenConnectionAsync();

            using (var result = await command.ExecuteReaderAsync())
            {
                while (await result.ReadAsync())
                {
                  
                    var entity = new T();
                    MapDataToEntity(result, entity);
                    results.Add(entity);
                }
            }
        }

        return results.AsQueryable().AsNoTracking().ToList();
    }

    private static void MapDataToEntity<T>(IDataReader reader, T entity)
    {
        var properties = typeof(T).GetProperties();

       
        for (int i = 0; i < reader.FieldCount; i++)
        {
            string columnName = reader.GetName(i).Trim();
            object columnValue = reader.IsDBNull(i) ? null : reader.GetValue(i);

         

            var property = properties.FirstOrDefault(p => p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));

            if (property != null)
            {
              

                if (property.CanWrite)
                {
                    try
                    {
                        Type targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        if (columnValue != null)
                        {
                            columnValue = Convert.ChangeType(columnValue, targetType);
                        }

                        property.SetValue(entity, columnValue);
                       
                    }
                    catch (Exception ex)
                    {
                      
                    }
                }
                else
                {
                 
                }
            }
            else
            {
               
            }
        }
    }



}
