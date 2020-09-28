using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace example2
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new LinkedList<int>();
            var list2 = new LinkedList<int>();

            Console.WriteLine("Print n:");
            if (int.TryParse(Console.ReadLine(), out var n))
            {
                for (var i = 0; i < n; i++)
                {
                    Console.WriteLine($"First list. {i + 1} elem:");
                    list.Add(int.Parse(Console.ReadLine() ?? "0"));
                }

                for (var i = 0; i < n; i++)
                {
                    Console.WriteLine($"Second list. {i + 1} elem:");
                    list2.Add(int.Parse(Console.ReadLine() ?? "0"));
                }

                list.Sort();
                Console.WriteLine($"Sorted first list: {list.ToMain()}");

                list2.Sort();
                Console.WriteLine($"Sorted second list: {list2.ToMain()}");

                var listCommon = new LinkedList<int>();
                listCommon.AddRange(list.ToArray());
                listCommon.AddRange(list2.ToArray());
                listCommon.SortDesc();

                Console.WriteLine($"Result sort desc: {listCommon.ToMain()}");
            }
            else
            {
                Console.WriteLine("Error n");
            }

            Console.ReadLine();
        }
    }

    public class Node<T> where T : IComparable
    {
        public Node(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public Node<T> Next { get; set; }
        public static Func<T, T, bool> Greater()
        {
            return (lhs, rhs) => lhs.CompareTo(rhs) > 0;
        }

        public static Func<T, T, bool> Less()
        {
            return (lhs, rhs) => lhs.CompareTo(rhs) < 0;
        }
    }

    public class LinkedList<T> : IEnumerable<T> where T : IComparable
    {
        private Node<T> _head;
        private Node<T> _tail;
        private int _count;

        public void Add(T data)
        {
            var node = new Node<T>(data);

            if (_head == null)
                _head = node;
            else
                _tail.Next = node;

            _tail = node;
            _count++;
        }
        public void AddRange(params T[] list)
        {
            foreach (var data in list)
            {
                Add(data);
            }
        }
        public void AppendFirst(T data)
        {
            var node = new Node<T>(data) {Next = _head};
            _head = node;
            if (_count == 0)
                _tail = _head;
            _count++;
        }
        public bool Remove(T data)
        {
            var current = _head;
            Node<T> previous = null;

            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    if (previous != null)
                    {
                        previous.Next = current.Next;
                        if (current.Next == null)
                            _tail = previous;
                    }
                    else
                    {
                        _head = _head.Next;

                        if (_head == null)
                            _tail = null;
                    }

                    _count--;
                    return true;
                }

                previous = current;
                current = current.Next;
            }

            return false;
        }
        public bool Contains(T data)
        {
            var current = _head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }

            return false;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) this).GetEnumerator();
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            var current = _head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
        public string ToMain()
        {
            var result = this.Aggregate(string.Empty, (current, item) => current + $"{item} -> ");
            return result.Remove(result.Length - 4);
        }

        public void Sort()
        {
            _head = MergeSort(_head, Node<T>.Less());
        }

        public void SortDesc()
        {
            _head = MergeSort(_head, Node<T>.Greater());
        }

        private static Node<T> SortedMerge(Node<T> a, Node<T> b, Func<T, T, bool> compare)
        {
            Node<T> result = null;

            if (a == null)
                return b;
            if (b == null)
                return a;

            if (compare(a.Data, b.Data))
            {
                result = a;
                result.Next = SortedMerge(a.Next, b, compare);
            }
            else
            {
                result = b;
                result.Next = SortedMerge(a, b.Next, compare);
            }

            return result;
        }
        private static Node<T> MergeSort(Node<T> h, Func<T, T, bool> compare)
        {
            if (h == null || h.Next == null)
            {
                return h;
            }

            var middle = GetMiddle(h);
            var nextofmiddle = middle.Next;

            middle.Next = null;

            var left = MergeSort(h, compare);

            var right = MergeSort(nextofmiddle, compare);

            var sortedlist = SortedMerge(left, right, compare);
            return sortedlist;
        }
        private static Node<T> GetMiddle(Node<T> h)
        {
            if (h == null)
                return h;
            var fastptr = h.Next;
            var slowptr = h;

            while (fastptr != null)
            {
                fastptr = fastptr.Next;
                if (fastptr != null)
                {
                    slowptr = slowptr.Next;
                    fastptr = fastptr.Next;
                }
            }

            return slowptr;
        }
    }
}