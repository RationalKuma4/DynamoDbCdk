using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using Constructs;

namespace DynamoDbCdk.Stacks;

internal class DynamoStack : Stack
{
    internal DynamoStack(Construct scope, string id, string tableName, string baseName, IStackProps? props = null)
        : base(scope, id, props)
    {
        _ = new Table(this, $"{baseName}{tableName}", new TableProps
        {
            TableName = $"{baseName}{tableName}",
            PartitionKey = new Amazon.CDK.AWS.DynamoDB.Attribute
            {
                Name = "id",
                Type = AttributeType.STRING
            },
            SortKey = new Amazon.CDK.AWS.DynamoDB.Attribute
            {
                Name = "category",
                Type = AttributeType.STRING
            },
            BillingMode = BillingMode.PAY_PER_REQUEST,
            RemovalPolicy = RemovalPolicy.DESTROY
        });
    }
}