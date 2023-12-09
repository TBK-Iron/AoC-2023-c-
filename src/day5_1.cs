using System.Diagnostics;

public class Day5_1 {
    public static void Run(){
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        string[] lines = File.ReadAllLines(@"../../../src/inputs/input5.txt");

        Double[] seeds = lines[0].Split(": ")[1].Split(" ").Select(Double.Parse).ToArray();

        List<List<Range>> maps = new List<List<Range>>();
        for(int i = 0; i < 7; i++){
            maps.Add(new List<Range>());
        }

        int mapNr = 0;

        for(int i = 2; i < lines.Length; i++){
            if(lines[i].EndsWith(":")){
                i++;
                while(!lines[i].Equals("")){
                    Double[] rangeData = lines[i].Split(" ").Select(Double.Parse).ToArray();
                    maps[mapNr].Add(new Range( rangeData[1], rangeData[0], rangeData[2]));
                    i++;
                    if(i >= lines.Length){
                        break;
                    }
                }

                mapNr++;
            }
        }

        Double minLocationVal = Double.MaxValue;

        foreach(Double seed in seeds){
            Double currentValue = seed;
            for(int i = 0; i < maps.Count; i++){
                //Console.WriteLine("seed " + seed + ": " + currentValue);
                for(int j = 0; j < maps[i].Count; j++){
                    //Console.WriteLine(i + " Range " + j + " Contains " + currentValue + " = " + maps[i][j].Contains(currentValue));
                    if(maps[i][j].Contains(currentValue)){
                        currentValue = maps[i][j].GetMappedValue(currentValue);
                        break;
                    }
                }
                
            }
            //Console.WriteLine(currentValue);
            minLocationVal = Math.Min(minLocationVal, currentValue);
        }

        stopwatch.Stop();
        Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
        Console.WriteLine($"Result: {minLocationVal}");

    }

    public class Range {

        private Double start;
        private Double end;

        private Double offset;
        
        public Range(Double start, Double offset, Double size){
            this.start = start;
            this.end = start + size;
            this.offset = offset;
        }

        public Boolean Contains(Double value){
            if((value >= start) && (value < end)){
                return true;
            } else {
                return false;
            }
        }

        public Double GetMappedValue(Double value){
            return value - start + offset;
        }
    }
}