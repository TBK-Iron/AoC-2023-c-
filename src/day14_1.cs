using System.Runtime.CompilerServices;

class Day14_1 {

    public static void moveRockUp(Pos rockPos, char[][] map){
        int minY = rockPos.y;
        for(int y = rockPos.y-1; y >= 0; y--){
            if(map[y][rockPos.x] == '.'){
                minY = y;
            } else {
                break;
            }
        }

        map[rockPos.y][rockPos.x] = '.';
        map[minY][rockPos.x] = 'O';
    }

    public static int countRound(char[] line){
        int count = 0;
        for(int i = 0; i < line.Length; i++){
            if(line[i] == 'O'){
                count++;
            }
        }

        return count;
    }

    public static void Run(){
        char[][] dishRocks = File.ReadAllLines(@"../../../src/inputs/input14.txt").Select(line => line.ToCharArray()).ToArray();

        List<Pos> roundRockPositions = new List<Pos>();

        for(int y = 0; y < dishRocks.Length; y++){
            for(int x = 0; x < dishRocks[y].Length; x++){
                if(dishRocks[y][x] == 'O'){
                    roundRockPositions.Add(new Pos(x, y));
                }
            }
        }

        foreach(Pos roundRock in roundRockPositions){
            moveRockUp(roundRock, dishRocks);
        }

        int sum = 0;

        for(int y = 0; y < dishRocks.Length; y++){
            sum += countRound(dishRocks[y]) * (dishRocks.Length - y);
        }

        Console.WriteLine(sum);


    }

    public record Pos(int x, int y);
}