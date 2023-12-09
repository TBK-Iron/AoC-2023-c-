using System;
using System.Dynamic;
using System.IO;
using System.Text.RegularExpressions;

public class Day2_1 {

    public static void Run(){

        Dictionary<string, int> colors = new Dictionary<string, int> {
            {"red", 12},
            {"green", 13},
            {"blue", 14}
        };


        string[] lines = File.ReadAllLines(@"../../../src/inputs/input2.txt");

        int sum = 0;

        foreach (string line in lines){
            int gameNum = int.Parse(line.Split(":")[0].Split(" ")[1]);
            Console.WriteLine(gameNum);
            string[] sets = line.Split(": ")[1].Split("; ");
            Boolean possible = true;
            foreach(string set in sets){
                string[] boxes = set.Split(", ");
                foreach(string box in boxes){
                    int number = int.Parse(box.Split(" ")[0]);
                    string color = box.Split(" ")[1];

                    Console.WriteLine(number + " " + color);

                    if(number > colors[color]){
                        possible = false;
                        break;
                    }
                }
            }

            if(possible){
                sum += gameNum;
            }
            
        }

        Console.WriteLine(sum);


    }

}