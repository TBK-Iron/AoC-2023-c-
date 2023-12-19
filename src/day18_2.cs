using System.Reflection.Metadata;

class Day18_2 {
    public static void Run(){
        List<Planpart> digPlan = File.ReadAllLines(@"../../../src/inputs/input18.txt").Select(line => new Planpart(
                    int.Parse(line.Split(" ")[2].Substring(7, 1)), 
                    Convert.ToInt32(line.Split(" ")[2].Substring(2, 5), 16))
                ).ToList();
        
        long minX = 0;
        long minY = 0;
        long maxX = 0;
        long maxY = 0;

        long x = 0;
        long y = 0;

        List<Pos> corners = new List<Pos>{new Pos(0, 0)};
        int circumferernce = 0;

        foreach(Planpart part in digPlan){
            //Console.WriteLine(x + " " + y);
            circumferernce += part.amount;
            
            if(part.direction == 3 || part.direction == 'U'){
                y -= part.amount;
            } else if(part.direction == 0 || part.direction == 'R'){
                x += part.amount;
            } else if(part.direction == 1 || part.direction == 'D'){
                y += part.amount;
            } else if(part.direction == 2 || part.direction == 'L'){
                x -= part.amount;
            }

            corners.Add(new Pos(x, y));

            minX = Math.Min(minX, x);
            minY = Math.Min(minY, y);
            maxX = Math.Max(maxX, x);
            maxY = Math.Max(maxY, y);
        }

        long innerArea = 0;

        for(int i = 0; i < corners.Count-1; i++){
            Pos p1 = corners[i];
            Pos p2 = corners[i+1];
            if(p1.y == p2.y){
                innerArea += (p2.x - p1.x)*(maxY-p1.y);
            }
        }


        long totalArea = innerArea + (circumferernce-(corners.Count-1))/2 + (corners.Count-1)/2 + 1;

        Console.WriteLine(totalArea);

    }

    public record Planpart(int direction, int amount);

    public record Pos(long x, long y);

    public class Box {
        Pos p1;
        Pos p2;

        public Box(Pos p1, Pos p2){
            if((p2.y < p1.y) || (p2.x < p1.x)){
                throw new Exception("Illegal argument, p2 must be to the lower right of p1");
            }

            this.p1 = p1;
            this.p2 = p2;

        }

        public bool isInBox(Pos pos){
            if((pos.x >= p1.x) && (pos.x <= p2.x) && (pos.y >= p1.y) && (pos.y <= p2.y)){
                return true;
            } else {
                return false;
            }
        }
    }


}