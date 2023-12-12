class Day11_2 {

    public static bool isAllDots(char[] line){
        foreach(char c in line){
            if(c != '.'){
                return false;
            }
        }

        return true;
    }
    public static HashSet<int> getHorizontalExpansions(char[][] space){
        HashSet<int> expansionLocations = new HashSet<int>();
        for(int y = 0; y < space.Length; y++){
            if(isAllDots(space[y])){
                expansionLocations.Add(y);
            }
        }

        return expansionLocations;

    }

    public static char[][] transpose(char[][] matrix){
        int rows = matrix.Length;
        int columns = matrix[0].Length;
        char[][] transposedMatrix = new char[columns][];
        for(int i = 0; i < transposedMatrix.Length; i++){
            transposedMatrix[i] = Enumerable.Repeat('.', rows).ToArray();
        }

        for(int i = 0; i < rows; i++){
            for(int j = 0; j < columns; j++){
                transposedMatrix[j][i] = matrix[i][j];
            }
        }

        return transposedMatrix;
    }

    public static List<Pos> findGalaxyLocations(char[][] space){
        List<Pos> galaxyLocations = new List<Pos>();
        for(int y = 0; y < space.Length; y++){
            for(int x = 0; x < space[y].Length; x++){
                if(space[y][x] == '#'){
                    galaxyLocations.Add(new Pos(x, y));
                }
            }
        }

        return galaxyLocations;
    }

    public static void Run(){
        char[][] space = File.ReadAllLines(@"../../../src/inputs/input11.txt").Select(line => line.ToCharArray()).ToArray();

        HashSet<int> horizontalExpansions = getHorizontalExpansions(space);
        space = transpose(space);
        HashSet<int> verticalExpansions = getHorizontalExpansions(space);
        space = transpose(space);

        Console.WriteLine(string.Join(", ", horizontalExpansions));
        Console.WriteLine(string.Join(", ", verticalExpansions));
        

        List<Pos> galaxyLocations = findGalaxyLocations(space);

        long sum = 0;

        int expansion = 999999;

        for(int i = 0; i < galaxyLocations.Count; i++){
            for(int j = i; j < galaxyLocations.Count; j++){
                Pos p1 = galaxyLocations[i];
                Pos p2 = galaxyLocations[j];

                int xExpansion = 0;
                for(int x = Math.Min(p1.x, p2.x); x < Math.Max(p1.x, p2.x); x++){
                    if(verticalExpansions.Contains(x)){
                        xExpansion += expansion;
                    }
                }

                int yExpansion = 0;
                for(int y = Math.Min(p1.y, p2.y); y < Math.Max(p1.y, p2.y); y++){
                    if(horizontalExpansions.Contains(y)){
                        yExpansion += expansion;
                    }
                }

                sum += Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y) + xExpansion + yExpansion;
            }
        }
        
        Console.WriteLine(sum);
    }

    public record Pos(int x, int y);
}