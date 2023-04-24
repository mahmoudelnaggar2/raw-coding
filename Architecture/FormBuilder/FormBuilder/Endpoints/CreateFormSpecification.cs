namespace FormBuilder.Endpoints;

public class CreateFormSpecification
{
    public static async Task<IResult> Handle(
        FormSpec spec,
        Database db
        )
    {
        db.Add(spec);
        await db.SaveChangesAsync();
        return Results.Ok();
    }
}