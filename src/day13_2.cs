class Day13_2 {

    public static int differences(char[] array1, char[] array2){
        int differences = 0;
        if(array1.Length != array2.Length){
            return int.MaxValue;
        }

        for(int i = 0; i < array1.Length; i++){
            if(array1[i] != array2[i]){
                differences++;
            }
        }

        return differences;
    }

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

    public static (int, int) findHorizontalMirroringIndex(char[][] pattern){
        int sum = 0;
        int y1 = 0;
        int foundSymmetries = 0;

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
                    foundSymmetries++;
                    break;
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
                    foundSymmetries++;
                    break;
                }
            }
        }

        return (sum, foundSymmetries);
    }

    public static char[][] changeSmudge(char[][] pattern, int y, int x){
        char[][] clonedPattern = new char[pattern.Length][];
        for (int i = 0; i < pattern.Length; i++){
            clonedPattern[i] = new char[pattern[i].Length];
            Array.Copy(pattern[i], clonedPattern[i], pattern[i].Length);
        }
        if(pattern[y][x] == '#'){
            clonedPattern[y][x] = '.';
        } else {
            clonedPattern[y][x] = '#';
        }

        return clonedPattern;
    }

    public static void Run(){
        List<char[][]> patterns = File.ReadAllText(@"../../../src/inputs/input13.txt").Replace("\r", "").Split("\n\n")
                                .Select(pattern => pattern.Split("\n")
                                .Select(line => line.ToCharArray()).ToArray()).ToList();

        int sum = 0;
 
        foreach(char[][] pattern in patterns){
            bool found = false;

            (int, int) initialMirrors1 = findHorizontalMirroringIndex(pattern);
            initialMirrors1.Item1 *= 100;
            (int, int) initialMirrors2 = findHorizontalMirroringIndex(rotate(pattern));

            for(int i = 0; i < pattern.Length; i++){
                for(int j = 0; j < pattern[i].Length; j++){
                    char[][] tempPattern = changeSmudge(pattern, i, j);

                    (int, int) mirrors1 = findHorizontalMirroringIndex(tempPattern);
                    mirrors1.Item1 *= 100;
                    (int, int) mirrors2 = findHorizontalMirroringIndex(rotate(tempPattern)); 

                    if(mirrors1.Item2 > 0){
                        if(mirrors1.Item2 == 1){
                            if(initialMirrors1.Item1 != mirrors1.Item1){
                                sum += mirrors1.Item1;
                                found = true;
                                break;
                            }
                        } else {
                            sum += mirrors1.Item1 - initialMirrors1.Item1;
                            found = true;
                            break;
                        }
                    } 
                    if(mirrors2.Item2 > 0){
                        if(mirrors2.Item2 == 1){
                            if(initialMirrors2.Item1 != mirrors2.Item1){
                                sum += mirrors2.Item1;
                                found = true;
                                break;
                            }
                        } else {
                            sum += mirrors2.Item1 - initialMirrors2.Item1;
                            found = true;
                            break;
                        }
                    }
                   
                }

                if(found){
                    break;
                }
            }         
        }

        Console.WriteLine(sum);


    }
}