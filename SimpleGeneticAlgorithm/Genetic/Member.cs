using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleGeneticAlgorithm.Genetic
{
    class Member :ICloneable
    {
        public int Data { get => _data; }
        public int Fitness { get => _fitness; }
        public bool[] DNA { get => _DNA; }
        public int Result { get => _result; }

        private int _result = 0;
        private int _data = 0;
        private int _fitness = 0;
        private bool[] _DNA = new bool[8];

        public Member() // Domyślny konstruktor osobnika w populacji, generuje wszystkie dane
        {
            var numberGenerator = new Random();
            var generatedData = numberGenerator.Next(Program.MaxDataNumber);
            _data = generatedData;
            _DNA = GenerateDNA(_data);
            Resolve(Program.A,Program.B,Program.C);

        }

        public Member(int data, bool[] dna) // Konstruktor używany kiedy nastąpuje krzyżowanie osobników, uwzględnia on mutacje
        {
            _data = data;
            _DNA = dna;
            Mutate(Program.MutationChance);
            Resolve(Program.A, Program.B, Program.C);
        }
        public Member(int data) // Konstruktor testowy
        {
            _data = data;
            _DNA = GenerateDNA(_data);
            Resolve(Program.A, Program.B, Program.C);
        }

        public Member(Member toCopy)
        {
            _data = toCopy.Data;
            _DNA = toCopy.DNA;

            Resolve(Program.A, Program.B, Program.C);
        }

        public Member(Member toCopy, int fitness, int result)
        {
            _data = toCopy.Data;
            _DNA = toCopy.DNA;
            _fitness = fitness;
        }

        public override string ToString() // Prosta serializacja obiektu
        {
            var text = $"Data: {_data}, DNA: ";
            for(var i = 0; i < _DNA.Length; i++)
            {
                if (_DNA[i])
                    text += $" 1, ";
                else
                    text += $" 0, ";
            }
            return text;
        }
        public static (Member, Member) Cross(Member m1, Member m2) // Operator krzyżowania
        {
            var rng = new Random();

            var geneCut = rng.Next(7);
            var mutationChance = rng.Next(Program.CrossChance);

            var dna_m1 = m1.DNA;
            var dna_m2 = m2.DNA;


            var cross_dna = new bool[8];
            var cross_dna_2 = new bool[8];


            for (int i = 0; i < dna_m1.Length; i++)
            {
                if(i <= geneCut)
                {
                    cross_dna[i] = dna_m1[i];
                    cross_dna_2[i] = dna_m2[i];
                }
                else
                {
                    cross_dna[i] = dna_m2[i];
                    cross_dna_2[i] = dna_m1[i];
                }
            }

            var encodedData = new int[] {EnocdeDNA(cross_dna), EnocdeDNA(cross_dna_2)};

            return (new Member(encodedData[0], cross_dna), new Member(encodedData[1], cross_dna_2));
        }

        

        private void Mutate(int mutateChance) //  Mutuje geny i następnie zapisuje dane do zmienej Data
        {
            var rng = new Random();
            for (int i = 0; i < _DNA.Length; i++)
            {
                var chance = rng.Next(100);

                if (chance <= mutateChance)
                {
                    _DNA[i] = !_DNA[i];
                }
            }
            _data = EnocdeDNA(_DNA);
        }

        private bool[] GenerateDNA(int data) // GeneRuje geny na podstawie podanej liczby
        {

            var dna = new bool[8];
            dna[7] = (data % 2) == 1 ? true : false ;
            data = data / 2;

            for(int i = 6; i >= 0; i--)
            {
                var binData = (data % 2) == 1 ? true : false;
                data = data / 2;
                dna[i] = binData;
            }
            return dna;
        }
        
        public static int EnocdeDNA(bool[] DNA) // Zwraca wartość zakodowanych danych w binarce
        {
            var encodedValue = 0.0;

            for(var i = 0; i < DNA.Length; i++)
            {
                if (DNA[i])
                {
                    encodedValue += Math.Pow((double)2, (double)7-i);
                }
                
            }
            return (int)encodedValue;
        }

        public void Resolve(int a, int b, int c) // Wylicza funkcje kwadraowa
        {
            int _x = (int)Math.Pow(Data, 2.0);
            int _a = a * _x;
            int _b = b * Data;
            int result = _a + _b + c;
            _result = result;
        }

        public void Fintess(int functionSum) // Wylicza funkcje celu
        {
            var fitness = (double)_result / (double)functionSum;
            _fitness = (int)(fitness * 100);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
