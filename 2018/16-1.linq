<Query Kind="Program" />

List<Action<int, int, int>> opcodes = new List<Action<int, int, int>>();
int[] registers;

void Main()
{
    Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
    var inputLines = File.ReadAllLines("16-1.txt");   
    opcodes.Add(addr);
    opcodes.Add(addi);
    opcodes.Add(mulr);
    opcodes.Add(muli);
    opcodes.Add(banr);
    opcodes.Add(bani);
    opcodes.Add(borr);
    opcodes.Add(bori);
    opcodes.Add(setr);
    opcodes.Add(seti);
    opcodes.Add(gtir);
    opcodes.Add(gtri);
    opcodes.Add(gtrr);
    opcodes.Add(eqir);
    opcodes.Add(eqri);
    opcodes.Add(eqrr);

    int totalWithThreeOrMore = 0;
    for (int i = 0; i < inputLines.Length; i = i + 4)
    {
        int matchingOps = 0;
        foreach (var op in opcodes)
        {
            registers = new int[4];
            registers[0] = inputLines[i][9] - 48;
            registers[1] = inputLines[i][12] - 48;
            registers[2] = inputLines[i][15] - 48;
            registers[3] = inputLines[i][18] - 48;

            int[] instructions = inputLines[i + 1].Split(' ').Select(n => int.Parse(n)).ToArray();
            op(instructions[1], instructions[2], instructions[3]);
            
            if (registers[0] == inputLines[i + 2][9] - 48 &&
            registers[1] == inputLines[i + 2][12] - 48 &&
            registers[2] == inputLines[i + 2][15] - 48 &&
            registers[3] == inputLines[i + 2][18] - 48)
            {
                matchingOps++;
            }
        }
        if (matchingOps >= 3) totalWithThreeOrMore++;
    }
    totalWithThreeOrMore.Dump();
}

void addr(int a, int b, int c)
{
    registers[c] = registers[a] + registers[b];
}

void addi(int a, int b, int c)
{
    registers[c] = registers[a] + b;
}

void mulr(int a, int b, int c)
{
    registers[c] = registers[a] * registers[b];
}

void muli(int a, int b, int c)
{
    registers[c] = registers[a] * b;
}

void banr(int a, int b, int c)
{
    registers[c] = registers[a] & registers[b];
}

void bani(int a, int b, int c)
{
    registers[c] = registers[a] & b;
}

void borr(int a, int b, int c)
{
    registers[c] = registers[a] | registers[b];
}

void bori(int a, int b, int c)
{
    registers[c] = registers[a] | b;
}

void setr(int a, int b, int c)
{
    registers[c] = registers[a];
}

void seti(int a, int b, int c)
{
    registers[c] = a;
}

void gtir(int a, int b, int c)
{
    if (a > registers[b]) registers[c] = 1;
    else registers[c] = 0;
}

void gtri(int a, int b, int c)
{
    if (registers[a] > b) registers[c] = 1;
    else registers[c] = 0;
}

void gtrr(int a, int b, int c)
{
    if (registers[a] > registers[b]) registers[c] = 1;
    else registers[c] = 0;
}

void eqir(int a, int b, int c)
{
    if (a == registers[b]) registers[c] = 1;
    else registers[c] = 0;
}

void eqri(int a, int b, int c)
{
    if (registers[a] == b) registers[c] = 1;
    else registers[c] = 0;
}

void eqrr(int a, int b, int c)
{
    if (registers[a] == registers[b]) registers[c] = 1;
    else registers[c] = 0;
}