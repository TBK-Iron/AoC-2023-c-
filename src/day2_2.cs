using System;
using System.Dynamic;
using System.IO;
using System.Text.RegularExpressions;

public class Day2_2 {

    public static void Run(){


        string[] lines = File.ReadAllLines(@"../../../src/inputs/input2.txt");

        int powerSum = 0;

        foreach (string line in lines){
            int gameNum = int.Parse(line.Split(":")[0].Split(" ")[1]);
            Console.WriteLine(gameNum);
            string[] sets = line.Split(": ")[1].Split("; ");

            Dictionary<string, int> minVals = new Dictionary<string, int> {
                {"red", 0},
                {"blue", 0},
                {"green", 0}
            };
         
            foreach(string set in sets){
                string[] boxes = set.Split(", ");
                foreach(string box in boxes){
                    int number = int.Parse(box.Split(" ")[0]);
                    string color = box.Split(" ")[1];

                    if(number > minVals[color]){
                        minVals[color] = number;
                    }

                    
                }
            }
            Console.WriteLine(minVals["red"] + " " + minVals["green"] + " " + minVals["blue"]);
            int power = minVals["red"] * minVals["green"] * minVals["blue"];
            powerSum += power;

        }

        Console.WriteLine(powerSum);

    }

}