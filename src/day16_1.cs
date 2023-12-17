class Day16_1 {

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
    public static void Run(){
        char[][] contraptionLayout = File.ReadAllLines(@"../../../src/inputs/input16.txt").Select(line => line.ToCharArray()).ToArray();

        char[][] energizedTiles = new char[contraptionLayout.Length][];

        for(int i = 0; i < contraptionLayout.Length; i++){
            energizedTiles[i] = new char[contraptionLayout[i].Length];
            for(int j = 0; j < contraptionLayout[i].Length; j++){
                energizedTiles[i][j] = '.';
            }
        }

        List<Lightbeam> activeBeams = new List<Lightbeam>{new Lightbeam(new Pos(-1, 0), 1, 0)};

        int notChangedIn = 0;

        while(notChangedIn < 10){
            List<Lightbeam> newLightBeams = new List<Lightbeam>();

            int origNotChangedIn = notChangedIn;
            
            notChangedIn++;
            foreach(Lightbeam activeBeam in activeBeams){
                if(activeBeam.getPos().x >= 0){
                    if(energizedTiles[activeBeam.getPos().y][activeBeam.getPos().x] != '#'){
                        energizedTiles[activeBeam.getPos().y][activeBeam.getPos().x] = '#';
                        notChangedIn = origNotChangedIn;
                    }
                }   
                
                newLightBeams.AddRange(getBeamsAfterMove(activeBeam, contraptionLayout));
            }

            activeBeams = newLightBeams;
        }


        int sum = 0;

        foreach(char[] line in energizedTiles){
            for(int i = 0; i < line.Length; i++){
                if(line[i] == '#'){
                    sum++;
                }
            }
        }

        Console.WriteLine(sum);


    }

    public class Lightbeam{
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