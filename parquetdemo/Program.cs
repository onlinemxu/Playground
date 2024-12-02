using Parquet;
using Parquet.Schema;
using Parquet.Serialization;
using System;
using System.Collections.Generic;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello Parquet...");

try
{
    var schema = new ParquetSchema(
        new DataField<int>("Id"),
        new DataField<string>("Client"),
        new DataField<DateTime>("IssueDate")
    );

    var ids = new List<int> { 1, 2, 3 };
    var clients = new List<string> { "abc", "temenos", "xyz" };
    var issueDates = new List<DateTime> { DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2) };

    const string fileName = "client.parquet";

    using (var fs = File.Create(fileName))
    {
        using var writer = await ParquetWriter.CreateAsync(schema, fs);
        using var groupWriter = writer.CreateRowGroup();
        await groupWriter.WriteColumnAsync(new Parquet.Data.DataColumn(schema.DataFields[0], ids.ToArray()));
        await groupWriter.WriteColumnAsync(new Parquet.Data.DataColumn(schema.DataFields[1], clients.ToArray()));
        await groupWriter.WriteColumnAsync(new Parquet.Data.DataColumn(schema.DataFields[2], issueDates.ToArray()));

        Console.WriteLine($"{fs.Name}Parquet file created successfully.");
    };

    using (var fs = File.OpenRead(fileName))
    {
        using var reader = await ParquetReader.CreateAsync(fs);
        for(int i = 0; i < reader.RowGroupCount; i++)
        {
            using var rowGroupReader = reader.OpenRowGroupReader(i);
            foreach(var df in reader.Schema.GetDataFields())
            {
                var columnData =await rowGroupReader.ReadColumnAsync(df);
                
                DisplayColumnData(df, columnData);
            }

        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred:");
    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
}

static void DisplayColumnData(DataField df, Parquet.Data.DataColumn columnData)
{
    Console.WriteLine($"Column: {df.Name}");
    foreach (var value in columnData.Data)
    {
        Console.WriteLine(value);
    }
}