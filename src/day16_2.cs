using System.Diagnostics;
using System.IO.Compression;

class Day16_2 {

    public static List<Lightbeam> getBeamsAfterMove(Lightbeam beam, char[][] layout){
        Lightbeam newBeam = beam.moved();
        int x = newBeam.getPos().x;
        int y = newBeam.getPos().y;
        if(x < 0 || x > layout[0].Length - 1){
            return new List<Lightbeam>{};
        }
        if(y < 0 || y > layout.Length - 1){
            return new List<Lightbeam>{};
        }

        if(layout[y][x] == '.'){
            return new List<Lightbeam>{newBeam};
        }
        if(layout[y][x] == '/'){
            if(beam.getDeltaX() == 1){
                return new List<Lightbeam>{new Lightbeam(newBeam.getPos(), 0, -1)};
            } else if(beam.getDeltaX() == -1) {
                return new List<Lightbeam>{new Lightbeam(newBeam.getPos(), 0, 1)};
            } else if(beam.getDeltaY() == 1){
                return new List<Lightbeam>{new Lightbeam(newBeam.getPos(), -1, 0)};
            } else {
                return new List<Lightbeam>{new Lightbeam(newBeam.getPos(), 1, 0)};
            }
        }
        if(layout[y][x] == '\\'){
            if(beam.getDeltaX() == 1){
                return new List<Lightbeam>{new Lightbeam(newBeam.getPos(), 0, 1)};
            } else if(beam.getDeltaX() == -1) {
                return new List<Lightbeam>{new Lightbeam(newBeam.getPos(), 0, -1)};
            } else if(beam.getDeltaY() == 1){
                return new List<Lightbeam>{new Lightbeam(newBeam.getPos(), 1, 0)};
            } else {
                return new List<Lightbeam>{new Lightbeam(newBeam.getPos(), -1, 0)};
            }
        }

        if(layout[y][x] == '-'){
            if(beam.getDeltaX() != 0){
                return new List<Lightbeam>{newBeam};
            } else {
                return new List<Lightbeam>{
                    new Lightbeam(newBeam.getPos(), -1, 0),
                    new Lightbeam(newBeam.getPos(), 1, 0)
                };
            }
        }

        if(layout[y][x] == '|'){
            if(beam.getDeltaY() != 0){
                return new List<Lightbeam>{newBeam};
            } else {
                
                return new List<Lightbeam>{
                    new Lightbeam(newBeam.getPos(), 0, -1),
                    new Lightbeam(newBeam.getPos(), 0, 1)
                };
            }
        }

        return new List<Lightbeam>{};
        
    }

    public static Boolean IsWithinBounds(Pos pos, int x0, int y0, int x1, int y1){
        if(pos.x < x0){
            return false;
        }

        if(pos.x >= x1){
            return false;
        }

        if(pos.y < y0){
            return false;
        }

        if(pos.y >= y1){
            return false;
        }

        return true;
    }


