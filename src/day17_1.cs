using System.IO.Compression;
using System.Runtime.ConstrainedExecution;

class Day17_1 {

    private static char movementFrom(Pos p1, Pos p2){
        if(p1.x == p2.x-1){
            return 'E';
        } else if(p1.x == p2.x+1){
            return 'W';
        } else if(p1.y == p2.y-1){
            return 'S';
        } else if(p1.y == p2.y+1){
            return 'N';
        } else {
            return 'A';
        }
    }

    private static bool Is90Degree(char m1, char m2){
        if(m1 == m2){
            return false;
        } else if(m1 == 'N' && m2 == 'S'){
            return false;
        } else if(m1 == 'S' && m2 == 'N'){
            return false;
        } else if(m1 == 'W' && m2 == 'E'){
            return false;
        } else if(m1 == 'E' && m2 == 'W'){
            return false;
        }

        return true;
    }

    private static List<List<char>> deepCopy(List<List<char>> list){
        List<List<char>> copiedList = new List<List<char>>(list.Count);
        foreach(List<char> sublist in list){
            copiedList.Add(new List<char>(sublist));
        }

        return copiedList;
    }

    private static List<Pos> Adjacent(Pos pos, int w, int h){
        List<Pos> adjacent = new List<Pos>();
        int x = pos.x;
        int y = pos.y;
        
        if(x > 0){
            adjacent.Add(new Pos(x-1, y));
        }
        if(y > 0){
            adjacent.Add(new Pos(x, y-1));
        }
        if(x < w - 1){
            adjacent.Add(new Pos(x+1, y));
        }
        if(y < h - 1){
            adjacent.Add(new Pos(x, y+1));
        }

        return adjacent;

    }
    public static void Run(){
        char[][] heatGraph = File.ReadAllLines(@"../../../src/inputs/input17.txt").Select(line => line.ToCharArray()).ToArray();

        int height = heatGraph.Length;
        int width = heatGraph[0].Length;

        Pos startPos = new Pos(0, 0);
        Pos endPos = new Pos(width-1, height - 1);

        Dictionary<(Pos, int, char), int> distances = new Dictionary<(Pos, int, char), int>();
        Dictionary<(Pos, int, char), List<List<char>>> sameDirectionMovements = new Dictionary<(Pos, int, char), List<List<char>>>();
        HashSet<(Pos, int, char)> visited = new HashSet<(Pos, int, char)>();

        for(int y = 0; y < heatGraph.Length; y++){
            for(int x = 0; x < heatGraph[y].Length; x++){
                for(int i = 1; i < 4; i++){
                    foreach(char c in "NESW".ToCharArray()){
                        distances.Add((new Pos(x, y), i, c), int.MaxValue);
                    }
                    
                }
            }
        }
        visited.Add((startPos, 1, 'A'));
        distances.Add((startPos, 1, 'A'), 0);
        sameDirectionMovements[(startPos, 1, 'A')] = new List<List<char>>{new List<char>{'A'}};


        PriorityQueue<(Pos, int, char), int> queue = new PriorityQueue<(Pos, int, char), int>();
        queue.Enqueue((startPos, 1, 'A'), 0);
        

        while(queue.Count != 0){
            (Pos currentPos, int currentRank, char currentDir) = queue.Dequeue();

            foreach(Pos adjPos in Adjacent(currentPos, width, height)){
                int newDist = distances[(currentPos, currentRank, currentDir)] + heatGraph[adjPos.y][adjPos.x] - 48;
                char newDir = movementFrom(currentPos, adjPos);
                if(currentDir != newDir){
                    if(Is90Degree(currentDir, newDir)){
                        if(!visited.Contains((adjPos, 1, newDir))){
                            if(distances[(adjPos, 1, newDir)] > newDist){
                                distances[(adjPos, 1, newDir)] = newDist;
                                sameDirectionMovements[(adjPos, 1, newDir)] = deepCopy(sameDirectionMovements[(currentPos, currentRank, currentDir)]);
                                sameDirectionMovements[(adjPos, 1, newDir)].Add(new List<char>{newDir});
                            }
                            queue.Enqueue((adjPos, 1, newDir), newDist);
                            visited.Add((adjPos, 1, newDir));

                        }
                    }
                        
                        
                } else if(sameDirectionMovements[(currentPos, currentRank, currentDir)].Last().Count < 3){
                    if(!visited.Contains((adjPos,currentRank + 1,currentDir))){
                        if(distances[(adjPos, currentRank + 1, newDir)] > newDist){
                            distances[(adjPos, currentRank + 1, newDir)] = newDist;
                            sameDirectionMovements[(adjPos, currentRank + 1, newDir)] = deepCopy(sameDirectionMovements[(currentPos, currentRank, newDir)]);
                            sameDirectionMovements[(adjPos, currentRank + 1, newDir)].Last().Add(movementFrom(currentPos, adjPos));
                        }
                        queue.Enqueue((adjPos, currentRank + 1, newDir), newDist);
                        visited.Add((adjPos,currentRank + 1, newDir));
                    }
                    
                    
                }
            }
                
        }

        /* foreach(((Pos pos, int rank, char dir), List<List<char>> listPath) in sameDirectionMovements){
            string path = string.Join("", listPath.Select(list => string.Join("", list)));
            Console.WriteLine($"Distance to x={pos.x},y={pos.y} is {distances[(pos, rank, dir)]} when rank={rank} and path is {path}");
        } */

        int lowestDist = int.MaxValue;

        for(int i = 1; i < 4; i++){
            foreach(char c in "NESW"){
                lowestDist = Math.Min(lowestDist, distances[(endPos, i, c)]);
            }
        }

        Console.WriteLine(lowestDist);   
    }

    public record Pos(int x, int y);
}