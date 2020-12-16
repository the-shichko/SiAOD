using System;
using System.Collections.Generic;
using System.Linq;

namespace example5
{
    public class HashTable
    {
        public Dictionary<int, List<int>> hashTable = new Dictionary<int, List<int>>();

        // - Вставка значения
        public void InsertValue(int value)
        {
            var hash = value.GetHashCode(); // получаем хэш значения

            var keys = hashTable.Keys.Where(x => x == hash); // проверяем есть ли такой хэш

            // реализация цепочки, при возникновении коллизии добавляем новый элемент в лист с этим хэшом
            if (!keys.Any())
            {
                var list = new List<int>();
                list.Add(value);
                hashTable.Add(hash, list);
            }
            else
            {
                hashTable[hash].Add(value); // добавляем элемент если такой ключ уже есть
            }
        }

        // - Поиск
        public List<int> SearchValues(int value)
        {
            var hash = value.GetHashCode(); // получаем хэш значения
            return hashTable[hash]; // возвращаем список элементов с этим хэшом
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var hashTable = new HashTable();
            Console.WriteLine("Enter 14 array values:");
            var listOfElements = new List<int>();
            for (var i = 0; i < 14; i++)
            {
                var item = Convert.ToInt32(Console.ReadLine());
                listOfElements.Add(item);
            }

            // заполнение
            listOfElements.ForEach(x => hashTable.InsertValue(x));
            // поиск 
            Console.WriteLine("Enter an item to search:");
            var elementForSearch = Convert.ToInt32(Console.ReadLine());
            var searchResult = hashTable.SearchValues(elementForSearch);

            Console.WriteLine("Array: ");
            listOfElements.ForEach(x => Console.Write(Convert.ToString(x) + " "));
            Console.WriteLine();
            Console.WriteLine("Hash table:");
            foreach (var m in hashTable.hashTable)
            {
                Console.WriteLine("Key: " + m.Key + ", values: ");
                m.Value.ForEach(x => Console.Write(Convert.ToString(x) + " "));
                Console.WriteLine();
            }

            Console.WriteLine("Search item " + elementForSearch + " with key: " + elementForSearch.GetHashCode());
            searchResult.ForEach(x => Console.Write(Convert.ToString(x) + " "));

            Console.ReadKey();
        }
    }
}