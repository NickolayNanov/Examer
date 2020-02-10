namespace OnlineExamer.Data
{
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.EntityFrameworkCore.Migrations.Operations;

    internal class CustomSqlServerMigrationsSqlGenerator : SqlServerMigrationsSqlGenerator
    {
        internal const string DatabaseCollationName = "Cyrillic_General_CS_AI";

        public CustomSqlServerMigrationsSqlGenerator(
            MigrationsSqlGeneratorDependencies dependencies,
            IMigrationsAnnotationProvider migrationsAnnotations)
        : base(dependencies, migrationsAnnotations)
        {
        }

        protected override void Generate(SqlServerCreateDatabaseOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            base.Generate(operation, model, builder);

            if (DatabaseCollationName != null)
            {
                builder
                    .Append("ALTER DATABASE ")
                    .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name))
                    .Append(" COLLATE ")
                    .Append(DatabaseCollationName)
                    .AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator)
                    .EndCommand(suppressTransaction: true);
            }
        }
    }
}
