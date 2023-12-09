using System;
using System.Dynamic;
using System.IO;

public class Day1_1 {
    public static void Run(){
        string[] lines = File.ReadAllLines(@"../../../src/inputs/input1.txt");

        List<char> numbers = new List<char>();

        HashSet<char> numchars = new HashSet<char> {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

        foreach (string line in lines){
            char[] lineArray = line.ToCharArray();
            //Console.WriteLine(line);
            for(int i = 0; i < lineArray.Length; i++){
                if(numchars.Contains(lineArray[i])){
                    //Console.Write(lineArray[i]);
                    numbers.Add(lineArray[i]);
                    break;
                }
            }
            for(int i = lineArray.Length - 1; i >= 0; i--){
                if(numchars.Contains(lineArray[i])){
                    //Console.WriteLine(lineArray[i]);
                    numbers.Add(lineArray[i]);
                    break;
                }
            }
        }

        int sum = 0;

        for(int i = 0; i < numbers.Count(); i+=2){
            //Console.WriteLine(numbers[i].ToString() + " " + numbers[i+1].ToString());
            sum += int.Parse(numbers[i].ToString() + numbers[i+1].ToString());
        }

        Console.WriteLine(sum);
    }
}