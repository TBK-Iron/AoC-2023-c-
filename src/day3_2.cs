using System;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

public class Day3_2 {

    static HashSet<char> nonSymbolChars = new HashSet<char> {'.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
    static HashSet<char> numChars = new HashSet<char> {'0','1', '2', '3', '4', '5', '6', '7', '8', '9'};

    private static Boolean isAdjacent(int gearX, int gearY, List<int[]> numPoses) {
        bool adjacent = false;

        foreach (int[] pos in numPoses){
            int x = pos[0];
            int y = pos[1];

            if((Math.Abs(x - gearX) <= 1) && (Math.Abs(y-gearY) <= 1)){
                adjacent = true;
                break;
            }
        }

        return adjacent;
    }

    public static void Run(){

        string[] lines = File.ReadAllLines(@"../../../src/inputs/input3.txt");

        char[,] engineMap = new char[lines[0].Length, lines.Length];

        for(int y = 0; y < lines.Length; y++){
            char[] chararray = lines[y].ToCharArray();
            for(int x = 0; x < lines[0].Length; x++){
                engineMap[x, y] = chararray[x];
            }
        }
        List<KeyValuePair<int, List<int[]>>> numbers = new List<KeyValuePair<int, List<int[]>>>();
        List<int[]> gearPositions = new List<int[]>();

        for(int y = 0; y < lines.Length; y++){
            string currentNum = "";
            List<int[]> numPoses = new List<int[]>();
            bool isBuildingString = false;

            for(int x = 0; x < lines[0].Length; x++){
                if(engineMap[x,y] == '*'){
                    gearPositions.Add(new int[]{x, y});
                }
                if(numChars.Contains(engineMap[x, y])){
                    isBuildingString = true;
                    currentNum += engineMap[x,y];
                    numPoses.Add(new int[] {x, y});

                    if(isBuildingString && x == lines[0].Length - 1){
                        numbers.Add(new KeyValuePair<int, List<int[]>>(int.Parse(currentNum), new List<int[]>(numPoses)));
                    }
                } else if(isBuildingString) {
                    isBuildingString = false;
                    numbers.Add(new KeyValuePair<int, List<int[]>>(int.Parse(currentNum), new List<int[]>(numPoses)));

                    currentNum = "";
                    numPoses = new List<int[]>();
                }
            }
        }

        int sum = 0;

        foreach (int[] gearPos in gearPositions){
            List<int> adjNums = new List<int>();

            foreach (var pair in numbers){
                int number = pair.Key;
                List<int[]> numPoses = pair.Value;

                if(isAdjacent(gearPos[0], gearPos[1], numPoses)){
                    adjNums.Add(number);
                }
            }

            if(adjNums.Count == 2){
                sum += adjNums[0] * adjNums[1];
            }
        }
       

        Console.WriteLine(sum);

    }
}