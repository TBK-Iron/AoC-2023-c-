using System.Data;
using System.Text.RegularExpressions;

class Day7_2 {
    public static void Run(){

        string[] lines = File.ReadAllLines(@"../../../src/inputs/input7.txt");

        List<KeyValuePair<string, int>> gameBids = new List<KeyValuePair<string, int>>();

        foreach(string line in lines){
            gameBids.Add(new KeyValuePair<string, int>(line.Split(" ")[0], int.Parse(line.Split(" ")[1])));
        }

        gameBids.Sort(new HandComparer());

        Console.WriteLine("");

        int winnings = 0;

        for(int i = 0; i < gameBids.Count; i++){
            Console.WriteLine($"Rank {i + 1}: {gameBids[i].Key} {gameBids[i].Value}");
            winnings += gameBids[i].Value * (i+1);
        }

        Console.WriteLine(winnings);

    }

    public class HandComparer : IComparer<KeyValuePair<string, int>> {

        private static Dictionary<char, int> cardValues = new Dictionary<char, int>{
            {'2', 2},
            {'3', 3},
            {'4', 4},
            {'5', 5},
            {'6', 6},
            {'7', 7},
            {'8', 8},
            {'9', 9},
            {'T', 10},
            {'J', 1},
            {'Q', 12},
            {'K', 13},
            {'A', 14},
        };

        private static int countJokers(string hand){
            int jokers = 0;
            foreach(char c in hand.ToCharArray()){
                if(c == 'J'){
                    jokers++;
                }
            }

            return jokers;
        }

        public static bool hasOnePair(string hand){
            Dictionary<char, int> charCounts = new Dictionary<char, int>();
            int jokers = countJokers(hand);

            foreach(char c in hand.ToCharArray()){
                if(!charCounts.ContainsKey(c)){
                    if(c == 'J'){
                        charCounts[c] = 0;
                    } else {
                        charCounts[c] = jokers;
                    }
                    
                }
                charCounts[c]++;
            }
            if(charCounts.Values.Contains(2)){
                return true;
            }
            return false;
        }

        public static bool hasTwoPair(string hand){
            HashSet<char> seen = new HashSet<char>();
            Dictionary<char, int> charCounts = new Dictionary<char, int>();
            int pairsCount = 0;

            int jokers = countJokers(hand);

            foreach(char c in hand.ToCharArray()){
                if(!charCounts.ContainsKey(c)){
                    if(c == 'J'){
                        charCounts[c] = 0;
                    } else {
                        charCounts[c] = jokers;
                    }
                }
                if(seen.Contains(c)){
                    if(charCounts[c] == 1){
                        pairsCount++;
                    }
                }
                seen.Add(c);
                charCounts[c]++;
            }
            
            return pairsCount >= 2;
        }

        public static bool hasThreeKind(string hand){
            Dictionary<char, int> charCounts = new Dictionary<char, int>();
            int jokers = countJokers(hand);

            foreach(char c in hand.ToCharArray()){
                if(!charCounts.ContainsKey(c)){
                    if(c == 'J'){
                        charCounts[c] = 0;
                    } else {
                        charCounts[c] = jokers;
                    }
                    
                }
                charCounts[c]++;
            }
            if(charCounts.Values.Contains(3)){
                return true;
            }
            return false;
        }

        public static bool hasFullHouse(string hand){
            Dictionary<char, int> charCounts = new Dictionary<char, int>();
            int jokers = countJokers(hand);

            foreach(char c in hand.ToCharArray()){
                if(!charCounts.ContainsKey(c)){
                    charCounts[c] = 0;
                }
                charCounts[c]++;
            }

            if(charCounts.Values.Contains(3) && charCounts.Values.Contains(2)){
                return true;
            } else if(jokers == 1){
                HashSet<char> seen = new HashSet<char>();
                Dictionary<char, int> charCounts2 = new Dictionary<char, int>();
                int pairsCount = 0;

                foreach(char c in hand.ToCharArray()){
                    if(c == 'J'){
                        continue;
                    }
                    if(!charCounts2.ContainsKey(c)){
                        charCounts2[c] = 0;
                    }
                    if(seen.Contains(c)){
                        if(charCounts2[c] == 1){
                            pairsCount++;
                        }
                    }
                    seen.Add(c);
                    charCounts2[c]++;
                }
                if(pairsCount == 2){
                    return true;
                }
            }
            return false;
        }

