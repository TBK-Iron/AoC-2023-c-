class Day6_2 {
    public static void Run(){
        string[] lines = File.ReadAllLines(@"../../../src/inputs/input6.txt");

        

        double time = double.Parse(lines[0].Split(": ")[1].Replace(" ", ""));
        double distance = double.Parse(lines[1].Split(": ")[1].Replace(" ", ""));

        double sum = 1;

        for(int j = 1; j < time; j++){
            if((j * (time - j)) > distance){
                sum *= time - j * 2 + 1;
                break;
            }
        }
        

        Console.WriteLine(sum);
    }
}