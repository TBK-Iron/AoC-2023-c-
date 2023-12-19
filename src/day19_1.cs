using System.Data;

class Day19_1 {
    public static void Run(){
        string[] fullInput = File.ReadAllText(@"../../../src/inputs/input19.txt").Replace("\r", "").Split("\n\n");
        Dictionary<string, List<Rule>> workFlows = new Dictionary<string, List<Rule>>();

        foreach(String line in fullInput[0].Split("\n")){
            string name = line.Split("{")[0];
            List<Rule> rules = line.Split("{")[1].Replace("}", "").Split(",").Select(rule => new Rule(rule)).ToList();
            workFlows.Add(name, rules);
        }

        List<Part> parts = fullInput[1].Split("\n").Select(line => new Part(line
                            .Replace("{","").Replace("}", "")
                            .Replace("x", "").Replace("m", "")
                            .Replace("a", "").Replace("s", "")
                            .Replace("=", "").Split(",")
                            .Select(num => int.Parse(num)).ToArray())).ToList();


        int sum = 0;

        foreach(Part part in parts){
            //Console.WriteLine(string.Join(",", part.ratings));
            string currentWorkFlow = "in";
            bool isSearching = true;
            while(isSearching){
                foreach(Rule rule in workFlows[currentWorkFlow]){
                    string result = rule.checkPart(part);
                    if(result == "A"){
                        sum += part.ratings.Sum();
                        isSearching = false;
                        break;
                    } else if(result == "R"){
                        isSearching = false;
                        break;
                    } else if(result == "") {
                        continue;
                    } else {
                        currentWorkFlow = result;
                        break;
                    }
                }
            }
        }

        Console.WriteLine(sum);

    }

    public class Part{
        public readonly int[] ratings; 
        public Part(int[] ratings){
            this.ratings = ratings;
        }
    }

    public class Rule {

        int variable;

        bool checkGT;
        int checkVal;
        string returnVal;
        public Rule(string stringRule){
            //Console.WriteLine(stringRule);
            if(stringRule.Split(":").Length == 1){
                this.returnVal = stringRule;
                this.variable = -1;

            } else{
                char variablechar = stringRule.ToCharArray()[0];
                if(variablechar == 'x'){
                    this.variable = 0;
                } else if(variablechar == 'm'){
                    this.variable = 1;
                } else if(variablechar == 'a'){
                    this.variable = 2;
                } else if(variablechar == 's'){
                    this.variable = 3;
                } else {
                    throw new Exception("Illegal variable: " + variablechar.ToString());
                }

                if(stringRule.ToCharArray()[1] == '>'){
                    checkGT = true;
                } else {
                    checkGT = false;
                }

                checkVal = int.Parse(stringRule.Split(":")[0].Substring(2));

                returnVal = stringRule.Split(":")[1];
            } 
            
        }

        public string checkPart(Part part){
            if(this.variable == -1){
                return returnVal;
            }
            if(checkGT){
                if(part.ratings[this.variable] > this.checkVal){
                    return returnVal;
                } else {
                    return "";
                }
            } else {
                //Console.WriteLine(string.Join(",", part.ratings) + " " + this.variable);
                if(part.ratings[this.variable] < this.checkVal){
                    return returnVal;
                } else {
                    return "";
                }
            }
            
        }
    }


}