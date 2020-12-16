using System;
using System.Collections.Generic;
using System.Linq;

namespace example13
{
    class Program
    {
        static void Main(string[] args)
        {
            var node1 = new Node("1");
            var node2 = new Node("2");
            var node3 = new Node("3");
            var node4 = new Node("4");
            var node5 = new Node("5");
            var node6 = new Node("6");
            var node7 = new Node("7");
            var node8 = new Node("8");
            var node9 = new Node("9");
            var node10 = new Node("10");
            var node11 = new Node("11");
            var node12 = new Node("12");
            var node13 = new Node("13");
            var node14 = new Node("14");
            var node15 = new Node("15");

            node1.AddChildren(node2).AddChildren(node3);
            node2.AddChildren(node5);
            node3.AddChildren(node4);
            node4.AddChildren(node5, false).AddChildren(node10, false).AddChildren(node11, false);
            node6.AddChildren(node1, false);
            node7.AddChildren(node3, false).AddChildren(node8);
            node9.AddChildren(node8).AddChildren(node10);
            node11.AddChildren(node12).AddChildren(node13);
            node12.AddChildren(node13);
            node14.AddChildren(node15);

            var search = new DepthFirstSearch();
            var path = search.DLS(node6, node13, 6);
            PrintPath(path);
        }

        private static void PrintPath(LinkedList<Node> path)
        {
            Console.WriteLine();
            if (path.Count == 0)
            {
                Console.WriteLine("Ты не пройдешь!");
            }
            else
            {
                Console.WriteLine(string.Join(", ", path.Select(x => x.Name)));
            }

            Console.Read();
        }
    }

    public class Node
    {
        public string Name { get; }
        public List<Node> Children { get; }

        public Node(string name)
        {
            Name = name;
            Children = new List<Node>();
        }

        // добавление узла потомка
        public Node AddChildren(Node node, bool bidirect = true)
        {
            Children.Add(node);
            if (bidirect)
            {
                node.Children.Add(this);
            }

            return this;
        }

        // функция для отображения узла
        public void Handler()
        {
            Console.WriteLine($"{this.Name}");
        }
    }

    public class DepthFirstSearch
    {
        private HashSet<Node> visited;
        private LinkedList<Node> path;
        private Node goal;
        private bool limitWasReached;

        // Поиск в глубину
        public LinkedList<Node> DFS(Node start, Node goal)
        {
            visited = new HashSet<Node>();
            path = new LinkedList<Node>();
            this.goal = goal;
            DFS(start); //вызываем метод ниже
            if (path.Count > 0)
            {
                path.AddFirst(start);
            }

            return path;
        }

        //действия над отдельной нодой
        private bool DFS(Node node)
        {
            //пишем в консоль что посетили ноду
            node.Handler();
            // если это не та что мы ищем, заканчиваем поиск
            if (node == goal)
            {
                return true;
            }

            visited.Add(node);
            //поиск дочерних элементов (которые мы еще не посещали)
            foreach (var child in node.Children.Where(x => !visited.Contains(x)))
            {
                if (DFS(child)) // рекурсия 
                {
                    path.AddFirst(child);
                    return true;
                }
            }

            return false;
        }

        //Поиск с ограничением глубины, отличается только указанием максимальной глубины поиска
        public LinkedList<Node> DLS(Node start, Node goal, int limit)
        {
            visited = new HashSet<Node>();
            path = new LinkedList<Node>();
            limitWasReached = true;
            this.goal = goal;
            DLS(start, limit);
            if (path.Count > 0)
            {
                path.AddFirst(start);
            }

            return path;
        }

        //действия над отдельной нодой
        private bool DLS(Node node, int limit)
        {
            //пишем в консоль что посетили ноду
            node.Handler();
            // если это не та что мы ищем, заканчиваем поиск
            if (node == goal)
            {
                return true;
            }

            if (limit == 0)
            {
                limitWasReached = false;
                return false;
            }

            visited.Add(node);
            //поиск дочерних элементов (которые мы еще не посещали)
            foreach (var child in node.Children.Where(x => !visited.Contains(x)))
            {
                if (DLS(child, limit - 1)) // рекурсия 
                {
                    path.AddFirst(child);
                    return true;
                }
            }

            return false;
        }
    }
}