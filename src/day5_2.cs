using System.Diagnostics;

public class Day5_2 {

    public static List<Range> MinimizeRanges(List<Range> ranges){
        ranges.Sort();
        List<Range> result = new List<Range>();

        Range compareFrom = ranges[0];

        for(int i = 1; i < ranges.Count; i++){
            List<Range> unionResult = Range.Union(compareFrom, ranges[i]);
            if(unionResult.Count == 1){
                compareFrom = unionResult[0];
            } else {
                result.Add(compareFrom);
                compareFrom = ranges[i];
            }

            if(i == ranges.Count - 1){
                result.Add(compareFrom);
            }
        }

        return result;
    }

    public static List<Range> FindGaps(Range superRange, List<Range> ranges){
        ranges.Sort();

        List<Range> gaps = new List<Range>();
        double previousEnd = superRange.getStart();

        foreach (var range in ranges) {
            if (previousEnd < range.getStart()) {
               
                gaps.Add(new Range(previousEnd, range.getStart()));
            }
            previousEnd = Math.Max(previousEnd, range.getEnd());
        }

        if (previousEnd < superRange.getEnd()) {
            gaps.Add(new Range(previousEnd, superRange.getEnd()));
        }

        return gaps;
    }

    public static void Run(){

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        string[] lines = File.ReadAllLines(@"../../../src/inputs/input5.txt");

        Double[] seedValues = lines[0].Split(": ")[1].Split(" ").Select(Double.Parse).ToArray();
        List<Range> seedRanges = new List<Range>();
        for(int i = 0; i < seedValues.Length; i+=2){
            seedRanges.Add(new Range(seedValues[i], seedValues[i] + seedValues[i+1]));
        }

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
                    maps[mapNr].Add(new Range(rangeData[0], rangeData[1], rangeData[2]));
                    i++;
                    if(i >= lines.Length){
                        break;
                    }
                }

                mapNr++;
            }
        }

        List<Range> currentRanges = seedRanges;

        for(int i = 0; i < maps.Count; i++){
            currentRanges = MinimizeRanges(currentRanges);
            //Console.WriteLine($"map {i}: {String.Join(", ", currentRanges)}");
            List<Range> newRanges = new List<Range>();
            foreach(Range range in currentRanges){
                List<Range> intersections = new List<Range>();
                for(int j = 0; j < maps[i].Count; j++){
                    Range intersection = maps[i][j].Intersection(range);
                    if(intersection != null){
                        intersections.Add(intersection);
                        newRanges.Add(maps[i][j].OffsetRange(intersection));
                    }
                }

                newRanges.AddRange(FindGaps(range, intersections));

            }
            currentRanges = newRanges;
        }
        currentRanges.Sort();

        stopwatch.Stop();
        Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
        Console.WriteLine($"Result: {currentRanges[0].getStart()}");

    }

    public class Range : IComparable<Range> {

        private Double start;
        private Double end;
        private Double offset;


        
        public Range(Double start, Double end){
            this.start = start;
            this.end = end;
            this.offset = 0;
        }

        public Range(Double offset, Double start, Double size){
            this.start = start;
            this.end = start + size;
            this.offset = offset;
        }

        public Double getStart(){
            return this.start;
        }

        public Double getEnd(){
            return this.end;
        }

        public override string ToString(){
            return $"Range({start}, {end})";
        }

        public Range OffsetRange(Range range){
            return new Range(range.start - this.start + offset, range.end - this.start + offset);
        }

        public Boolean Contains(Double value){
            if((value >= start) && (value < end)){
                return true;
            } else {
                return false;
            }
        }
        
        public int CompareTo(Range other){
            if(other == null) return 1;

            int startComparison = this.start.CompareTo(other.start);
            if (startComparison != 0) {
                return startComparison;
            } else {
                return this.end.CompareTo(other.end);
            }
        }

        public Range Intersection(Range range2){
            Double newStart = Math.Max(this.start, range2.start);
            Double newEnd = Math.Min(this.end, range2.end);
            if(newStart <= newEnd){
                return new Range(newStart, newEnd);
            } else {
                return null;
            }
            
        }

        public static List<Range> Union(Range range1, Range range2){
            // Case where the second range is entirely within the first range.
            if ((range2.start >= range1.start) && (range2.end <= range1.end)){
                return new List<Range> { range1 };
            }

            // Case where the first range is entirely within the second range.
            if ((range2.start < range1.start) && (range2.end > range1.end)){
                return new List<Range> { range2 };
            }

            // Case where the second range starts within the first range.
            if ((range2.start <= range1.end) && (range2.start >= range1.start)){
                return new List<Range> { new Range(range1.start, Math.Max(range1.end, range2.end)) };
            }

            // Case where the second range ends within the first range.
            if ((range2.end >= range1.start) && (range2.end <= range1.end)){
                return new List<Range> { new Range(Math.Min(range1.start, range2.start), range1.end) };
            }

            // Case where the ranges do not overlap at all.
            return new List<Range> { range1, range2 };
        }
    }
}