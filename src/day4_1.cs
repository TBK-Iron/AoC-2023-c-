public class Day4_1 {
    public static void Run(){
        string[] lines = File.ReadAllLines(@"../../../src/inputs/input4.txt");

        int totalPoints = 0;

        foreach (string line in lines){
            HashSet<string> winning = new HashSet<string>(line.Split(": ")[1].Split(" | ")[0].Split(" "));
            string[] guesses = line.Split(": ")[1].Split(" | ")[1].Split(" ");

            int points = 0;

            foreach(string guess in guesses){
                if(guess.Equals("")){
                    continue;
                }
                Console.Write(guess + " " + winning.Contains(guess) + " ");
                if(winning.Contains(guess)){
                    if(points == 0){
                        points = 1;
                    } else {
                        points *= 2;
                    }
                }
            }
            Console.WriteLine(points);
            totalPoints += points;
        }

        Console.WriteLine(totalPoints);
    }
}