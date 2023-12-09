class Day8_1 {
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

        

        int steps = 0;
        int iIndex = 0;
        string currentValue = "AAA";

        while(!currentValue.Equals("ZZZ")){
            if(instructions[iIndex] == 'L'){
                currentValue = mappings[currentValue].left;
            } else {
                currentValue = mappings[currentValue].right;
            }

            iIndex = (iIndex + 1) % instructions.Length;
            steps++;

        }

        Console.WriteLine(steps);
    }

    record Pair(string left, string right){}
}