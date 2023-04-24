using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace FormBuilder;

public class Database : DbContext
{
    public Database(DbContextOptions<Database> options)
        : base(options)
    {
    }

    public DbSet<FormSpec> FormSpecs { get; set; }
    public DbSet<FieldSpec> FieldSpecs { get; set; }
    public DbSet<FormSubmission> FormSubmissions { get; set; }
}

public class FormSubmission
{
    public int Id { get; set; }
    public string Payload { get; set; }
    
    public int FormSpecId { get; set; }
    public FormSpec FormSpec { get; set; }
}

public class FormSpec
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<FieldSpec> Fields { get; set; } = new();
}

public class FieldSpec
{
    public int Id { get; set; }
    public string Label { get; set; }
    public string Hint { get; set; }
    public InputType InputType { get; set; }
    public string Metadata { get; set; }
}

public class SelectionMetadata
{
    public List<string> Options { get; set; } = new();

    public static implicit operator string(SelectionMetadata metadata) =>
        JsonSerializer.Serialize(metadata);
    
    public static implicit operator SelectionMetadata(string value) =>
        JsonSerializer.Deserialize<SelectionMetadata>(value);
}

public enum InputType
{
    Text,
    Number,
    Selection,
}