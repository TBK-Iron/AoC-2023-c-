class Day15_1 {

    public static int hash(string input){
        int result = 0;

        foreach(char c in input.ToCharArray()){
            result = (result + c) * 17 % 256;
        }

        return result;
    }
    public static void Run(){
        string[] sequence = File.ReadAllText(@"../../../src/inputs/input15.txt").Replace("\n", "").Split(",");

        int sum = 0;

        foreach(string step in sequence){
            sum += hash(step);
        }

        Console.WriteLine(sum);
    }
}