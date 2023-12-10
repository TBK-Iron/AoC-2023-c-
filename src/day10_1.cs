using System.IO.Compression;

class Day10_1 {

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

        HashSet<Pos> visited = new HashSet<Pos>();
        Queue<Pipe> queue = new Queue<Pipe>();
        queue.Enqueue(pipes[startPos]);
        visited.Add(startPos);
        int steps = 0;

        bool searching = true;


        while(searching && (queue.Count != 0)){
            steps++;
            Queue<Pipe> tempQueue = new Queue<Pipe>();
            HashSet<Pos> stepVisited = new HashSet<Pos>();
            while(queue.Count != 0){
                Pipe currentPipe = queue.Dequeue();
                
                foreach(Pos pipePos in currentPipe.getAdjacent()){
                    if(stepVisited.Contains(pipePos)){
                        searching = false;
                    }
                    if(!visited.Contains(pipePos)){
                        outputMatrix[pipePos.y][pipePos.x] = steps.ToString().ToCharArray().Last();
                        tempQueue.Enqueue(new Pipe(matrix, pipePos));
                        visited.Add(pipePos);
                        stepVisited.Add(pipePos);
                    }
                    
                    

                    

                }
            }

            queue = tempQueue;
            

            
        }

        foreach(char[] line in outputMatrix){
            Console.WriteLine(string.Join("", line)); 
        }
        Console.WriteLine(steps);
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

        public Pos getPos(){
            return this.position;
        }

        public char getType(){
            return this.type;
        }

    }

    record Pos(int x, int y);
}