using Amazon.CDK;
using DynamoDbCdk.Stacks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using AWSEnvironment = Amazon.CDK.Environment;

namespace DynamoDbCdk;

internal static class Program
{
    private static string? _serviceName;
    private static string? _tableName;

    private static async Task Main(string[] args)
    {
        _ = CreateHostBuilder(args).Build();
        var baseName = $"{_serviceName}-";
        var serviceTags = new Dictionary<string, string>
        {
            ["service"] = _serviceName
        };

        var app = new App();
        var env = new AWSEnvironment { Region = "us-east-1" };

        _ = new DynamoStack(app, $"{baseName}dynamo", _tableName, baseName, new StackProps
        {
            StackName = $"{baseName}dynamo",
            Description = $"Creates {_serviceName} dynamo table",
            Tags = serviceTags,
            Env = env
        });

        app.Synth();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        var hostBuilder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.Sources.Clear();
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddConfiguration(configuration);
            });

        _serviceName = configuration.GetValue<string>("DeploymentSettings:ServiceName");
        _tableName = configuration.GetValue<string>("DeploymentSettings:TableName");
        return hostBuilder;
    }
}