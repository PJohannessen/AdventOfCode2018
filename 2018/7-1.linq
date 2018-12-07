<Query Kind="Program" />

void Main()
{
    string inputString = @"Step X must be finished before step C can begin.;Step C must be finished before step G can begin.;Step F must be finished before step G can begin.;Step U must be finished before step Y can begin.;Step O must be finished before step S can begin.;Step D must be finished before step N can begin.;Step M must be finished before step H can begin.;Step J must be finished before step Q can begin.;Step G must be finished before step R can begin.;Step I must be finished before step N can begin.;Step R must be finished before step K can begin.;Step A must be finished before step Z can begin.;Step Y must be finished before step L can begin.;Step H must be finished before step P can begin.;Step K must be finished before step S can begin.;Step Z must be finished before step P can begin.;Step T must be finished before step S can begin.;Step N must be finished before step P can begin.;Step E must be finished before step S can begin.;Step S must be finished before step W can begin.;Step W must be finished before step V can begin.;Step L must be finished before step V can begin.;Step P must be finished before step B can begin.;Step Q must be finished before step V can begin.;Step B must be finished before step V can begin.;Step P must be finished before step Q can begin.;Step S must be finished before step V can begin.;Step C must be finished before step Q can begin.;Step I must be finished before step H can begin.;Step A must be finished before step E can begin.;Step H must be finished before step Q can begin.;Step G must be finished before step V can begin.;Step N must be finished before step L can begin.;Step R must be finished before step Q can begin.;Step W must be finished before step L can begin.;Step X must be finished before step L can begin.;Step X must be finished before step J can begin.;Step W must be finished before step P can begin.;Step U must be finished before step B can begin.;Step P must be finished before step V can begin.;Step O must be finished before step P can begin.;Step W must be finished before step Q can begin.;Step S must be finished before step Q can begin.;Step U must be finished before step Z can begin.;Step Z must be finished before step T can begin.;Step M must be finished before step T can begin.;Step A must be finished before step P can begin.;Step Z must be finished before step B can begin.;Step N must be finished before step S can begin.;Step H must be finished before step N can begin.;Step J must be finished before step E can begin.;Step M must be finished before step J can begin.;Step R must be finished before step A can begin.;Step A must be finished before step Y can begin.;Step F must be finished before step V can begin.;Step L must be finished before step P can begin.;Step K must be finished before step L can begin.;Step F must be finished before step P can begin.;Step G must be finished before step L can begin.;Step I must be finished before step Q can begin.;Step C must be finished before step L can begin.;Step I must be finished before step Y can begin.;Step G must be finished before step B can begin.;Step H must be finished before step L can begin.;Step X must be finished before step U can begin.;Step I must be finished before step K can begin.;Step R must be finished before step N can begin.;Step I must be finished before step L can begin.;Step M must be finished before step I can begin.;Step K must be finished before step V can begin.;Step G must be finished before step E can begin.;Step F must be finished before step B can begin.;Step O must be finished before step Y can begin.;Step Y must be finished before step Q can begin.;Step F must be finished before step K can begin.;Step N must be finished before step W can begin.;Step O must be finished before step R can begin.;Step N must be finished before step E can begin.;Step M must be finished before step V can begin.;Step H must be finished before step T can begin.;Step Y must be finished before step T can begin.;Step F must be finished before step J can begin.;Step F must be finished before step O can begin.;Step W must be finished before step B can begin.;Step T must be finished before step E can begin.;Step T must be finished before step P can begin.;Step F must be finished before step M can begin.;Step U must be finished before step I can begin.;Step H must be finished before step S can begin.;Step S must be finished before step P can begin.;Step T must be finished before step W can begin.;Step A must be finished before step N can begin.;Step O must be finished before step N can begin.;Step L must be finished before step B can begin.;Step U must be finished before step K can begin.;Step Z must be finished before step W can begin.;Step X must be finished before step D can begin.;Step Z must be finished before step L can begin.;Step I must be finished before step T can begin.;Step O must be finished before step W can begin.;Step I must be finished before step B can begin.";
    
    List<Tuple<char, char>> pairs = new List<System.Tuple<char, char>>();
    foreach (string input in inputString.Split(';'))
    {
        char before = input[5];
        char after = input[36];
        pairs.Add(new Tuple<char, char>(before, after));
    }
    char[] steps = pairs.SelectMany(p => new char[] { p.Item1, p.Item2 }).Distinct().ToArray();
    List<char> remainingSteps = pairs.SelectMany(p => new char[] { p.Item1, p.Item2 }).Distinct().OrderBy(c => c).ToList();
    List<char> answer = new List<char>();
    while (remainingSteps.Count > 0)
    {
        foreach (var remainingStep in remainingSteps)
        {
            if (!pairs.Any(p => p.Item2 == remainingStep && remainingSteps.Contains(p.Item1)))
            {
                answer.Add(remainingStep);
                break;
            }
        }
        remainingSteps = remainingSteps.Where(rs => !answer.Contains(rs)).ToList();
    }
    string.Join("", answer).Dump();
}