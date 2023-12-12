using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;

class Day10_2 {

    private static char[][] LineWithDots(char[][] matrix){
        int width = matrix[0].Length;
        int height = matrix.Length;
        char[][] newMatrix = new char[height + 2][];
        newMatrix[0] = Enumerable.Repeat('.', width + 2).ToArray();
        newMatrix[height + 1] = Enumerable.Repeat('.', width + 2).ToArray();

        for(int y = 1; y < height + 1; y++){
            newMatrix[y] = new char[width + 2];
            newMatrix[y][0] = '.';
            newMatrix[y][width + 1] = '.';

            Array.Copy(matrix[y - 1], 0, newMatrix[y], 1, width);
            
        }

        return newMatrix;

    }
    public static void Run(){
        char[][] matrix = LineWithDots(File.ReadAllLines(@"../../../src/inputs/input10.txt").Select(line => line.ToCharArray()).ToArray());
        char[][] outputMatrix = LineWithDots(File.ReadAllLines(@"../../../src/inputs/input10.txt").Select(line => line.ToCharArray()).ToArray());

        Dictionary<Pos, Pipe> pipes = new Dictionary<Pos, Pipe>();

        Pos startPos = new Pos(0, 0);

        for(int y = 0; y < matrix.Length; y++){
            for(int x = 0; x < matrix[y].Length; x++){
                pipes.Add(new Pos(x, y), new Pipe(matrix, new Pos(x, y)));
                if(matrix[y][x] == 'S'){
                    startPos = new Pos(x, y);
                }
            }
        }

        HashSet<Pos> visited = new HashSet<Pos>{startPos};

        LinkedList<Pos> currentPath = new LinkedList<Pos>();
        currentPath.AddLast(startPos);
        
        bool loopFound = false;
        while(!loopFound){
           
            Pipe currentPipe = pipes[currentPath.Last()];
            bool needReversal = true;
            foreach(Pos pipePos in currentPipe.getAdjacent()){
                if(pipePos.Equals(startPos) && (currentPath.Count > 2)){
                    loopFound = true;
                    needReversal = false;
                    break;
                }
                if(!visited.Contains(pipePos)){
                    visited.Add(pipePos);
                    currentPath.AddLast(pipePos);
                    needReversal = false;
                    break;
                }
            }
            if(needReversal){
                currentPath.RemoveLast();
            }
        }

        HashSet<Pos> coloredPositions = new HashSet<Pos>();

        HashSet<Pos> pathPositions = new HashSet<Pos>(currentPath);

        matrix[startPos.y][startPos.x] = '7';
        pipes[startPos] = new Pipe(matrix, startPos);

        Pipe prevPipe = pipes[currentPath.Last()];
        int side = 2;

        foreach(Pos pipePos in currentPath){
            var pipeInfo = pipes[pipePos].getInners(prevPipe, side);

            foreach(Pos pos in pipeInfo.Item2){
                if(!pathPositions.Contains(pos)){
                    coloredPositions.Add(pos);
                }
            }
            prevPipe = pipes[pipePos];
            side = pipeInfo.Item1;
        }

        Queue<Pos> coloredPositionQueue = new Queue<Pos>(coloredPositions);

        while(coloredPositionQueue.Count != 0){
            Pos currentPos = coloredPositionQueue.Dequeue();
            int x = currentPos.x;
            int y = currentPos.y;
            if(!pathPositions.Contains(new Pos(x-1, y)) && !coloredPositions.Contains(new Pos(x-1, y))){
                coloredPositionQueue.Enqueue(new Pos(x-1, y));
                coloredPositions.Add(new Pos(x-1, y));
            }

            if(!pathPositions.Contains(new Pos(x+1, y)) && !coloredPositions.Contains(new Pos(x+1, y))){
                coloredPositionQueue.Enqueue(new Pos(x+1, y));
                coloredPositions.Add(new Pos(x+1, y));
            }

            if(!pathPositions.Contains(new Pos(x, y-1)) && !coloredPositions.Contains(new Pos(x, y-1))){
                coloredPositionQueue.Enqueue(new Pos(x, y-1));
                coloredPositions.Add(new Pos(x, y-1));
            }

            if(!pathPositions.Contains(new Pos(x, y+1)) && !coloredPositions.Contains(new Pos(x, y+1))){
                coloredPositionQueue.Enqueue(new Pos(x, y+1));
                coloredPositions.Add(new Pos(x, y+1));
            }
        }


        foreach(Pos pos in coloredPositions){
            outputMatrix[pos.y][pos.x] = 'I';
        }

        foreach(char[] line in outputMatrix){
            Console.WriteLine(string.Join("", line)); 
        }

        Console.WriteLine(coloredPositions.Count());

    }


    class Pipe {
        Pos position;
        Pos[] adjacentPipes;
        char type;
        public Pipe(char[][] matrix, Pos pos){
            this.position = pos;
            this.type = matrix[pos.y][pos.x];

            int x = pos.x;
            int y = pos.y;

            Pos[] pipesPointedTo = pointingFrom(this.type, pos);

            List<Pos> adjacentPipes = new List<Pos>();

            foreach(Pos pipePos in pipesPointedTo){
                HashSet<Pos> pipesPointedFrom = new HashSet<Pos>(pointingFrom(matrix[pipePos.y][pipePos.x], pipePos));

                if(pipesPointedFrom.Contains(pos)){
                    adjacentPipes.Add(pipePos);
                }
            }

            this.adjacentPipes = adjacentPipes.ToArray();

            

        }

