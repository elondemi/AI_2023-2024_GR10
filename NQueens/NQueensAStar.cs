

public class NQueensAStar
{
    private class BoardState : IComparable<BoardState>
    {
        public int[] Queens { get; }
        public int StepCost { get; private set; }
        private int heuristic;

        public BoardState(int n)
        {
            Queens = new int[n];
            for (int i = 0; i < n; i++)
            {
                Queens[i] = -1;
            }
            StepCost = 0;
            heuristic = CalculateHeuristic();
        }

        private BoardState(int[] queens, int stepCost)
        {
            Queens = queens;
            StepCost = stepCost;
            heuristic = CalculateHeuristic();
        }

        public bool IsValid()
        {
            for (int i = 0; i < Queens.Length; i++)
            {
                if (Queens[i] == -1) continue;

                for (int j = i + 1; j < Queens.Length; j++)
                {
                    if (Queens[j] == -1) continue;
                    if (Queens[i] == Queens[j] || Math.Abs(Queens[i] - Queens[j]) == Math.Abs(i - j))
                        return false;
                }
            }
            return true;
        }


        public int CalculateHeuristic()
        {
            // Admissible heuristic
            return Queens.Count(q => q == -1);
            //int h = 0;
            //for (int i = 0; i < Queens.Length; i++)
            //{
            //    if (Queens[i] == -1) continue;

            //    for (int j = i + 1; j < Queens.Length; j++)
            //    {
            //        if (Queens[j] == -1) continue;
            //        if (Queens[i] == Queens[j] || Math.Abs(Queens[i] - Queens[j]) == Math.Abs(i - j))
            //            h++;
            //    }
            //}
            //return h;
        }

        public int CompareTo(BoardState other)
        {
            return (StepCost + heuristic).CompareTo(other.StepCost + other.heuristic);
        }

        public BoardState CreateNextState(int column, int row)
        {
            int[] newQueens = (int[])Queens.Clone();
            newQueens[column] = row;
            return new BoardState(newQueens, StepCost + 1);
        }
    }

    public static int[] Solve(int n)
    {
        var openSet = new PriorityQueue<BoardState, int>();

        openSet.Enqueue(new BoardState(n), 0);

        // this is implmentin A* 
        while (openSet.Count > 0)
        {
            var currentState = openSet.Dequeue();

            if (currentState.IsValid() && currentState.Queens.All(q => q != -1))
                return currentState.Queens;

            for (int i = 0; i < n; i++)
            {
                if (currentState.Queens[i] == -1)
                {
                    for (int j = 0; j < n; j++)
                    {
                        var newState = currentState.CreateNextState(i, j);
                        if (newState.IsValid())
                        {
                            openSet.Enqueue(newState, newState.StepCost + newState.CalculateHeuristic());
                        }
                    }
                    break;
                }
            }
        }
        return null;
    }
}

public static class Extensions
{
    public static int SequenceCompareTo(this int[] seq1, int[] seq2)
    {
        for (int i = 0; i < Math.Min(seq1.Length, seq2.Length); i++)
        {
            int comparison = seq1[i].CompareTo(seq2[i]);
            if (comparison != 0) return comparison;
        }
        return seq1.Length.CompareTo(seq2.Length);
    }
}

class Program1
{
    static void Main()
    {
        int n = 8;
        int[] solution = NQueensAStar.Solve(n);
        if (solution != null)
        {
            Console.WriteLine($"Solution for {n} queens: [{string.Join(", ", solution)}]");
            PrintBoard(solution);
        }
        else
        {
            Console.WriteLine("No solution found.");
        }
    }

    static void PrintBoard(int[] queens)
    {
        for (int i = 0; i < queens.Length; i++)
        {
            for (int j = 0; j < queens.Length; j++)
            {
                if (queens[j] == i)
                {
                    Console.Write("Q ");
                }
                else
                {
                    Console.Write(". ");
                }
            }
            Console.WriteLine();
        }
    }
}
