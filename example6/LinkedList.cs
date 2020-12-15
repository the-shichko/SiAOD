using System;
using System.Collections;
using System.Collections.Generic;

namespace example6
{
    public class LinkedList<T> : IEnumerable<T>
    {
        Node<T> head;
        Node<T> tail;
        public int count { private set; get; } = 0;

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

        public T Search(T data)
        {
            Node<T> current = head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return current.Data;
                current = current.Next;
            }
            throw new ArgumentException($"no item");
        }

        public T Remove()
        {
            if (count == 0 || head == null)
                throw new Exception("empty list");
            Node<T> current = head;
            head = current.Next;
            count--;
            return current.Data;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
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
