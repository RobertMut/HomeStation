﻿using HomeStation.Application.Common.Enums;
using HomeStation.Application.Common.Interfaces;
using HomeStation.Application.Common.Options;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace HomeStation.Infrastructure.Helpers;

public class DatabaseHelper //todo refactor in future
{
    private const string MySqlCheck = @"SELECT count(*)
                                    FROM information_schema.tables
                                    WHERE table_name LIKE '%Climate%'";

    private const string SqlServerCheck = @"SELECT IIF(COUNT(TABLE_NAME) = 0, 0, 1)
                                            FROM   INFORMATION_SCHEMA.TABLES
                                            WHERE  TABLE_NAME LIKE '%Climate%'";
    
    public static async Task InitDatabase(DatabaseOptions options, IServiceCollection serviceCollection)
    {
        int count = 0;
        IAirDbContext dbContext = serviceCollection.BuildServiceProvider().GetRequiredService<IAirDbContext>();
        switch (options.DatabaseType)
        {
            case DatabaseType.MySql:
                try
                {
                    await using (var connection = new MySqlConnection
                                 {
                                     ConnectionString = options.ConnectionString
                                 })
                    await using (MySqlCommand sqlCommand = new MySqlCommand(MySqlCheck, connection))
                    {
                        await connection.OpenAsync();
                        count = Convert.ToInt32(sqlCommand.ExecuteScalar());

                        await connection.CloseAsync();
                    }
                }
                catch (InvalidOperationException)
                {
                    count = 0;
                }

                await Migrate(count, dbContext);
                
                break;
            
            case DatabaseType.SqlServer:

                try
                {
                    await using (SqlConnection connection = new SqlConnection(options.ConnectionString))
                    await using (SqlCommand command = new SqlCommand(SqlServerCheck, connection))
                    {
                        await connection.OpenAsync();
                        count = Convert.ToInt32(command.ExecuteScalar());
                        await connection.CloseAsync();
                    }
                }
                catch (SqlException)
                {
                    count = 0;
                }

                await Migrate(count, dbContext);
                
                break;
            
            case DatabaseType.PostgreSql:
                throw new NotImplementedException();
                break;
            
        }
    }

    private static async Task Migrate(int count, IAirDbContext dbContext)
    {
        if (count == 0)
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}