    static Dictionary<(Pos, int, int), HashSet<Pos>> memoization = new Dictionary<(Pos, int, int), HashSet<Pos>>();
    private static HashSet<Pos> energizedPositionsFromBeam(Lightbeam startBeam, char[][] contraptionLayout, HashSet<(Pos, int, int)> path, int loopIteration){
        HashSet<Pos> result = null;
        if(memoization.TryGetValue((startBeam.getPos(), startBeam.getDeltaX(), startBeam.getDeltaY()), out result)){
            return new HashSet<Pos>(result);
        }
        List<Lightbeam> newBeams = getBeamsAfterMove(startBeam, contraptionLayout);
      
        if(path.Contains((startBeam.getPos(), startBeam.getDeltaX(), startBeam.getDeltaY()))){
            if(loopIteration >= 1){
                return new HashSet<Pos>{};
            } else {
                loopIteration++;
                path = new HashSet<(Pos, int, int)>();
            }
        }

        path.Add((startBeam.getPos(), startBeam.getDeltaX(), startBeam.getDeltaY()));
        
        if(newBeams.Count == 0){
            result = new HashSet<Pos>{startBeam.getPos()};
        } else if(newBeams.Count == 1){
            result = energizedPositionsFromBeam(newBeams[0], contraptionLayout, new HashSet<(Pos, int, int)>(path), loopIteration);

            if(IsWithinBounds(startBeam.getPos(), 0, 0, contraptionLayout[0].Length, contraptionLayout.Length)){
                result.Add(startBeam.getPos());
            }
            
        } else {
            result = energizedPositionsFromBeam(newBeams[0], contraptionLayout, new HashSet<(Pos, int, int)>(path), loopIteration);
            result.UnionWith(energizedPositionsFromBeam(newBeams[1], contraptionLayout, new HashSet<(Pos, int, int)>(path), loopIteration));

            if(IsWithinBounds(startBeam.getPos(), 0, 0, contraptionLayout[0].Length, contraptionLayout.Length)){
                result.Add(startBeam.getPos());
            }
        }
        /* if(!hasLoop){
            memoization.Add((startBeam.getPos(), startBeam.getDeltaX(), startBeam.getDeltaY()), result);
        } */
        //Console.WriteLine($"{startBeam.getPos()} {startBeam.getDeltaX()} {startBeam.getDeltaY()}");
        if(!memoization.ContainsKey((startBeam.getPos(), startBeam.getDeltaX(), startBeam.getDeltaY()))){
            
            memoization.Add((startBeam.getPos(), startBeam.getDeltaX(), startBeam.getDeltaY()), new HashSet<Pos>(result));
        } else {
            //Console.WriteLine($"{result.Count()} {startBeam.getPos()} {startBeam.getDeltaX()} {startBeam.getDeltaY()}");
            //Console.WriteLine(memoization[(startBeam.getPos(), startBeam.getDeltaX(), startBeam.getDeltaY())].Count());
            memoization[(startBeam.getPos(), startBeam.getDeltaX(), startBeam.getDeltaY())] = new HashSet<Pos>(result);
        }
    
        return result;
    }

    public static void Run(){
        
        Thread myThread = new Thread(new ThreadStart(runCode), 1024*1024*10);
        myThread.Start();

    }

    private static void runCode(){
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        char[][] contraptionLayout = File.ReadAllLines(@"../../../src/inputs/input16.txt").Select(line => line.ToCharArray()).ToArray();

        List<Lightbeam> startingBeams = new List<Lightbeam>();
        for(int i = 0; i < contraptionLayout[0].Length; i++){
            startingBeams.Add(new Lightbeam(new Pos(i, -1), 0, 1));
            startingBeams.Add(new Lightbeam(new Pos(i, contraptionLayout.Length), 0, -1));
        }

        for(int i = 0; i < contraptionLayout.Length; i++){
            startingBeams.Add(new Lightbeam(new Pos(-1, i), 1, 0));
            startingBeams.Add(new Lightbeam(new Pos(contraptionLayout[0].Length, i), -1, 0));
        }

        HashSet<Pos>[] energizedPositions = new HashSet<Pos>[startingBeams.Count];

        int bestEnergizedTilesCount = 0;
        int bestI = 0;
        Lightbeam bestStartingBeam = null;

        

        for(int i = 0; i < startingBeams.Count; i++){
            energizedPositions[i] = energizedPositionsFromBeam(startingBeams[i], contraptionLayout, new HashSet<(Pos, int, int)>{}, 0);

            if(energizedPositions[i].Count > bestEnergizedTilesCount){
                bestEnergizedTilesCount = energizedPositions[i].Count;
                bestI = i;
                bestStartingBeam = startingBeams[i];
            }

        }

        stopwatch.Stop();
        Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
        Console.WriteLine($"Result: {bestEnergizedTilesCount}");

    }

    public class Lightbeam {
        Pos pos;
        int deltaX;
        int deltaY;
        public Lightbeam(Pos pos, int deltaX, int deltaY){
            this.pos = pos;
            this.deltaX = deltaX;
            this.deltaY = deltaY;
        }

        public Lightbeam moved(){
            Pos newPos = new Pos(this.pos.x + deltaX, this.pos.y + deltaY);
            return new Lightbeam(newPos, deltaX, deltaY);
        }

        public char getDirectionChar(){
            if(deltaX == -1){
                return '<';
            } else if(deltaX == 1){
                return '>';
            } else if(deltaY == -1){
                return '^';
            } else {
                return 'v';
            }
        }

        public int getDeltaX(){
            return this.deltaX;
        }

        public int getDeltaY(){
            return this.deltaY;
        }


        public Pos getPos(){
            return this.pos;
        }

        

    }

    public record Pos(int x, int y);
}