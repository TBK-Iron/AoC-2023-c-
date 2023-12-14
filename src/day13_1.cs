class Day13_1 {

    public static char[][] rotate(char[][] matrix){
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

    public static int findHorizontalMirroringIndex(char[][] pattern){
        int sum = 0;
        int y1 = 0;

        for(int y2i = pattern.Length - 1; y2i > 0; y2i--){
            if(pattern[y1].SequenceEqual(pattern[y2i]) && ((y2i - y1) % 2 == 1)){
                
                bool foundSymmetry = true;
                for(int i = 0; i <= (y2i - y1 - 1)/2; i++){
                    if(!pattern[y1+i].SequenceEqual(pattern[y2i-i])){
                        foundSymmetry = false;
                        break;
                    }
                }

                if(foundSymmetry){
                    
                    sum += y1 + (y2i - y1 - 1)/2 + 1;
                }
            }
        }

        int initialSum = sum;


        int y2 = pattern.Length - 1;
        for(int y1i = 0; y1i < pattern.Length - 1; y1i++){
            
            if(pattern[y1i].SequenceEqual(pattern[y2]) && ((y2 - y1i) % 2 == 1)){
                
                bool foundSymmetry = true;
                for(int i = 0; i <= (y2 - y1i - 1)/2; i++){
                   
                    if(!pattern[y1i+i].SequenceEqual(pattern[y2-i])){
                        foundSymmetry = false;
                        break;
                    }
                }

                if(foundSymmetry){
                    sum += y1i + (y2 - y1i - 1)/2 +1;
                }
            }
        }

        return sum;
    }
    public static void Run(){
        List<char[][]> patterns = File.ReadAllText(@"../../../src/inputs/input13.txt").Split("\n\n")
                                .Select(pattern => pattern.Split("\n")
                                .Select(line => line.ToCharArray()).ToArray()).ToList();

        int sum = 0;
        foreach(char[][] pattern in patterns){
            sum += 100 * findHorizontalMirroringIndex(pattern);  
            sum += findHorizontalMirroringIndex(rotate(pattern));
 
        }

        Console.WriteLine(sum);


    }
}