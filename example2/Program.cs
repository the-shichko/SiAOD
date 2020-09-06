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
            
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);


        }
    }

    public class Node<T>
    {
        public Node(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
        public Node<T> Next { get; set; }
    }

    public class LinkedList<T> : IEnumerable<T>
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
    }
}
