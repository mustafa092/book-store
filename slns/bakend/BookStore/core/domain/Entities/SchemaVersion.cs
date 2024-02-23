namespace persistence;

public class SchemaVersion
{
    public int Id { get; set; }
    public string ScriptName { get; set; } = null!;
    public DateTime Applied { get; set; }
}