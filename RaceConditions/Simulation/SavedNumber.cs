public class SavedNumber
{
    private readonly string _name = "number";

    public SavedNumber()
    {
        File.WriteAllText(_name, "0");
    }

    public async Task IncrementAsync()
    {
        var text = await File.ReadAllTextAsync(_name);
        var number = int.Parse(text);
        number++;
        await File.WriteAllTextAsync(_name, number.ToString());
    }
}