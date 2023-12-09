class Day6_1 {
    public static void Run(){
        string[] lines = File.ReadAllLines(@"../../../src/inputs/input6.txt");

        

        int[] times = lines[0].Split(": ")[1].Split(" ").Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToArray();
        int[] distances = lines[1].Split(": ")[1].Split(" ").Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToArray();

        int sum = 1;

        for(int i = 0; i < times.Length; i++){
            for(int j = 1; j < times[i]; j++){
                if((j * (times[i] - j)) > distances[i]){
                    sum *= times[i] - j * 2 + 1;

                    break;
                }
            }
        }

        Console.WriteLine(sum);
    }
}