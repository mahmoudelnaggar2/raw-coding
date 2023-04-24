using FormBuilder;
using FormBuilder.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Database>(o => o.UseNpgsql(
    builder.Configuration.GetConnectionString("Default")
));

var app = builder.Setup();

var apiEndpoints = app.MapGroup("/api");

apiEndpoints.MapGet("/form-spec", (Database db) => db.FormSpecs.Include(x => x.Fields).ToListAsync());
apiEndpoints.MapGet("/form-spec/{id:int}", (int id, Database db) => 
    db.FormSpecs.Include(x => x.Fields).FirstOrDefaultAsync(x => x.Id == id));
apiEndpoints.MapPost("/form-spec", CreateFormSpecification.Handle);
apiEndpoints.MapGet("/form", (Database db) => db.FormSubmissions.ToListAsync());
apiEndpoints.MapPost("/form", SubmitForm.Handle);

app.Run();