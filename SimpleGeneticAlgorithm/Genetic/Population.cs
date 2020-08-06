using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleGeneticAlgorithm.Genetic
{
    class Population
    {
        private List<Tuple<Member, Member>> _matingPool = new List<Tuple<Member, Member>>(Program.PopulationMembers / 2);
        private List<Member> _members = new List<Member>(Program.PopulationMembers);
        private List<Member> _tempMembers = new List<Member>(Program.PopulationMembers);
        private List<Member> _selectionPool = new List<Member>();
        private int _populationId = 0;

        public int FunctionSum = 0;
        public Population(int id) // Generuje losowa populacje
        {

            for(int i = 0; i < Program.PopulationMembers; i++)
            {
                _members.Add(new Member());
            }
        }

        private void CreateMatingPool() // Metoda, która tworzy zbiór osobników do krzyżowania
        {
            var rng = new Random();

            if (_members.Count == 0)
                return;

            while(_members.Count != 0)
            {
                var index = rng.Next(_members.Count);
                var parent = new Member(_members[index]);
                _members.RemoveAt(index);

                index = rng.Next(_members.Count);
                var parent_2 = new Member(_members[index]);
                _members.RemoveAt(index);

                _matingPool.Add(new Tuple<Member, Member>(parent, parent_2));
            }
        }

        private void InvokeCross() // Wywołuje krzyżowanie
        {
            foreach(var couple in _matingPool)
            {
                var childs = Member.Cross(couple.Item1, couple.Item2);
                _tempMembers.Add(childs.Item1);
                _tempMembers.Add(childs.Item2);
            }
        }

        private void CalculateFitness() // Oblicza funckje celu dla wszystkich osobnikow w tymczasowej populacji
        {
            foreach (var member in _tempMembers)
                member.Resolve(Program.A, Program.B, Program.C);
            FunctionSum = 0;

            foreach (var member in _tempMembers)
                FunctionSum += member.Result;

            foreach (var member in _tempMembers)
                member.Fintess(FunctionSum);
        }

        private void GenerateSelectionPool() // Generuje zbiór osobników do selekcji po krzyżowaniu i mutacji
        {
            foreach (var member in _tempMembers)
                for (int i = 0; i < member.Fitness; i++)
                    _selectionPool.Add(new Member(member, member.Fitness, member.Result));
        }

        private void ChooseNewPopulation() // Wybiera osobniki do nowej populacji
        {
            var rng = new Random();

            for(int i = 0; i<Program.PopulationMembers; i++)
            {
                var memberRef = _selectionPool[rng.Next(_selectionPool.Count)];
                _members.Add(new Member(memberRef, memberRef.Fitness, memberRef.Result));
            }
      
        }

        private Member GetGreatestMember() // Zwraca najlepszego osobnika w populacji
        {
            Member bestMember = _members[0];

            foreach(var member in _members)
            {
                if (bestMember.Fitness < member.Fitness)
                    bestMember = member;
            }

            return bestMember;
        }
        public void Print()
        {
            Console.WriteLine("========= Population: " + _populationId);

            foreach (var member in _members)
                Console.WriteLine(member.ToString());
        }

        private void Clear() // Czyści niepotrzebne zbiory
        {
            _matingPool.Clear();
            _tempMembers.Clear();
            _selectionPool.Clear();
        }
        public Member Run() // uruchamia algorytm
        {
            Print();
            CreateMatingPool();
            InvokeCross();
            CalculateFitness();
            GenerateSelectionPool();
            ChooseNewPopulation();
            var bestMember = GetGreatestMember();
            Clear();
            return bestMember;
        }
    }
}
