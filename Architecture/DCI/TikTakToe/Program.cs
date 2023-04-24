using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("c").AddCookie("c");
builder.Services.AddSingleton<List<GameSeed>>();
builder.Services.AddSingleton<Dictionary<Guid, TikTakToeGame>>();
builder.Services.AddAuthorization();

var app = builder.Build();

app.MapGet("/", (List<GameSeed> games) => games);
app.MapGet("/{id:guid}", (
    Guid id,
    Dictionary<Guid, TikTakToeGame> sessions
) => sessions[id].Board);

app.MapGet("/login",
    () => Results.SignIn(
        new(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) }, "c"))));

app.MapGet("/create", (
    ClaimsPrincipal user,
    List<GameSeed> games
) =>
{
    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
    games.Add(new GameSeed() { Host = userId });
});

app.MapGet("/{id:guid}/join", (
    Guid id,
    ClaimsPrincipal user,
    List<GameSeed> games,
    Dictionary<Guid, TikTakToeGame> sessions
) =>
{
    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
    var game = games.First(x => x.Id == id);
    game.Guest = userId;
    sessions[game.Id] = new TikTakToeGame(game);
    sessions[game.Id].Play();
});

app.MapGet("/{id:guid}/takeTurn/{pos:int}", (
    Guid id,
    int pos,
    ClaimsPrincipal user,
    Dictionary<Guid, TikTakToeGame> sessions
) =>
{
    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
    return sessions[id].Players[userId].TakeTurn(pos);
});

app.Run();

public class GameSeed
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Host { get; set; }
    public string Guest { get; set; }
}

public class TikTakToeGame
{
    public TikTakToeGame(GameSeed seed)
    {
        Board = new BoardRole(new char[9]);
        Players = new Dictionary<string, Player>()
        {
            { seed.Host, new Player('x', this) },
            { seed.Guest, new Player('o', this) },
        };
    }

    public BoardRole Board { get; set; }
    public Dictionary<string, Player> Players { get; }
    public Player Lead { get; set; }
    public SemaphoreSlim FinishTurn = new SemaphoreSlim(0);

    public async Task Play()
    {
        Lead = Players.Values.First();
        do
        {
            await FinishTurn.WaitAsync();
        } while (!Board.HasWinner());
    }

    public void FinishCurrentTurn()
    {
        Lead = Players.Values.First(x => x != Lead);
        FinishTurn.Release();
    }

    public record Player(
        char Symbol,
        TikTakToeGame Ctx
    )
    {
        private readonly SemaphoreSlim Busy = new SemaphoreSlim(1);

        public async Task<string> TakeTurn(int pos)
        {
            if (Ctx.Lead != this) return "not your turn!";

            if (!await Busy.WaitAsync(TimeSpan.Zero)) return "busy";

            try
            {
                var verifyMessage = Ctx.Board.VerifyPosition(pos);
                if (!string.IsNullOrEmpty(verifyMessage)) return verifyMessage;

                Ctx.Board.Mark(pos, Symbol);
                Ctx.FinishCurrentTurn();
                return $"Placed {Symbol}";
            }
            finally
            {
                Busy.Release();
            }
        }
    }

    public record BoardRole(char[] Slots)
    {
        public void Mark(
            int pos,
            char symbol
        ) => Slots[pos - 1] = symbol;

        public string VerifyPosition(int pos)
        {
            if (pos is < 1 or > 9) return "invalid position";
            if (Slots[pos - 1] != default) return "position taken";
            return "";
        }

        public bool HasWinner() => false;
    }
}