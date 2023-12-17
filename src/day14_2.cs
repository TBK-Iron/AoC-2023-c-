using System.Numerics;

class Day14_2 {

    // Taken from day 13 solution
    public static bool dishEquals(char[][] dish1, char[][] dish2){
        if(dish1.Length != dish2.Length){
            return false;
        }

        for(int i = 0; i < dish1.Length; i++){
            if(dish1[i].Length != dish2[i].Length){
                return false;
            }

            for(int j = 0; j < dish1[i].Length; j++){
                if(dish1[i][j] != dish2[i][j]){
                    return false;
                }
            }
        }

        return true;
    }
    public static char[][] rotateClockwise(char[][] matrix){
        int rows = matrix.Length;
        int columns = matrix[0].Length;
        char[][] rotatedMatrix = new char[columns][];
        for(int i = 0; i < rotatedMatrix.Length; i++){
            rotatedMatrix[i] = Enumerable.Repeat('.', rows).ToArray();    
        }

     

        for(int i = 0; i < rows; i++){
            for(int j = 0; j < columns; j++){
                rotatedMatrix[j][i] = matrix[matrix.Length - i - 1][j];
            }
        }

        return rotatedMatrix;
    }

    public static Pos moveRockUp(Pos rockPos, char[][] map){
        int minY = rockPos.y;
        for(int y = rockPos.y-1; y >= 0; y--){
            if(map[y][rockPos.x] == '.'){
                minY = y;
            } else {
                break;
            }
        }

        map[rockPos.y][rockPos.x] = '.';
        map[minY][rockPos.x] = 'O';

        return new Pos(rockPos.x, minY);
    }

    public static int countRound(char[] line){
        int count = 0;
        for(int i = 0; i < line.Length; i++){
            if(line[i] == 'O'){
                count++;
            }
        }

        return count;
    }

    public static List<Pos> findRoundRocks(char[][] dishRocks){
        List<Pos> roundRockPositions = new List<Pos>();

        for(int y = 0; y < dishRocks.Length; y++){
            for(int x = 0; x < dishRocks[y].Length; x++){
                if(dishRocks[y][x] == 'O'){
                    roundRockPositions.Add(new Pos(x, y));
                }
            }
        }

        return roundRockPositions;

    }

    public static char[][] cloneDish(char[][] dish){
        char[][] clonedDish = new char[dish.Length][];
        for (int i = 0; i < dish.Length; i++){
            clonedDish[i] = new char[dish[i].Length];
            Array.Copy(dish[i], clonedDish[i], dish[i].Length);
        }

        return clonedDish;
    }

    public static char[][] run1cycle(char[][] dishRocks){
        for(int i = 0; i < 4; i++){
            List<Pos> roundRocks = findRoundRocks(dishRocks);
            
            for(int j = 0; j < roundRocks.Count; j++){
                roundRocks[j] = moveRockUp(roundRocks[j], dishRocks);
            }

            dishRocks = rotateClockwise(dishRocks);
            
        }

        return dishRocks;
    }

    public static void Run(){
        char[][] dishRocks = File.ReadAllLines(@"../../../src/inputs/input14.txt").Select(line => line.ToCharArray()).ToArray();

        HashSet<char[][]> seenPatterns = new HashSet<char[][]>(new CharArrayComparer()){dishRocks};
        List<char[][]> allPatterns = new List<char[][]>();

        char[][] finalPattern = null;
        while(true){
            char[][] cycledDishRocks = run1cycle(cloneDish(dishRocks));

            if(seenPatterns.Contains(cycledDishRocks)){
                int cycleEnteredAt = 0;
                for(int i = 0; i < allPatterns.Count; i++){
                    if(dishEquals(allPatterns[i], cycledDishRocks)){
                        cycleEnteredAt = i;
                        break;
                    }
                }

                int finalRockCycleIndex = (1_000_000_000 - cycleEnteredAt) % (allPatterns.Count - cycleEnteredAt);
                finalPattern = allPatterns[cycleEnteredAt + finalRockCycleIndex - 1];
                break;
            }

            seenPatterns.Add(cycledDishRocks);
            allPatterns.Add(cycledDishRocks);
            dishRocks = cloneDish(cycledDishRocks);
           
        }

        int sum = 0;

        for(int y = 0; y < finalPattern.Length; y++){
            sum += countRound(finalPattern[y]) * (finalPattern.Length - y);
        }

        Console.WriteLine(sum);


    }

    public record Pos(int x, int y);

    public class CharArrayComparer : IEqualityComparer<char[][]>{

        public bool Equals(char[][] x, char[][] y){
            if (x.Length != y.Length) return false;
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i].Length != y[i].Length) return false;
                for (int j = 0; j < x[i].Length; j++)
                {
                    if (x[i][j] != y[i][j]) return false;
                }
            }
            return true;
        }

        public int GetHashCode(char[][] obj){
            int hash = 948721;

            foreach (var array in obj)
            {
                foreach (var ch in array)
                {
                    hash = hash * 31 + ch.GetHashCode();
                }
            }
            return hash;
        }
    }
}