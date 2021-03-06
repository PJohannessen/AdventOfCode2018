<Query Kind="Program" />

int width;
int height;
char[,] grid;
void Main()
{
    int minutes = 100;
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("18.txt");
    width = inputLines[0].Length;
    height = inputLines.Length;
    grid = new char[width, height];
    for (int j = 0; j < height; j++)
    {
        for (int i = 0; i < width; i++)
        {
            grid[i, j] = inputLines[j][i];
        }
    }

    for (int m = 1; m <= minutes; m++)
    {
        char[,] newGrid = new char[width, height];
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if ((i == 0 && j == 0) || (i == 0 && j == height-1) || (i == width-1 && j == 0) || (i == width-1 && j == height-1))
                {
                    newGrid[i,j] = '#';
                }
                else if (grid[i, j] == '#')
                {
                    int surroundingOn = GetSurrounding(grid, i, j, '#');
                    if (surroundingOn == 2 || surroundingOn == 3) newGrid[i, j] = '#';
                    else newGrid[i, j] = '.';
                }
                else if (grid[i, j] == '.')
                {
                    int surroundingOn = GetSurrounding(grid, i, j, '#');
                    if (surroundingOn == 3) newGrid[i, j] = '#';
                    else newGrid[i, j] = '.';
                }
            }
        }
        grid = newGrid;
    }

    int lightsOn = 0;
    for (int j = 0; j < height; j++)
    {
        for (int i = 0; i < width; i++)
        {
            if (grid[i, j] == '#') lightsOn++;
        }
    }
    lightsOn.Dump();
}

int GetSurrounding(char[,] grid, int x, int y, char c)
{
    List<char> surrounding = new List<char>();
    if (x - 1 >= 0 && y - 1 >= 0) surrounding.Add(grid[x - 1, y - 1]);
    if (y - 1 >= 0) surrounding.Add(grid[x, y - 1]);
    if (x + 1 < width && y - 1 >= 0) surrounding.Add(grid[x + 1, y - 1]);
    if (x - 1 >= 0) surrounding.Add(grid[x - 1, y]);
    if (x + 1 < width) surrounding.Add(grid[x + 1, y]);
    if (x - 1 >= 0 && y + 1 < height) surrounding.Add(grid[x - 1, y + 1]);
    if (y + 1 < height) surrounding.Add(grid[x, y + 1]);
    if (x + 1 < width && y + 1 < height) surrounding.Add(grid[x + 1, y + 1]);
    return surrounding.Where(c2 => c2 == c).Count();
}

void Print()
{
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            Console.Write(grid[x, y]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}