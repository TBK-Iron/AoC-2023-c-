using System.Numerics;

class Day8_2 {

    static long LCM(List<int> numbers) {
        long result = numbers[0];
        for (int i = 1; i < numbers.Count; i++)
        {
            result = LCM(result, numbers[i]);
        }
        return result;
    }

    static long LCM(long a, int b) {
        return (long)(a / BigInteger.GreatestCommonDivisor(a, b) * b);
    }

    public static void Run(){
        string[] lines = File.ReadAllLines(@"../../../src/inputs/input8.txt");

        char[] instructions = lines[0].ToCharArray();
        Dictionary<string, Pair> mappings = new Dictionary<string, Pair>();

        for(int i = 2; i < lines.Length; i++){
            string[] split = lines[i].Replace(",", "").Split(" ");
            string left = split[2].Replace("(", "");
            string right = split[3].Replace(")", "");
            mappings[split[0]] = new Pair(left, right);
        }

        

        

        List<string> startNodes = new List<string>();
        List<string> endingNodes = new List<string>();

        foreach(string node in mappings.Keys){
            if(node.EndsWith("A")){
                startNodes.Add(node);
            } else if(node.EndsWith("Z")){
                endingNodes.Add(node);
            }
        }

        List<Path> paths = new List<Path>();

        for(int i = 0; i < startNodes.Count; i++){
            for(int j = 0; j < endingNodes.Count; j++){
                string currentNode = startNodes[i];

                int steps = 0;
                int iIndex = 0;
                while(!currentNode.Equals(endingNodes[j])){      
                    if(instructions[iIndex] == 'L'){
                        currentNode = mappings[currentNode].left;
                    } else {
                        currentNode = mappings[currentNode].right;
                    }

                    iIndex = (iIndex + 1) % instructions.Length;
                    steps++;
                    if(steps > lines.Length * instructions.Length){
                        break;
                    }
                }
                paths.Add(new Path(startNodes[i], endingNodes[j], steps));
            }  
        }

        List<int> stepCounts = new List<int>();
        for(int i = 0; i < paths.Count; i++){
            if(paths[i].steps < lines.Length * instructions.Length){
                stepCounts.Add(paths[i].steps);
            }
        }

        foreach(Path path in paths){
            if(path.steps < lines.Length * instructions.Length){
                Console.WriteLine(path);
            }
            
        }

        Console.WriteLine(LCM(stepCounts));
    }

    record Pair(string left, string right);

    record Path(string start, string end, int steps);
}