        public static bool hasFourKind(string hand){
            Dictionary<char, int> charCounts = new Dictionary<char, int>();
            int jokers = countJokers(hand);

            foreach(char c in hand.ToCharArray()){
                if(!charCounts.ContainsKey(c)){
                    if(c == 'J'){
                        charCounts[c] = 0;
                    } else {
                        charCounts[c] = jokers;
                    }
                }
                charCounts[c]++;
            }
            if(charCounts.Values.Contains(4)){
                return true;
            }
            return false;
        }

        public static bool hasFiveKind(string hand){
            Dictionary<char, int> charCounts = new Dictionary<char, int>();
            int jokers = countJokers(hand);

            foreach(char c in hand.ToCharArray()){
                if(!charCounts.ContainsKey(c)){
                    if(c == 'J'){
                        charCounts[c] = 0;
                    } else {
                        charCounts[c] = jokers;
                    }

                }
                charCounts[c]++;
            }
            if(charCounts.Values.Contains(5)){
                return true;
            }
            return false;
        }

        public static int compareFirstCards(string hand1, string hand2){
            for(int i = 0; i < hand1.Length; i++){
                if(cardValues[hand1.ToCharArray()[i]] > cardValues[hand2.ToCharArray()[i]]){
                    return 1;
                } else if(cardValues[hand1.ToCharArray()[i]] < cardValues[hand2.ToCharArray()[i]]){
                    return -1;
                } 
            }
            return 0;
        }

        public int Compare(KeyValuePair<string, int> x, KeyValuePair<string, int> y) {
            string hand1 = x.Key;
            string hand2 = y.Key;

            //Console.WriteLine(hand1 + " " + hand2);

            if(hasFiveKind(hand1) && !hasFiveKind(hand2)){
                return 1;
            } else if(!hasFiveKind(hand1) && hasFiveKind(hand2)){
                return -1;
            } else if(hasFiveKind(hand1) && hasFiveKind(hand2)){
                return compareFirstCards(hand1, hand2);
            }

            if(hasFourKind(hand1) && !hasFourKind(hand2)){
                return 1;
            } else if(!hasFourKind(hand1) && hasFourKind(hand2)){
                return -1;
            } else if(hasFourKind(hand1) && hasFourKind(hand2)){
                return compareFirstCards(hand1, hand2);
            }

            if(hasFullHouse(hand1) && !hasFullHouse(hand2)){
                return 1;
            } else if(!hasFullHouse(hand1) && hasFullHouse(hand2)){
                return -1;
            } else if(hasFullHouse(hand1) && hasFullHouse(hand2)){
                return compareFirstCards(hand1, hand2);
            }

            if(hasThreeKind(hand1) && !hasThreeKind(hand2)){
                return 1;
            } else if(!hasThreeKind(hand1) && hasThreeKind(hand2)){
                return -1;
            } else if(hasThreeKind(hand1) && hasThreeKind(hand2)){
                return compareFirstCards(hand1, hand2);
            }

            if(hasTwoPair(hand1) && !hasTwoPair(hand2)){
                return 1;
            } else if(!hasTwoPair(hand1) && hasTwoPair(hand2)){
                return -1;
            } else if(hasTwoPair(hand1) && hasTwoPair(hand2)){
                return compareFirstCards(hand1, hand2);
            }

            if(hasOnePair(hand1) && !hasOnePair(hand2)){
                return 1;
            } else if(!hasOnePair(hand1) && hasOnePair(hand2)){
                return -1;
            } else if(hasOnePair(hand1) && hasOnePair(hand2)){
                return compareFirstCards(hand1, hand2);
            }

            return compareFirstCards(hand1, hand2);
        }
    }
}