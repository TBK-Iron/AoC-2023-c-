class Day11_1 {

    public static bool isAllDots(char[] line){
        foreach(char c in line){
            if(c != '.'){
                return false;
            }
        }

        return true;
    }
    public static char[][] addHorizontalExpansions(char[][] space){
        HashSet<int> expansionLocations = new HashSet<int>();
        for(int y = 0; y < space.Length; y++){
            if(isAllDots(space[y])){
                Console.WriteLine("test");
                expansionLocations.Add(y + expansionLocations.Count);
            }
        }

        char[][] newSpace = new char[space.Length + expansionLocations.Count][];
        int iteratedExpansions = 0;
        for(int i = 0; i < newSpace.Length; i++){
            if(expansionLocations.Contains(i)){
                newSpace[i] = Enumerable.Repeat('.', space[0].Length).ToArray();
                iteratedExpansions++;
            } else {
                newSpace[i] = space[i - iteratedExpansions];
            }
        }

        return newSpace;
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

        space = addHorizontalExpansions(space);
        space = transpose(space);
        space = addHorizontalExpansions(space);
        space = transpose(space);

        foreach(char[] row in space){
            Console.WriteLine(string.Join("", row));
        }

        List<Pos> galaxyLocations = findGalaxyLocations(space);

        int sum = 0;

        for(int i = 0; i < galaxyLocations.Count; i++){
            for(int j = i; j < galaxyLocations.Count; j++){
                Pos p1 = galaxyLocations[i];
                Pos p2 = galaxyLocations[j];

                sum += Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
            }
        }
        
        Console.WriteLine(sum);
    }

    public record Pos(int x, int y);
}