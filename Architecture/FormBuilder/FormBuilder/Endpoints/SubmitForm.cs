namespace FormBuilder.Endpoints;

public class SubmitForm
{
    public static async Task<IResult> Handle(
        int specId,
        HttpContext ctx,
        Database db
    )
    {
        string form = "";
        using (var sr = new StreamReader(ctx.Request.Body))
        {
            form = await sr.ReadToEndAsync();
        }

        if (string.IsNullOrEmpty(form))
        {
            return Results.BadRequest();
        }

        db.FormSubmissions.Add(new FormSubmission()
        {
            FormSpecId = specId,
            Payload = form
        });
        await db.SaveChangesAsync();
        return Results.Ok();
    }
}