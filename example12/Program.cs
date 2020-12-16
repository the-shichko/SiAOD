using System;

namespace example12
{
    class Program
    {
        static void Main(string[] args)
        {
            var graf = new[,] {
            {0,1,1,1,0,0,0,0},
            {0,0,0,1,1,0,0,0},
            {0,0,0,0,0,0,1,0},
            {0,0,0,0,0,0,1,0},
            {0,0,0,0,0,1,0,1},
            {0,0,0,0,0,0,1,0},
            {0,0,0,0,0,0,0,1},
            {0,0,0,0,0,0,0,0},
            };
            Console.WriteLine("Пути в графе");
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (graf[i, j] == 1)
                    {
                        Console.Write($"{i + 1}-{j + 1}\t");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("Выберите вершину от 1 до 8");
            var v = Convert.ToInt32(Console.ReadLine()) - 1;
            Console.WriteLine($"Смежные вершины с вершиной {v+1}");
            for (var i = 0; i < 8; i++)
            {
                    if (graf[i, v] == 1)
                    {
                        Console.Write($"{i+1}\t");
                    }               
            }
            for (var i = 0; i < 8; i++)
            {
                if (graf[v, i] == 1)
                {
                    Console.Write($"{i + 1}\t");
                }
            }
            Console.WriteLine($"\nСписок вершин из которых можно попасть в вершину {v + 1}");
            for (var i = 0; i < 8; i++)
            {
                if (graf[i, v] == 1)
                {
                    Console.Write($"{i + 1}\t");
                }
            }
            Console.WriteLine($"\nИндексы смежных вершин с вершиной {v + 1}");
            for (var i = 0; i < 8; i++)
            {
                if (graf[i, v] == 1)
                {
                    Console.Write($"{i}\t");
                }
            }
            for (var i = 0; i < 8; i++)
            {
                if (graf[v, i] == 1)
                {
                    Console.Write($"{i}\t");
                }
            }
            Console.WriteLine("\nВыберите индекс интересующей вершины");
            Console.WriteLine($"Номер вершины = {Convert.ToInt32(Console.ReadLine())+1} ");
            Console.ReadKey();
        }
    }
}