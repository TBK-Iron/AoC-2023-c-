using System;
using System.Dynamic;
using System.IO;
using System.Text.RegularExpressions;


public class Day1_2 {
    public static void Run(){
        string[] lines = File.ReadAllLines(@"../../../src/inputs/input1.txt");

        List<char> numbers = new List<char>();


        for(int i = 0; i < lines.Length; i++){
            lines[i] = Regex.Replace(lines[i], "one", "one1one");
            lines[i] = Regex.Replace(lines[i], "two", "two2two");
            lines[i] = Regex.Replace(lines[i], "three", "three3three");
            lines[i] = Regex.Replace(lines[i], "four", "four4four");
            lines[i] = Regex.Replace(lines[i], "five", "five5five");
            lines[i] = Regex.Replace(lines[i], "six", "six6six");
            lines[i] = Regex.Replace(lines[i], "seven", "seven7seven");
            lines[i] = Regex.Replace(lines[i], "eight", "eight8eight");
            lines[i] = Regex.Replace(lines[i], "nine", "nine9nine");
        }



        HashSet<char> numchars = new HashSet<char> {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
        

        foreach (string line in lines){
            Console.WriteLine(line);
            char[] lineArray = line.ToCharArray();

            for(int i = 0; i < lineArray.Length; i++){
                if(numchars.Contains(lineArray[i])){
                   
                    numbers.Add(lineArray[i]);
                    break;
                }
            }
            for(int i = lineArray.Length - 1; i >= 0; i--){
                if(numchars.Contains(lineArray[i])){
      
                    numbers.Add(lineArray[i]);
                    break;
                }
            }
        }

        int sum = 0;

        for(int i = 0; i < numbers.Count(); i+=2){
            sum += int.Parse(numbers[i].ToString() + numbers[i+1].ToString());
        }

        Console.WriteLine(sum);
    }
}