        private static Pos[] pointingFrom(char type, Pos pos){
            Pos[] pointedTo;
            int x = pos.x;
            int y = pos.y;

            if(type == 'S'){
                pointedTo = new Pos[]{
                    new Pos(x - 1, y),
                    new Pos(x, y - 1),
                    new Pos(x + 1, y),
                    new Pos(x, y + 1)
                };
            } else if(type == '|'){
                pointedTo = new Pos[]{
                    new Pos(x, y - 1),
                    new Pos(x, y + 1)
                };
            } else if(type == '-'){
                pointedTo = new Pos[]{
                    new Pos(x - 1, y),
                    new Pos(x + 1, y)
                };
            } else if(type == 'L'){
                pointedTo = new Pos[]{
                    new Pos(x, y - 1),
                    new Pos(x + 1, y)
                };
            } else if(type == 'J'){
                pointedTo = new Pos[]{
                    new Pos(x, y - 1),
                    new Pos(x - 1, y)
                };
            } else if(type == '7'){
                pointedTo = new Pos[]{
                    new Pos(x - 1, y),
                    new Pos(x, y + 1)
                };
            } else if(type == 'F'){
                pointedTo = new Pos[]{
                    new Pos(x + 1, y),
                    new Pos(x, y + 1)
                };
            } else {
                pointedTo = new Pos[]{};
            }

            return pointedTo;
        }

        public Pos[] getAdjacent(){
            return this.adjacentPipes;
        }

        public (int, Pos[]) getInners(Pipe prevPipe, int side){
            int x = this.position.x;
            int y = this.position.y;

            if(this.type == '|'){
                if(side == 1){
                    return (1, new Pos[]{new Pos(x-1, y)});
                } else {
                    return (2, new Pos[]{new Pos(x+1, y)});
                }
            } 

            if(this.type == '-'){
                if((prevPipe.type == '-') || (prevPipe.type == 'F') || (prevPipe.type == 'J')){
                    if(side == 1){
                        return (1, new Pos[]{new Pos(x, y-1)});
                    } else {
                        return (2, new Pos[]{new Pos(x, y+1)});
                    }
                }
                if((prevPipe.type == 'L') || (prevPipe.type == '7')){
                    if(side == 1){
                        return (2, new Pos[]{new Pos(x, y+1)});
                    } else {
                        return (1, new Pos[]{new Pos(x, y-1)});
                    }
                }
            }

            if(this.type == 'F'){
                if((prevPipe.type == 'J') || (prevPipe.type == '-') || (prevPipe.type == '|') || (prevPipe.type == 'L')){
                    if(side == 1){
                        return (1, new Pos[]{new Pos(x-1, y), new Pos(x, y-1)});
                    } else {
                        return (2, new Pos[]{});
                    }
                }
                if(prevPipe.type == '7'){
                    if(side == 1){
                        return (2, new Pos[]{});
                    } else {
                        return (1, new Pos[]{new Pos(x-1, y), new Pos(x, y-1)});
                    }
                }
            }

            if(this.type == 'L'){
                if((prevPipe.type == '|') || (prevPipe.type == 'F') || (prevPipe.type == '7')){
                    if(side == 1){
                        return (1, new Pos[]{new Pos(x-1, y), new Pos(x, y+1)});
                    } else {
                        return (2, new Pos[]{});
                    }
                }
                if((prevPipe.type == '-') || (prevPipe.type == 'J')){
                    if(side == 1){
                        return (2, new Pos[]{});
                    } else {
                        return (1, new Pos[]{new Pos(x-1, y), new Pos(x, y+1)});
                    }
                }
            }

            if(this.type == 'J'){
                if((prevPipe.type == '|') || (prevPipe.type == '-') || (prevPipe.type == 'F') || (prevPipe.type == '7')){
                    if(side == 1){
                        return (1, new Pos[]{});
                    } else {
                        return (2, new Pos[]{new Pos(x+1, y), new Pos(x, y+1)});
                    }
                }
                if(prevPipe.type == 'L'){
                    if(side == 1){
                        return (2, new Pos[]{new Pos(x+1, y), new Pos(x, y+1)});
                    } else {
                        return (1, new Pos[]{});
                    }
                }
            }

            if(this.type == '7'){
                if((prevPipe.type == 'L') || (prevPipe.type == '|') || (prevPipe.type == 'J')){
                    if(side == 1){
                        return (1, new Pos[]{});
                    } else {
                        return (2, new Pos[]{new Pos(x+1, y), new Pos(x, y-1)});
                    }
                }
                if((prevPipe.type == '-') || (prevPipe.type == 'F')){
                    if(side == 1){
                        return (2, new Pos[]{new Pos(x+1, y), new Pos(x, y-1)});
                    } else {
                        return (1, new Pos[]{});
                    }
                }
            }

            //should never happen
            return (1, new Pos[]{});
        }

    }

    record Pos(int x, int y);
}