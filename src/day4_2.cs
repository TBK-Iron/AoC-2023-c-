public class Day4_2 {
    public static void Run(){
        string[] lines = File.ReadAllLines(@"../../../src/inputs/input4.txt");

        int[] multipliers = new int[lines.Length];

        for (int i = 0; i < lines.Length; i++){
            multipliers[i] += 1;
            for(int cardNr = 0; cardNr < multipliers[i]; cardNr++){
                string line = lines[i];
                HashSet<string> winning = new HashSet<string>(line.Split(": ")[1].Split(" | ")[0].Split(" "));
                string[] guesses = line.Split(": ")[1].Split(" | ")[1].Split(" ");

                int matching = 0;

                foreach(string guess in guesses){
                    if(guess.Equals("")){
                        continue;
                    }
                    //Console.Write(guess + " " + winning.Contains(guess) + " ");
                    if(winning.Contains(guess)){
                        matching += 1;
                    }
                }

                for(int j = 0; (j < matching) && (i + j + 1 < lines.Length); j++){
                    multipliers[i + j + 1] += 1;
                }
            }
            
        }

        int totalCards = 0;

        for(int i = 0; i < multipliers.Length; i++){
            totalCards += multipliers[i];
        }

        Console.WriteLine(totalCards);

   
    }
}