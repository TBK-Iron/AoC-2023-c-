public class Day12_1 {
    private static bool IsValidConfiguration(char[] springs, int[] groupings){
        char currentChar = '.';
        int currentGrouping = 0;
        int groupingIndex = 0;

        List<int> foundGroupings = new List<int>();

        for(int i = 0; i < springs.Length; i++){
            if(springs[i] == '#'){
                currentChar = '#';
                currentGrouping++;
            } else if(springs[i] == '?'){
                return false;
            } else if((springs[i] == '.') && (currentChar == '#')){
                if (groupingIndex >= groupings.Length || groupings[groupingIndex] != currentGrouping){
                    return false;
                }
                foundGroupings.Add(currentGrouping);
                groupingIndex++;
                currentChar = '.';
                currentGrouping = 0;

            } 
            if ((i == springs.Length - 1) && (currentChar == '#')){
                foundGroupings.Add(currentGrouping);
            }
            
        }

        if (foundGroupings.Count != groupings.Length){
                return false;
        }

        for(int i = 0; i < foundGroupings.Count; i++){
            if(foundGroupings[i] != groupings[i]){
                return false;
            }
        }


        return true;

    }

    private static int CountUnknown(char[] array){
        int count = 0;
        for(int i = 0; i < array.Length; i++){
            if(array[i] == '?'){
                count++;
            }
        }

        return count;
    }

    private static bool[] CreateBoolArray(int configuration, int size){
        bool[] bits = new bool[size];
        for (int i = 0; i < size; i++)
        {
            bits[i] = (configuration & (1 << i)) != 0;
        }

        return bits;
    }

    public static void Run(){
        string[] lines = File.ReadAllLines(@"../../../src/inputs/input12.txt");

        List<char[]> springs = new List<char[]>();
        List<int[]> groupings = new List<int[]>();

        foreach(string line in lines){
            springs.Add(line.Split(" ")[0].ToCharArray());
            groupings.Add(line.Split(" ")[1].Split(",").Select(int.Parse).ToArray());
        }

        int sum = 0;

        for(int lineIndx = 0; lineIndx < lines.Length; lineIndx++){
            int unknown = CountUnknown(springs[lineIndx]);
            int configurations = (int) Math.Pow(2, unknown);

            int validConfigs = 0;

            

            for(int configuration = 0; configuration < configurations; configuration++){
                char[] springConfiguration = (char[]) springs[lineIndx].Clone();
                bool[] bitArray = CreateBoolArray(configuration, unknown);
                int bitIndex = 0;

                

                for(int i = 0; i < springConfiguration.Length; i++){
                    if(springs[lineIndx][i] == '?'){
                        if(bitArray[bitIndex] == true){
                            springConfiguration[i] = '#';
                        } else {
                            springConfiguration[i] = '.';
                        }
                        bitIndex++;
                    }
                }

                //Console.WriteLine(string.Join("", springConfiguration) + " " + string.Join(" ",bitArray) + " " +  configuration + " " + unknown);
                bool isValid = IsValidConfiguration(springConfiguration, groupings[lineIndx]);
                //Console.WriteLine(string.Join("", springConfiguration) + " " + string.Join(",", groupings[lineIndx]) + " " + isValid);

                if(isValid){
                    //Console.WriteLine(string.Join("", springConfiguration) + " " + string.Join(",", groupings[lineIndx]) + " " + isValid);
                    validConfigs++;
                }
            }

            Console.WriteLine(lineIndx + " - " + validConfigs);
            sum += validConfigs;

            
        }

        Console.WriteLine(sum);
    }
}