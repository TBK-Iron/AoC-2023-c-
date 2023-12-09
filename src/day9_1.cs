class Day9_1 {

    private static bool isAllZeroes(long[] array) {
        for(int i = 0; i < array.Length; i++){
            if(array[i] != 0){
                return false;
            }
        }

        return true;
    }
    public static void Run(){
        long[][] histories = File.ReadAllLines(@"../../../src/inputs/input9.txt").Select(line => line.Split(" ").Select(long.Parse).ToArray()).ToArray();

        long sum = 0;

        foreach(long[] history in histories){
            List<long[]> derivatives = new List<long[]>{history};

            while(!isAllZeroes(derivatives.Last())){
                derivatives.Add(new long[derivatives.Last().Length -  1]);

                for(int i = 0; i < derivatives.Last().Length; i++){
                    derivatives.Last()[i] = derivatives[derivatives.Count - 2][i + 1] - derivatives[derivatives.Count - 2][i];
                }
            }

            long currentValue = 0;

            for(int i = derivatives.Count - 1; i > 0; i--){
                currentValue = derivatives[i-1].Last() + currentValue;
            }

            sum += currentValue;

        }

        Console.WriteLine(sum);


    }
}