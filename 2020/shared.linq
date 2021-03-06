<Query Kind="Program" />

public static class Utils
{
	public static string[] ParseStrings(string file, bool removeEmpty = false)
	{
		Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
        var split = removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
		var strings = File.ReadAllText(file).Split("\r\n", split).ToArray();
		return strings;
	}
	
	public static int[] ParseInts(string file)
	{
		Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
		var ints = File.ReadAllText(file).Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray();
		return ints;
	}

	public static long[] ParseLongs(string file)
	{
		Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
		var longs = File.ReadAllText(file).Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToArray();
		return longs;
	}
}