using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace example3
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new DoublyLinkedList<int>();
            var list2 = new DoublyLinkedList<int>();

            Console.WriteLine("Print n:");
            var res = int.TryParse(Console.ReadLine(), out var n);

            if (!res)
            {
                Console.WriteLine("Error n");
                return;
            }
            
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
            Console.WriteLine($"First sort: {list.ToMain()}");
            list2.Sort();
            Console.WriteLine($"Second sort: {list2.ToMain()}");

            var listCommon = new DoublyLinkedList<int>();
            listCommon.AddRange(list.ToArray());
            listCommon.AddRange(list2.ToArray());
            listCommon.SortDesc();
            Console.WriteLine($"Result sort desc: {listCommon.ToMain()}");

            Console.ReadLine();
        }
    }

    public class DoublyNode<T> where T : IComparable
    {
        public DoublyNode(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public DoublyNode<T> Previous { get; set; }
        public DoublyNode<T> Next { get; set; }
    }
    public class DoublyLinkedList<T> : IEnumerable<T> where T : IComparable
    {
        private DoublyNode<T> _head;
        private DoublyNode<T> _tail;

        public void Add(T data)
        {
            var node = new DoublyNode<T>(data);

            if (_head == null)
                _head = node;
            else
            {
                _tail.Next = node;
                node.Previous = _tail;
            }

            _tail = node;
            Count++;
        }

        public void AddRange(params T[] list)
        {
            foreach (var data in list)
            {
                var node = new DoublyNode<T>(data);

                if (_head == null)
                    _head = node;
                else
                {
                    _tail.Next = node;
                    node.Previous = _tail;
                }

                _tail = node;
                Count++;
            }
        }

        public void AddFirst(T data)
        {
            var node = new DoublyNode<T>(data);
            var temp = _head;
            node.Next = temp;
            _head = node;
            if (Count == 0)
                _tail = _head;
            else
                temp.Previous = node;
            Count++;
        }

        public bool Remove(T data)
        {
            var current = _head;

            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                if (current.Next != null)
                {
                    current.Next.Previous = current.Previous;
                }
                else
                {
                    _tail = current.Previous;
                }

                if (current.Previous != null)
                {
                    current.Previous.Next = current.Next;
                }
                else
                {
                    _head = current.Next;
                }

                Count--;
                return true;
            }

            return false;
        }

        public int Count { get; private set; }

        public bool IsEmpty => Count == 0;

        public void Clear()
        {
            _head = null;
            _tail = null;
            Count = 0;
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

        private IEnumerable<T> BackEnumerator()
        {
            var current = _tail;
            while (current != null)
            {
                yield return current.Data;
                current = current.Previous;
            }
        }

        public string ToMain()
        {
            var result = this.Aggregate(string.Empty, (current, item) => current + $"{item} -> ");
            return result.Remove(result.Length - 4);
        }

        public string ToBack()
        {
            var result = BackEnumerator().Aggregate(string.Empty, (current, item) => current + $"{item} -> ");
            return result.Remove(result.Length - 4);
        }

        public void Sort()
        {
            DoublyNode<T> firstItem;
            DoublyNode<T> secondItem;
            if (_head?.Next == null) return;

            var swap = true;

            while (swap)
            {
                swap = false;
                firstItem = _head;
                while (firstItem.Next != null)
                {
                    secondItem = firstItem.Next;
                    if (firstItem.Data.CompareTo(secondItem.Data) > 0)
                    {
                        firstItem.Next = secondItem.Next;

                        if (secondItem.Next != null)
                            secondItem.Next.Previous = firstItem;

                        secondItem.Next = firstItem;
                        secondItem.Previous = firstItem.Previous;

                        if (secondItem.Previous != null)
                        {
                            secondItem.Previous.Next = secondItem;
                        }
                        else
                        {
                            _head = secondItem;
                        }

                        firstItem.Previous = secondItem;

                        firstItem = secondItem;
                        swap = true;
                    }

                    firstItem = firstItem.Next;
                }
            }
        }

        public void SortDesc()
        {
            DoublyNode<T> firstItem;
            DoublyNode<T> secondItem;
            if (_head?.Next == null) return;

            var swap = true;

            while (swap)
            {
                swap = false;
                firstItem = _head;
                while (firstItem.Next != null)
                {
                    secondItem = firstItem.Next;
                    if (firstItem.Data.CompareTo(secondItem.Data) < 0)
                    {
                        firstItem.Next = secondItem.Next;

                        if (secondItem.Next != null)
                            secondItem.Next.Previous = firstItem;

                        secondItem.Next = firstItem;
                        secondItem.Previous = firstItem.Previous;

                        if (secondItem.Previous != null)
                        {
                            secondItem.Previous.Next = secondItem;
                        }
                        else
                        {
                            _head = secondItem;
                        }

                        firstItem.Previous = secondItem;

                        firstItem = secondItem;
                        swap = true;
                    }

                    firstItem = firstItem.Next;
                }
            }
        }
    }
}