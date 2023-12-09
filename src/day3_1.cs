using System;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

public class Day3_1 {

    static HashSet<char> nonSymbolChars = new HashSet<char> {'.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
    static HashSet<char> numChars = new HashSet<char> {'0','1', '2', '3', '4', '5', '6', '7', '8', '9'};

    private static Boolean hasAdjSymbol(int x, int y, char[,] map) {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);

        if (x > 0 && y > 0 && !nonSymbolChars.Contains(map[x - 1, y - 1]))
            return true;
        if (y > 0 && !nonSymbolChars.Contains(map[x, y - 1]))
            return true;
        if (x < cols - 1 && y > 0 && !nonSymbolChars.Contains(map[x + 1, y - 1]))
            return true;
        if (x < cols - 1 && !nonSymbolChars.Contains(map[x + 1, y]))
            return true;
        if (x < cols - 1 && y < rows - 1 && !nonSymbolChars.Contains(map[x + 1, y + 1]))
            return true;
        if (y < rows - 1 && !nonSymbolChars.Contains(map[x, y + 1]))
            return true;
        if (x > 0 && y < rows - 1 && !nonSymbolChars.Contains(map[x - 1, y + 1]))
            return true;
        if (x > 0 && !nonSymbolChars.Contains(map[x - 1, y]))
            return true;

        return false;
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

        for(int y = 0; y < lines.Length; y++){
            string currentNum = "";
            List<int[]> numPoses = new List<int[]>();
            bool isBuildingString = false;

            for(int x = 0; x < lines[0].Length; x++){
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

        foreach (var pair in numbers){
            int number = pair.Key;
            Console.Write(number + ": ");
            List<int[]> numPoses = pair.Value;
            bool isAdjacent = false;
            foreach (int[] pos in numPoses){
                Console.Write("x=" + pos[0] + ", y=" + pos[1] + "; ");
                if(hasAdjSymbol(pos[0], pos[1], engineMap)){
                    isAdjacent = true;
                }
            }
            Console.WriteLine(isAdjacent);
            if(isAdjacent){
                sum += number;
            }
        }

        Console.WriteLine(sum);

    }
}