using System.Data;

class Day19_2 {
    
    private static long findCombinations(Range[] ranges, Dictionary<string, List<Rule>> workFlows, string currentWorkflow){
        long combinations = 0;

        if(currentWorkflow == "A"){
            return (ranges[0].end - ranges[0].start) *
                    (ranges[1].end - ranges[1].start) *
                    (ranges[2].end - ranges[2].start) *
                    (ranges[3].end - ranges[3].start);

        } else if(currentWorkflow == "R"){
            return 0;
        }

        bool foundNullInvalidRange = false;

        for(int i = 0; i < workFlows[currentWorkflow].Count - 1; i++){
            Rule rule = workFlows[currentWorkflow][i];
            Range validRange = rule.getValidRange(ranges[rule.getVariable()]);
            if(validRange != null){
                Range[] newRanges = (Range[]) ranges.Clone();
                newRanges[rule.getVariable()] = validRange;
                combinations += findCombinations(newRanges, workFlows, rule.getReturnVal());
            }

            Range invalidRange = rule.getInvalidRange(ranges[rule.getVariable()]);

            if(invalidRange == null){
                foundNullInvalidRange = true;
                break;
            }

            ranges[rule.getVariable()] = invalidRange;
            
        }

        if(!foundNullInvalidRange){
            combinations += findCombinations((Range[]) ranges.Clone(), workFlows, workFlows[currentWorkflow].Last().getReturnVal());
        }
        

        return combinations;


    }

    public static void Run(){
        string[] fullInput = File.ReadAllText(@"../../../src/inputs/input19.txt").Replace("\r", "").Split("\n\n");
        Dictionary<string, List<Rule>> workFlows = new Dictionary<string, List<Rule>>();

        foreach(String line in fullInput[0].Split("\n")){
            string name = line.Split("{")[0];
            List<Rule> rules = line.Split("{")[1].Replace("}", "").Split(",").Select(rule => new Rule(rule)).ToList();
            workFlows.Add(name, rules);
        }


        int sum = 0;

        Range[] ratingRanges = new Range[]{
            new Range(1, 4001),
            new Range(1, 4001),
            new Range(1, 4001),
            new Range(1, 4001)
        };

        long combinations = findCombinations(ratingRanges, workFlows, "in");
        Console.WriteLine(combinations);

    }

    public record Range(long start, long end);

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

        public int getVariable(){
            return this.variable;
        }

        public Range getValidRange(Range range){
            if(checkGT){
                if(range.end > this.checkVal + 1){
                    return new Range(Math.Max(this.checkVal + 1, range.start), range.end);
                }
            } else {
                if(range.start < this.checkVal){
                    return new Range(range.start, Math.Min(this.checkVal, range.end));
                }           
            }

            return null;
        }

        public Range getInvalidRange(Range range){
            if(checkGT){
                if(range.start <= this.checkVal){
                    return new Range(range.start, Math.Min(this.checkVal + 1, range.end));
                }
                
            } else {
                if(range.end >= this.checkVal + 1){
                    return new Range(Math.Max(this.checkVal, range.start), range.end);
                }
            }

            return null;
        }

        public string getReturnVal(){
            return returnVal;
        }
    }


}