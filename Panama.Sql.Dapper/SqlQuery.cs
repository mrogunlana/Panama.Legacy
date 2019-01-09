using Dapper;
using Dapper.Contrib.Extensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using Newtonsoft.Json;
using Panama.Entities;
using Panama.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Panama.Sql.Dapper
{
    public class SqlQuery : IQuery
    {
        private ILog _log;
        private ISqlGenerator _sql;

        public SqlQuery(ILog log)
        {
            _log = log;
            _sql = new SqlGeneratorImpl(new DapperExtensions.DapperExtensionsConfiguration());
        }
        public List<T> Get<T>(string sql, object parameters)
        {
            var result = new List<T>();

            using (var connection = new SqlConnection(ConfigurationManager.AppSettings["Database"]))
            {
                _log.LogTrace<SqlQuery>($"SELECT: {sql}. Parameters: {JsonConvert.SerializeObject(parameters)}");

                connection.Open();

                result = connection.Query<T>(sql, parameters).ToList();

                connection.Close();
            }

            return result.ToList();
        }

        public T GetSingle<T>(string sql, object parameters)
        {
            return Get<T>(sql, parameters).FirstOrDefault();
        }

        public void Insert<T>(T obj) where T : class
        {
            using (var connection = new SqlConnection(ConfigurationManager.AppSettings["Database"]))
            {
                connection.Open();
                connection.Insert(obj);
                connection.Close();
            }
        }

        public void Update<T>(T obj) where T : class
        {
            using (var connection = new SqlConnection(ConfigurationManager.AppSettings["Database"]))
            {
                connection.Open();
                connection.Update(obj);
                connection.Close();
            }
        }

        public void Save<T>(T obj, object parameters) where T : class, IModel
        {
            var properties = string.Join(" AND ", parameters.GetType().GetProperties().Select(x => $"{x.Name} = @{x.Name}"));
            var exist = Get<T>($"select * from [{ _sql.Configuration.GetMap<T>().TableName }] where {properties}", parameters);
            if (exist.Count == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public bool Exist<T>(string sql, object parameters) where T : class, IModel
        {
            var exist = Get<T>(sql, parameters);
            if (exist.Count == 0)
                return false;

            return true;
        }

        public void Delete<T>(T obj) where T : class, IModel
        {
            using (var connection = new SqlConnection(ConfigurationManager.AppSettings["Database"]))
            {
                connection.Open();
                connection.Delete(obj);
                connection.Close();
            }
        }

        public void Execute(string sql, object parameters)
        {
            using (var connection = new SqlConnection(ConfigurationManager.AppSettings["Database"]))
            {
                _log.LogTrace<SqlQuery>($"EXECUTE: {sql}. Parameters: {JsonConvert.SerializeObject(parameters)}");

                connection.Open();
                connection.Execute(sql, parameters);
                connection.Close();
            }
        }

        public void InsertBatch<T>(List<T> models) where T : class, IModel
        {
            using (var connection = new SqlConnection(ConfigurationManager.AppSettings["Database"]))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var map = _sql?.Configuration?.GetMap<T>();
                        if (map == null)
                            throw new Exception($"Class Map for:{typeof(T).Name} could not be found.");

                        var name = map.TableName;
                        var table = models.ToDataTable();
                        var timer = Stopwatch.StartNew();

                        timer.Start();

                        using (var bulk = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulk.DestinationTableName = name;
                            bulk.WriteToServer(table);
                        }

                        transaction.Commit();

                        _log.LogDebug<SqlQuery>($"bulk insert complete in: {timer.Elapsed.ToString(@"hh\:mm\:ss\:fff")}");
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();

                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
