<Query Kind="Program" />

void Main()
{
	var list = new ImmutableList<string>();
	var alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
	foreach (var l in alphabet)
	{
		list = list.Add(l.ToString());
	}
	var j = 0;
	foreach (var l in alphabet.Reverse())
	{
		list = list.Replace(l.ToString(), j++);
	}

	list.Dump();

	for (int i = 0; i < list.Count; i++)
	{
		list.Get(i).Dump();
	}
}

public class ImmutableList<T>
{
	private int _depth;

	public ImmutableList()
	{

	}

	private ImmutableList(INode<T> root, int count, int depth)
	{
		Root = root;
		Count = count;
		_depth = depth;
	}

	public INode<T> Root { get; }
	public int Count { get; private set; }

	public ImmutableList<T> Add(T value)
	{
		if (Count == 0)
		{
			return new ImmutableList<T>(new ValueNode<T>(value, default), 1, 0);
		}

		var bits = new BitArray(new int[] { Count });
		var depth = GetDepth(bits);

		if (_depth < depth)
		{
			return new ImmutableList<T>(
				new PathNode<T>(Root, INode<T>.Of(value, _depth)),
				Count + 1,
				depth
			);
		}

		return new ImmutableList<T>(Root.Add(value, bits, _depth), Count + 1, _depth);
	}

	public T Get(int index)
	{
		var bits = new BitArray(new int[] { index });

		return Root.Get(bits, _depth);
	}

	public ImmutableList<T> Replace(T value, int index)
	{
		var bits = new BitArray(new int[] { index });
		return new ImmutableList<T>(
			Root.Replace(value, bits, _depth),
			Count,
			_depth
		);
	}

	private static int GetDepth(BitArray bits)
	{
		for (int i = bits.Length - 1; i >= 0; i--)
		{
			if (bits.Get(i))
			{
				return i;
			}
		}
		return 0;
	}
}

public interface INode<T>
{
	INode<T> Add(T value, BitArray path, int depth);
	INode<T> Replace(T value, BitArray path, int depth);

	T Get(BitArray path, int depth);

	static INode<T> Of(T value, int depth)
	{
		if (depth == 0)
		{
			return new ValueNode<T>(value, default);
		}

		return new PathNode<T>(Of(value, depth - 1), null);
	}
}

public class ValueNode<T> : INode<T>
{
	public ValueNode(T left, T right)
	{
		LeftValue = left;
		RightValue = right;
	}

	public T LeftValue { get; }
	public T RightValue { get; }

	public INode<T> Add(T value, BitArray path, int depth)
	{
		return path[depth] ? new ValueNode<T>(LeftValue, value) : new ValueNode<T>(value, default);
	}

	public INode<T> Replace(T value, BitArray path, int depth)
	{
		return path[depth] ? new ValueNode<T>(LeftValue, value) : new ValueNode<T>(value, RightValue);
	}

	public T Get(BitArray path, int depth)
	{
		return path[depth] ? RightValue : LeftValue;
	}
}

public class PathNode<T> : INode<T>
{	
	public PathNode(INode<T> left, INode<T> right)
	{
		Left = left;
		Right = right;
	}
	public INode<T> Left{ get; }
	public INode<T> Right{ get; }

	public INode<T> Add(T value, BitArray path, int depth)
	{
		if (path[depth])
		{
			if (Right == null)
			{
				return new PathNode<T>(Left, INode<T>.Of(value, depth - 1));
			}

			return new PathNode<T>(Left, Right.Add(value, path, depth - 1));
		}

		if (Left == null)
		{
			return new PathNode<T>(INode<T>.Of(value, depth - 1), null);
		}

		return new PathNode<T>(Left.Add(value, path, depth - 1), Right);
	}

	public INode<T> Replace(T value, BitArray path, int depth)
	{
		if (path[depth])
		{
			return new PathNode<T>(Left, Right.Replace(value, path, depth - 1));
		}

		return new PathNode<T>(Left.Replace(value, path, depth - 1), Right);
	}

	public T Get(BitArray path, int depth)
	{
		return (path[depth] ? Right : Left).Get(path, depth - 1);
	}
}









