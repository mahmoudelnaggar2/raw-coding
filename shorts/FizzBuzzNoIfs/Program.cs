
var count = int.Parse(args[0]);
var tableIndex = 0;
var table = new Dictionary<int, string>()
{
    {0,""},
    {1,""},
    {2,""},
    {3,"Fizz"},
    {4,""},
    {5,"Buzz"},
    {6,"Fizz"},
    {7,""},
    {8,""},
    {9,"Fizz"},
    {10,"Buzz"},
    {11,""},
    {12,"Fizz"},
    {13,""},
    {14,""},
    {15,"FizzBuzz"},
};

var actions = new Dictionary<int, Action<int, string>>
{
    {0, (_, _) => {}},
    {4, (n, str) => Console.WriteLine($"{n} => {str}")},
    {8, (n, str) => {
        Console.WriteLine($"{n} => {str}");
        tableIndex = 0;
    }},
};

var recurTable = new Dictionary<int, Action<int>>()
{
    {1, (_) => {}}
};

var recur = (int c) =>
{
    var str = table[tableIndex];
    var action = actions[str.Length];
    action(c, str);
    var next = c /count;
    tableIndex++;
    recurTable[next](c + 1);
};

recurTable[0] = recur;
recur(0);