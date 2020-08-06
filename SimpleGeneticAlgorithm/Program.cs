using System;
using System.IO;
using SimpleGeneticAlgorithm.Genetic;

namespace SimpleGeneticAlgorithm
{
    class Program
    {
        public static int A { get => 4; }
        public static int B { get => 7; }
        public static int C { get => 2; }
        public static int IterationCount { get => 40; }
        public static int PopulationCount { get => 15; }
        public static int PopulationMembers { get => 10; }
        public static int CrossChance { get => 80; }
        public static int MutationChance{ get => 10; }
        public static int MaxDataNumber { get => 255; }

        private static Population[] _populations = new Population[PopulationCount]; 
        static void Main(string[] args)
        {
            string path = @".\" + DateTime.Now.ToString("h_mm_ss") + ".txt"; // Path do pliku tekstowego

            string content = ""; // Zawartosc pliku tekstowego

            var population = new Population(0);

            Member bestMemberRef = null;

            for(int i = 0; i < Program.IterationCount; i++)
            {
                for (int j = 0; j < PopulationCount; j++)
                    bestMemberRef = population.Run();
                bestMemberRef.Resolve(Program.A, Program.B, Program.C);
                content += $"f({bestMemberRef.Data}), {bestMemberRef.Result}" + '\n';
            }

            using (StreamWriter sw = File.CreateText(path)) // Zapisuje zawartość do pliku tekstowego
            {
                sw.Write(content);
            }

        }
    }
}
