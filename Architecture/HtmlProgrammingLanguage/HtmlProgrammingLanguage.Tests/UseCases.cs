using HtmlProgrammingLanguage.Core;

namespace HtmlProgrammingLanguage.Tests;

public class UseCases
{
    [Fact]
    public void HelloWorld()
    {
        var html = """
<fn name="main">
    <print>Hello World</print>
</fn>
""";

        var output = new List<string>();
        new Interpreter(output.Add, html).Execute(Array.Empty<string>());

        Assert.Equal("Hello World", output[0]);
    }


    [Fact]
    public void Variable()
    {
        var html = """
<fn name="main">
    <var name="my_var">Hello World</var>
    <print><my_var /></print>
</fn>
""";

        var output = new List<string>();
        new Interpreter(output.Add, html).Execute(Array.Empty<string>());

        Assert.Equal("Hello World", output[0]);
    }

    [Theory]
    [InlineData("add", 2, 2, 4)]
    [InlineData("sub", 2, 2, 0)]
    [InlineData("mul", 3, 3, 9)]
    [InlineData("div", 9, 3, 3)]
    [InlineData("mod", 5, 3, 2)]
    public void Arithmetic(
        string op,
        int a,
        int b,
        int expected
    )
    {
        var html = $"""
<fn name="main">
    <var name="a">{a}</var>
    <var name="b">{b}</var>
    <print>
        <{op}><a/><b/></{op}>
    </print>
</fn>
""";

        var output = new List<string>();
        new Interpreter(output.Add, html).Execute(Array.Empty<string>());

        Assert.Equal(expected.ToString(), output[0]);
    }

    [Theory]
    [InlineData("add", 2, 2, 4)]
    [InlineData("sub", 2, 2, 0)]
    [InlineData("mul", 3, 3, 9)]
    [InlineData("div", 9, 3, 3)]
    [InlineData("mod", 5, 3, 2)]
    public void Arithmetic2(
        string op,
        int a,
        int b,
        int expected
    )
    {
        var html = $"""
<fn name="main">
    <var name="a">{a}</var>
    <print>
        <{op}><a/>{b}</{op}>
    </print>
</fn>
""";

        var output = new List<string>();
        new Interpreter(output.Add, html).Execute(Array.Empty<string>());

        Assert.Equal(expected.ToString(), output[0]);
    }

    [Theory]
    [InlineData("add", 2, 2, 4)]
    [InlineData("sub", 2, 2, 0)]
    [InlineData("mul", 3, 3, 9)]
    [InlineData("div", 9, 3, 3)]
    [InlineData("mod", 5, 3, 2)]
    public void Arithmetic3(
        string op,
        int a,
        int b,
        int expected
    )
    {
        var html = $"""
<fn name="main">
    <var name="a"></var>
    <var name="b">{b}</var>
    <print>
        <{op}>{a}<b/></{op}>
    </print>
</fn>
""";

        var output = new List<string>();
        new Interpreter(output.Add, html).Execute(Array.Empty<string>());

        Assert.Equal(expected.ToString(), output[0]);
    }

    [Theory]
    [InlineData("add", 2, 2, 4)]
    [InlineData("sub", 2, 2, 0)]
    [InlineData("mul", 3, 3, 9)]
    [InlineData("div", 9, 3, 3)]
    [InlineData("mod", 5, 3, 2)]
    public void Arithmetic4(
        string op,
        int a,
        int b,
        int expected
    )
    {
        var html = $"""
<fn name="main">
    <print>
        <{op}>
            <val>{a}</val>
            <val>{b}</val>
        </{op}>
    </print>
</fn>
""";

        var output = new List<string>();
        new Interpreter(output.Add, html).Execute(Array.Empty<string>());

        Assert.Equal(expected.ToString(), output[0]);
    }

    [Fact]
    public void Function()
    {
        var html = """
<fn name="main">
    <fn name="my_add" parameters="left, right">
        <add>
            <left />
            <right />
        </add>
    </fn>
    <print>
        <my_add>
            <val>9</val>
            <val>5</val>
        </my_add>
    </print>
</fn>
""";

        var output = new List<string>();
        new Interpreter(output.Add, html).Execute(Array.Empty<string>());

        Assert.Equal("14", output[0]);
    }

    [Fact]
    public void FunctionWithVars()
    {
        var html = """
<fn name="main">
    <fn name="my_add" parameters="left, right">
        <add>
            <left />
            <right />
        </add>
    </fn>
    <var name="a">5</var>
    <var name="b">9</var>
    <print>
        <my_add><a/><b/></my_add>
    </print>
</fn>
""";

        var output = new List<string>();
        new Interpreter(output.Add, html).Execute(Array.Empty<string>());

        Assert.Equal("14", output[0]);
    }

    [Theory]
    [InlineData("<a/>")]
    [InlineData("5")]
    [InlineData("true")]
    [InlineData("<eq><a/>5</eq>")]
    public void IfSatisfied(string equality)
    {
        var html = $"""
<fn name="main">
    <var name="a">5</var>
    <if>
        {equality}
        <print>True</print>
    </if>
</fn>
""";

        var output = new List<string>();
        new Interpreter(output.Add, html).Execute(Array.Empty<string>());

        Assert.Equal("True", output[0]);
    }


    [Fact]
    public void Recur()
    {
        var html = """
<fn name="main">
    <fn name="count" parameters="n">
        <print><n/></print>
        <if>
            <eq><n/>0</eq>
            <return>0</return>
        </if>
        <count>
            <sub><n/>1</sub>
        </count>
    </fn>
    <count>5</count>
</fn>
""";

        var output = new List<string>();
        new Interpreter(output.Add, html).Execute(Array.Empty<string>());

        Assert.Equal("5", output[0]);
        Assert.Equal("4", output[1]);
        Assert.Equal("3", output[2]);
        Assert.Equal("2", output[3]);
        Assert.Equal("1", output[4]);
        Assert.Equal("0", output[5]);
    }

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(2, "1")]
    [InlineData(3, "2")]
    [InlineData(4, "3")]
    [InlineData(5, "5")]
    [InlineData(6, "8")]
    [InlineData(7, "13")]
    [InlineData(8, "21")]
    [InlineData(9, "34")]
    [InlineData(10, "55")]
    public void Fib(int nth, string expected)
    {
        var html = $"""
<fn name="main">
    <fn name="fib" parameters="n">
        <if>
            <eq><n/>0</eq>
            <return>0</return>
        </if>
        <if>
            <eq><n/>1</eq>
            <return>1</return>
        </if>
        <add>
            <fib><sub><n/>1</sub></fib>
            <fib><sub><n/>2</sub></fib>
        </add>
    </fn>
    <print>
        <fib>{nth}</fib>
    </print>
</fn>
""";

        var output = new List<string>();
        new Interpreter(output.Add, html).Execute(Array.Empty<string>());

        Assert.Equal(expected, output[0]);
    }
}