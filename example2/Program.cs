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
            Console.WriteLine("Hello World!");

            var list = new LinkedList<int>();
            
            list.AddRange(4, 6, 3, 2, 1);
            list.Sort();
            Console.WriteLine(list.ToMain());
            
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
    }

    public class LinkedList<T> : IEnumerable<T> where T : IComparable
    {
        Node<T> head;
        Node<T> tail;
        int count;
        
        public void Add(T data)
        {
            Node<T> node = new Node<T>(data);

            if (head == null)
                head = node;
            else
                tail.Next = node;

            tail = node;
            count++;
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
            Node<T> node = new Node<T>(data);
            node.Next = head;
            head = node;
            if (count == 0)
                tail = head;
            count++;
        }
        public bool Remove(T data)
        {
            Node<T> current = head;
            Node<T> previous = null;

            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    // Если узел в середине или в конце
                    if (previous != null)
                    {
                        previous.Next = current.Next;
                        if (current.Next == null)
                            tail = previous;
                    }
                    else
                    {
                        head = head.Next;

                        if (head == null)
                            tail = null;
                    }
                    count--;
                    return true;
                }
                previous = current;
                current = current.Next;
            }
            return false;
        }
        public bool Contains(T data)
        {
            Node<T> current = head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }
            return false;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            Node<T> current = head;
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
            Node<T> firstItem;
            Node<T> secondItem;
            if (head?.Next == null) return;

            var swap = true;

            while (swap)
            {
                swap = false;
                firstItem = head;
                while (firstItem.Next != null)
                {
                    secondItem = firstItem.Next;
                    if (firstItem.Data.CompareTo(secondItem.Data) > 0)
                    {
                        firstItem.Next = secondItem.Next;
                        secondItem.Next = firstItem;

                        if (head == secondItem)
                            head = firstItem;

                        firstItem = secondItem;
                        swap = true;
                    }

                    firstItem = firstItem.Next;
                }
            }
        }

        // public void SortDesc()
        // {
        //     Node<T> firstItem;
        //     Node<T> secondItem;
        //     if (head?.Next == null) return;
        //
        //     var swap = true;
        //
        //     while (swap)
        //     {
        //         swap = false;
        //         firstItem = head;
        //         while (firstItem.Next != null)
        //         {
        //             secondItem = firstItem.Next;
        //             if (firstItem.Data.CompareTo(secondItem.Data) < 0)
        //             {
        //                 firstItem.Next = secondItem.Next;
        //
        //                 if (secondItem.Next != null)
        //                     secondItem.Next.Previous = firstItem;
        //
        //                 secondItem.Next = firstItem;
        //                 secondItem.Previous = firstItem.Previous;
        //
        //                 if (secondItem.Previous != null)
        //                 {
        //                     secondItem.Previous.Next = secondItem;
        //                 }
        //                 else
        //                 {
        //                     head = secondItem;
        //                 }
        //
        //                 firstItem.Previous = secondItem;
        //
        //                 firstItem = secondItem;
        //                 swap = true;
        //             }
        //
        //             firstItem = firstItem.Next;
        //         }
        //     }
        // }
    }
}
