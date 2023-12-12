using System.Diagnostics;

public class Day12_2 {

    public static char[] RepeatCharArray(char[] array, int repeat, char join){
        int totalLength = array.Length * repeat + repeat - 1;
        char[] result = new char[totalLength];

        int currentPosition = 0;
        for(int i = 0; i < repeat; i++){
            Array.Copy(array, 0, result, currentPosition, array.Length);
            currentPosition += array.Length + 1;
            if(i != repeat-1){
                result[currentPosition - 1] = '?';
            }
            
        }

        return result;
    }

    public static int[] RepeatIntArray(int[] array, int repeat){
        int totalLength = array.Length * repeat;
        int[] result = new int[totalLength];

        int currentPosition = 0;
        for(int i = 0; i < repeat; i++){
            Array.Copy(array, 0, result, currentPosition, array.Length);
            currentPosition += array.Length;
           
            
        }

        return result;
    }

    private static Dictionary<(int, int, int, char[], int[]), long> memoization = new Dictionary<(int, int, int, char[], int[]), long>();

    private static long countValidSequences(int springIndx, int groupIndx, int currentGrouping, char[] springs, int[] groupings){
        long result = 0;
        if(memoization.TryGetValue((springIndx, groupIndx, currentGrouping, springs, groupings), out result)){
            return result;
        }

        if(springIndx == springs.Length){
            if(groupIndx == groupings.Length -1 && groupings[groupIndx] == currentGrouping){
                result = 1;
            } else if(groupIndx == groupings.Length && currentGrouping == 0){
                result = 1;
            } else {
                result = 0;
            }
        } else {
            if(springs[springIndx] == '#' || springs[springIndx] == '?'){
                result += countValidSequences(springIndx+1, groupIndx, currentGrouping+1, springs, groupings);
            }

            if(springs[springIndx] == '.' || springs[springIndx] == '?'){
                if(currentGrouping == 0){
                    result += countValidSequences(springIndx+1, groupIndx, 0, springs, groupings);
                } else if(groupIndx < groupings.Length && groupings[groupIndx] == currentGrouping){
                    result += countValidSequences(springIndx+1, groupIndx+1, 0, springs, groupings);
                }
            }
        }
        

        memoization.Add((springIndx, groupIndx, currentGrouping, springs, groupings), result);
        return result;

    }


    public static void Run(){
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        string[] lines = File.ReadAllLines(@"../../../src/inputs/input12.txt");

        List<char[]> springsList = new List<char[]>();
        List<int[]> groupingsList = new List<int[]>();

        foreach(string line in lines){
            

            springsList.Add(RepeatCharArray(line.Split(" ")[0].ToCharArray(), 5, '?'));
            groupingsList.Add(RepeatIntArray(line.Split(" ")[1].Split(",").Select(int.Parse).ToArray(), 5));
        }

        long sum = 0;

        for(int i = 0; i < lines.Length; i++){
            sum += countValidSequences(0, 0, 0, springsList[i], groupingsList[i]);
        }

        stopwatch.Stop();
        Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
        Console.WriteLine($"Result: {sum}");
    }
}