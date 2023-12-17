using System.Collections;
using System.Collections.Specialized;

class Day15_2 {

    public static int hash(string input){
        int result = 0;

        foreach(char c in input.ToCharArray()){
            result = (result + c) * 17 % 256;
        }

        return result;
    }
    public static void Run(){
        string[] sequence = File.ReadAllText(@"../../../src/inputs/input15.txt").Replace("\n", "").Split(",");

        List<OrderedDictionary> boxes = new List<OrderedDictionary>();
        for(int i = 0; i < 256; i++){
            boxes.Add(new OrderedDictionary());
        } 


        foreach(string step in sequence){
            if(step.EndsWith('-')){
                string label = step.Substring(0, step.Length-1);
                int box = hash(label);
                boxes[box].Remove(label);
        
            } else {
                string label = step.Substring(0, step.Length-2);
                int box = hash(label);
                if (boxes[box].Contains(label)){
                    boxes[box][label] = int.Parse(step.Substring(step.Length-1, 1)); 
                } else {
                    
                    boxes[box].Add(label, int.Parse(step.Substring(step.Length-1, 1)));   
                }
                
            }

           
        }
        int sum = 0;
        for(int i = 0; i < boxes.Count; i++){
            int j = 0;
            foreach(DictionaryEntry entry in boxes[i]){
                sum += (int) entry.Value * (i+1)*(j+1);
                j++;
            }
        }

        Console.WriteLine(sum);
    }
}