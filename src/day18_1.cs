using System.Reflection.Metadata;

class Day18_1 {

    public static void floodFill(char[][] board, Pos fromPos, char fill){
        Queue<Pos> toCheck = new Queue<Pos>();
        HashSet<Pos> visited = new HashSet<Pos>();
        toCheck.Enqueue(fromPos);

        Box box = new Box(1, 1, board[0].Length-2, board.Length-2);

        while(toCheck.Count > 0){
            Pos currentPos = toCheck.Dequeue();
            //Console.WriteLine(currentPos);
            if(box.isInBox(currentPos)){
                int x = currentPos.x;
                int y = currentPos.y;
                board[y][x] = '#';
                if(board[y-1][x] == '.' && !visited.Contains(new Pos(x, y-1))){
                    toCheck.Enqueue(new Pos(x, y-1));
                    visited.Add(new Pos(x, y-1));
                }
                if(board[y+1][x] == '.' && !visited.Contains(new Pos(x, y+1))){
                    toCheck.Enqueue(new Pos(x, y+1));
                    visited.Add(new Pos(x, y+1));
                }
                if(board[y][x-1] == '.' && !visited.Contains(new Pos(x-1, y))){
                    toCheck.Enqueue(new Pos(x-1, y));
                    visited.Add(new Pos(x-1, y));
                }
                if(board[y][x+1] == '.' && !visited.Contains(new Pos(x+1, y))){
                    toCheck.Enqueue(new Pos(x+1, y));
                    visited.Add(new Pos(x+1, y));
                }
            }
            
        }
    }
    public static void Run(){
        List<Planpart> digPlan = File.ReadAllLines(@"../../../src/inputs/input18.txt").Select(line => new Planpart(
                    line.ToCharArray()[0], 
                    int.Parse(line.Split(" ")[1]), 
                    line.Split(" ")[2].Replace("(", "").Replace(")", ""))
                ).ToList();
        
        int minX = 0;
        int minY = 0;
        int maxX = 0;
        int maxY = 0;

        int x = 0;
        int y = 0;

        foreach(Planpart part in digPlan){
            if(part.direction == 'U'){
                y -= part.amount;
            } else if(part.direction == 'R'){
                x += part.amount;
            } else if(part.direction == 'D'){
                y += part.amount;
            } else if(part.direction == 'L'){
                x -= part.amount;
            }

            minX = Math.Min(minX, x);
            minY = Math.Min(minY, y);
            maxX = Math.Max(maxX, x);
            maxY = Math.Max(maxY, y);
        }

        char[][] board = new char[-minY + maxY + 1][];
        for(int i = 0; i < board.Length; i++){
            board[i] = Enumerable.Repeat('.', -minX + maxX + 1).ToArray();
            
        }

        Console.WriteLine($"minX={minX} maxX={maxX} minY={minY} maxY={maxY}");


        x = 0;
        y = 0;
        foreach(Planpart part in digPlan){
            for(int i = 0; i < part.amount; i++){
                board[-minY + y][-minX + x] = '#';

                if(part.direction == 'U'){
                    y--;
                } else if(part.direction == 'R'){
                    x++;
                } else if(part.direction == 'D'){
                    y++;
                } else if(part.direction == 'L'){
                    x --;
                }

                
            }
        }

        //File.WriteAllLines(@"../../../src/outputs/output18_1.txt", board.Select(line => string.Join("", line)));

        floodFill(board, new Pos(board[0].Length/2, board.Length/2), '#');

        int sum = 0;

        foreach(char[] line in board){
            for(int i = 0; i < line.Length; i++){
                if(line[i] == '#'){
                    sum++;
                }
            }
            //Console.WriteLine(string.Join("", line));
        }

        File.WriteAllLines(@"../../../src/outputs/output18_1.txt", board.Select(line => string.Join("", line)));

        Console.WriteLine(sum);





    }

    public record Planpart(char direction, int amount, string color);

    public record Pos(int x, int y);

    public class Box {
        Pos p1;
        Pos p2;
        public Box(int x0, int y0, int width, int height){
            this.p1 = new Pos(x0, y0);
            this.p2 = new Pos(x0+width-1, y0+height-1);
        }

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