using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.DTO.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication
{
    public static class InitSerilog
    {
        public static async Task InitializeAsync(IServiceProvider services, IConfiguration Configuration)
        {
            const int LIMIT_ROWS = 9999;
            string connectionstring = Configuration.GetConnectionString("AdminLogDbConnection");
            string tableName = "logs";
            //Used columns (Key is a column name) 
            //Column type is writer's constructor parameter
            IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {"time_stamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
                {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                {"log_event", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
                {"properties", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) }
            };
            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                                .Enrich.WithThreadId()
                                .Enrich.FromLogContext()
                                .WriteTo.PostgreSQL(connectionstring, tableName, columnWriters, schemaName: "logging")
                                .CreateLogger();

            var databaseDbContext = services.GetRequiredService<AdminLogDbContext>();

            if (await databaseDbContext.Logs.CountAsync() == LIMIT_ROWS)
            {
                // export to excel, csv file logs_backup_dd_MM_yyyy_hh_mm_ss_ms
                ExportToCsv(databaseDbContext);
                // clear all data
                string sqlCommand = "TRUNCATE TABLE logging.logs RESTART IDENTITY;";
                await databaseDbContext.Database.ExecuteSqlRawAsync(sqlCommand);
            }
        }

        private static void ExportToCsv(AdminLogDbContext databaseDbContext)
        {
            Log.Fatal("Export to csv" + DateTime.Today.Ticks);
            //var resultLogs = databaseDbContext.Logs.Select(log =>
            //    new LogDto()
            //    {
            //        Id = log.Id,
            //        Level = log.Level,
            //        Exception = log.Exception,
            //        LogEvent = log.LogEvent,
            //        Message = log.Message,
            //        MessageTemplate = log.MessageTemplate,
            //        Properties = log.Properties,
            //        TimeStamp = log.TimeStamp,
            //    }
            //).Skip(0).Take(99999);

            //var cc = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
            //using (var ms = new MemoryStream())
            //{
            //    using (var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true)))
            //    {
            //        using (var cw = new CsvWriter(sw, cc))
            //        {
            //            cw.WriteRecords(YourGenericList);
            //        }
            //        // The stream gets flushed here.
            //        File(ms.ToArray(), "text/csv", $"export_{DateTime.UtcNow.Ticks}.csv");
                    
            //    }
            //}
        }
    }
}
