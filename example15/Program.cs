using System;
using System.Collections.Generic;
using System.Linq;

namespace example15
{
    class Program
    {
        public class Node<TK, TP>
        {
            public List<Node<TK, TP>> Children { get; set; }
            public List<Entry<TK, TP>> Entries { get; set; } // - значения в листе

            private int degree;

            public Node(int degree)
            {
                this.degree = degree;
                Children = new List<Node<TK, TP>>(degree);
                Entries = new List<Entry<TK, TP>>(degree);
            }

            public bool IsLeaf => Children.Count == 0; // - это узел?

            public bool HasReachedMaxEntries => Entries.Count == (2 * degree) - 1;

            public bool HasReachedMinEntries => Entries.Count == degree - 1;
        }

        public class Entry<TK, TP> : IEquatable<Entry<TK, TP>> // - узлы листа
        {
            public TK Key { get; set; } // - ключ

            public TP Pointer { get; set; } // - указатель

            public bool Equals(Entry<TK, TP> other) // - функция эквивалентности значений ключей
            {
                return Key.Equals(other.Key) && Pointer.Equals(other.Pointer);
            }
        }

        public class BTree<TK, TP> where TK : IComparable<TK>
        {
            public BTree(int degree)
            {
                if (degree < 2)
                {
                    throw new ArgumentException("BTree degree must be at least 2", "degree");
                }

                Root = new Node<TK, TP>(degree);
                Degree = degree;
                Height = 1;
            }

            public Node<TK, TP> Root { get; private set; } // - корень

            public int Degree { get; private set; } // - степень

            public int Height { get; private set; } // - высота

            public Entry<TK, TP> Search(TK key)
            {
                return SearchInternal(Root, key); // - возвращает узел (вызов приватной функции поиска)
            }

            public void Insert(TK newKey, TP newPointer)
            {
                if (!Root.HasReachedMaxEntries) // - если есть место в корне
                {
                    InsertNonFull(Root, newKey, newPointer); //вставка
                    return;
                }

                // Если родительский узел также был заполнен – то нам опять приходится разбивать.
                // И так далее до корня (если разбивается корень – то появляется новый корень и глубина дерева увеличивается).
                // Вставить ключ в уже заполненный лист невозможно => необходима операция разбиения узла на 2
                var oldRoot = Root;
                Root = new Node<TK, TP>(Degree); // - создаем новый узел
                Root.Children.Add(oldRoot); // - перемещаем старый корень
                SplitChild(Root, 0, oldRoot); // - расчепляем узел на 2
                InsertNonFull(Root, newKey, newPointer); // - добалвяем ключ

                Height++; // - если разбивается корень – то появляется новый корень и глубина дерева увеличивается
            }

            // Удаление по ключу (внешний)
            public void Delete(TK keyToDelete)
            {
                DeleteInternal(Root, keyToDelete);

                if (Root.Entries.Count == 0 && !Root.IsLeaf)
                {
                    Root = Root.Children.Single();
                    Height--;
                }
            }

            // Метод удаления
            private void DeleteInternal(Node<TK, TP> node, TK keyToDelete)
            {
                var i = node.Entries.TakeWhile(entry => keyToDelete.CompareTo(entry.Key) > 0)
                    .Count(); // - ищем позицию

                // Нашли ключ в узле - удаляем из него
                if (i < node.Entries.Count && node.Entries[i].Key.CompareTo(keyToDelete) == 0)
                {
                    DeleteKeyFromNode(node, keyToDelete, i);
                    return;
                }

                // Иначе удаление из поддерева
                if (!node.IsLeaf)
                {
                    DeleteKeyFromSubtree(node, keyToDelete, i);
                }
            }

            private void DeleteKeyFromSubtree(Node<TK, TP> parentNode, TK keyToDelete, int subtreeIndexInNode)
            {
                var childNode = parentNode.Children[subtreeIndexInNode];

                //Если удаление происходит из листа,
                //то необходимо проверить, сколько ключей находится в нем.
                //если существует соседний лист (находящийся рядом с ним и имеющий такого же родителя), который содержит больше t-1 ключа,
                //то выберем ключ из этого соседа, который является разделителем между оставшимися ключами узла-соседа и исходного узла
                if (childNode.HasReachedMinEntries)
                {
                    var leftIndex = subtreeIndexInNode - 1;
                    var leftSibling =
                        subtreeIndexInNode > 0 ? parentNode.Children[leftIndex] : null; // - сосед слева

                    var rightIndex = subtreeIndexInNode + 1;
                    var rightSibling = subtreeIndexInNode < parentNode.Children.Count - 1 // - сосед справа
                        ? parentNode.Children[rightIndex]
                        : null;

                    if (leftSibling != null && leftSibling.Entries.Count > Degree - 1
                    ) // - если сосед слева не пуст и содержит больше t-1 ключа
                    {
                        //Перемещаем соседа в родительский узел и один узел-разделитель из родительского перемещаем в исходный узел
                        childNode.Entries.Insert(0,
                            parentNode.Entries[subtreeIndexInNode]); // - перемещение разделителя
                        parentNode.Entries[subtreeIndexInNode] =
                            leftSibling.Entries.Last(); // - вставка соседа в родительский
                        leftSibling.Entries.RemoveAt(leftSibling.Entries.Count - 1); // - удаление из соседа

                        if (!leftSibling.IsLeaf)
                        {
                            childNode.Children.Insert(0, leftSibling.Children.Last());
                            leftSibling.Children.RemoveAt(leftSibling.Children.Count - 1);
                        }
                    }
                    else if (rightSibling != null && rightSibling.Entries.Count > Degree - 1
                    ) // - если сосед справа не пуст и содержит больше t-1 ключа
                    {
                        // Перемещаем соседа в родительский узел и один узел-разделитель из родительского перемещаем в исходный узел
                        childNode.Entries.Add(parentNode.Entries[subtreeIndexInNode]); // - перемещение разделителя
                        parentNode.Entries[subtreeIndexInNode] =
                            rightSibling.Entries.First(); // - вставка соседа в родительский
                        rightSibling.Entries.RemoveAt(0); // - удаление из соседа

                        if (!rightSibling.IsLeaf) // - если это не корень, мы выполняем аналогичную процедуру с ним
                        {
                            childNode.Children.Add(rightSibling.Children.First());
                            rightSibling.Children.RemoveAt(0);
                        }
                    }
                    else
                    {
                        //Если же все соседи нашего узла имеют по t-1 ключу. То мы объединяем его с каким-либо соседом, удаляем нужный ключ.
                        //И тот ключ из узла-родителя, который был разделителем для
                        //этих двух «бывших» соседей, переместим в наш новообразовавшийся узел (очевидно, он будет в нем медианой).

                        if (leftSibling != null)
                        {
                            childNode.Entries.Insert(0, parentNode.Entries[subtreeIndexInNode]);
                            var oldEntries = childNode.Entries;
                            childNode.Entries = leftSibling.Entries;
                            childNode.Entries.AddRange(oldEntries);
                            if (!leftSibling.IsLeaf)
                            {
                                var oldChildren = childNode.Children;
                                childNode.Children = leftSibling.Children;
                                childNode.Children.AddRange(oldChildren);
                            }

                            parentNode.Children.RemoveAt(leftIndex);
                            parentNode.Entries.RemoveAt(subtreeIndexInNode);
                        }
                        else
                        {
                            childNode.Entries.Add(parentNode.Entries[subtreeIndexInNode]);
                            childNode.Entries.AddRange(rightSibling.Entries);
                            if (!rightSibling.IsLeaf)
                            {
                                childNode.Children.AddRange(rightSibling.Children);
                            }

                            parentNode.Children.RemoveAt(rightIndex);
                            parentNode.Entries.RemoveAt(subtreeIndexInNode);
                        }
                    }
                }


                // Если больше t-1, то просто удаляем и больше ничего делать не нужно. 
                DeleteInternal(childNode, keyToDelete);
            }

            private void DeleteKeyFromNode(Node<TK, TP> node, TK keyToDelete, int keyIndexInNode)
            {
                //Если корень одновременно является листом, то есть в дереве всего один узел,
                //мы просто удаляем ключ из этого узла.
                if (node.IsLeaf)
                {
                    node.Entries.RemoveAt(keyIndexInNode);
                    return;
                }

                // Предшествующий потомок
                var predecessorChild = node.Children[keyIndexInNode];
                if (predecessorChild.Entries.Count >= Degree
                ) // - если в нем количество вхождений больше или равно мин. степени
                {
                    var predecessor = DeletePredecessor(predecessorChild); // - удаляем предшественника
                    node.Entries[keyIndexInNode] = predecessor; // - перемешаем его в исходный узел
                }
                else
                {
                    // Преемник
                    var successorChild = node.Children[keyIndexInNode + 1];
                    if (successorChild.Entries.Count >= Degree
                    ) // - если в нем количество вхождений больше или равно мин. степени
                    {
                        var successor = DeleteSuccessor(predecessorChild); // - удаляем преемника
                        node.Entries[keyIndexInNode] = successor; // - перемешаем его в исходный узел
                    }
                    else
                    {
                        // Иначе добавляем в предшественника все значение исходного узла и преемника
                        predecessorChild.Entries.Add(node.Entries[keyIndexInNode]);
                        predecessorChild.Entries.AddRange(successorChild.Entries);
                        predecessorChild.Children.AddRange(successorChild.Children);

                        node.Entries.RemoveAt(keyIndexInNode);
                        node.Children.RemoveAt(keyIndexInNode + 1);

                        DeleteInternal(predecessorChild, keyToDelete); // - удаление предшественника
                    }
                }
            }

            // Удаление предшественника (вспомогательный метод)
            private Entry<TK, TP> DeletePredecessor(Node<TK, TP> node)
            {
                if (node.IsLeaf) // - если лист
                {
                    var result = node.Entries[node.Entries.Count - 1];
                    node.Entries.RemoveAt(node.Entries.Count -
                                          1); // - удаляем последнее входное значение и возвращаем его
                    return result;
                }

                // Иначе рекурсивно переходим к потомку
                return DeletePredecessor(node.Children.Last());
            }

            // Удаление преемника (вспомогательный метод)
            private Entry<TK, TP> DeleteSuccessor(Node<TK, TP> node)
            {
                if (node.IsLeaf) // - если лист
                {
                    var result = node.Entries[0];
                    node.Entries.RemoveAt(0); // - удаляем первое входное узначение и возвращаем его
                    return result;
                }

                // Иначе рекурсивно переходим к потомку
                return DeletePredecessor(node.Children.First());
            }

            // Поиск по ключу
            private Entry<TK, TP> SearchInternal(Node<TK, TP> node, TK key)
            {
                // Начиная с корня просматриваем ключи
                var i = node.Entries.TakeWhile(entry => key.CompareTo(entry.Key) > 0).Count();

                if (i < node.Entries.Count && node.Entries[i].Key.CompareTo(key) == 0)
                {
                    // Если нашли нужное знаение - возврвщаем
                    return node.Entries[i];
                }

                // Если это был лист без потомков (дошли до конца) - ничего не нашли
                // Если еще не дошли - продолжаем поиск в потомке
                return node.IsLeaf ? null : SearchInternal(node.Children[i], key);
            }

            //разделяет узел на 2
            private void SplitChild(Node<TK, TP> parentNode, int nodeToBeSplitIndex, Node<TK, TP> nodeToBeSplit)
            {
                // t - минимальная степень
                //разбиваем на 2 по t-1, а средний элемент (для которого t-1 первых ключей меньше его, а t-1 последних больше)
                //перемещается в родительский узел.
                var newNode = new Node<TK, TP>(Degree);

                parentNode.Entries.Insert(nodeToBeSplitIndex, nodeToBeSplit.Entries[Degree - 1]);
                parentNode.Children.Insert(nodeToBeSplitIndex + 1, newNode);

                newNode.Entries.AddRange(nodeToBeSplit.Entries.GetRange(Degree, Degree - 1));

                nodeToBeSplit.Entries.RemoveRange(Degree - 1, Degree);


                if (!nodeToBeSplit.IsLeaf)
                {
                    newNode.Children.AddRange(nodeToBeSplit.Children.GetRange(Degree, Degree));
                    nodeToBeSplit.Children.RemoveRange(Degree, Degree);
                }
            }

            // Метод вставки
            private void InsertNonFull(Node<TK, TP> node, TK newKey, TP newPointer)
            {
                var positionToInsert =
                    node.Entries.TakeWhile(entry => newKey.CompareTo(entry.Key) >= 0).Count(); //позиция для вставки


                if (node.IsLeaf) // - если это вершина
                {
                    node.Entries.Insert(positionToInsert,
                        new Entry<TK, TP>() {Key = newKey, Pointer = newPointer}); //вствка элемента в лист
                    return;
                }

                var child = node.Children[positionToInsert];
                if (child.HasReachedMaxEntries) // - если лист заполнен
                {
                    SplitChild(node, positionToInsert, child); // - расщепляем на 2
                    if (newKey.CompareTo(node.Entries[positionToInsert].Key) > 0)
                    {
                        positionToInsert++; // - вставляем по возрастанию (сравниваем знаения). Если наше значение больше - позиция для вставки увеличивется
                    }
                }

                InsertNonFull(node.Children[positionToInsert], newKey,
                    newPointer); // - идем от родителя к листам дальше
            }

            public void PrintTree(Node<TK, TP> root, string indent = "")
            {
                if (!root.IsLeaf) // - если есть потомки
                {
                    Console.Write($"{indent}[Корень] - ");
                    foreach (var value in root.Entries) // - пишем root, выводим все значения внутри
                    {
                        if (root.Entries.Last().Key.Equals(value.Key))
                        {
                            Console.Write(value.Key + ".\n");
                        }
                        else
                        {
                            Console.Write(value.Key.ToString() + ',');
                        }
                    }

                    foreach (var leaf in root.Children)
                    {
                        PrintTree(leaf, indent + "   ");
                    }
                }
                else
                {
                    if (Root.Equals(root)) // - если это корень
                    {
                        Console.Write($"{indent}[Корень] - ");
                        foreach (var value in root.Entries) // - выводим корень + все элементы
                        {
                            if (root.Entries.Last().Key.Equals(value.Key))
                            {
                                Console.Write(value.Key + ".\n");
                            }
                            else
                            {
                                Console.Write(value.Key.ToString() + ',');
                            }
                        }
                    }
                    else // - если это лист
                    {
                        Console.Write($"{indent}[Узел] - ");
                        foreach (var value in root.Entries) // - выводим узел и все элементы
                        {
                            if (root.Entries.Last().Key.Equals(value.Key))
                            {
                                Console.Write(value.Key + ".\n");
                            }
                            else
                            {
                                Console.Write(value.Key.ToString() + ',');
                            }
                        }
                    }
                }
            }
        }

        static void Main()
        {
            var keys = new[] {1, 2, 13, 4, 5, 16, 7, 18, 9, 10}; // - ключи

            var pointers = new[] {2, 1, 4, 13, 7, 16, 9, 5, 10, 18}; // - указатели

            var tree = new BTree<int, int>(2); // - дерево с размером вершин 2

            for (var i = 0; i < keys.Length; i++) // - заполнение бинароного дерева
            {
                tree.Insert(keys[i], pointers[i]);
            }

            PrintTree(tree, "Бинарное дерево:");
            Console.WriteLine("\nУдаление элемента 1");

            tree.Delete(1);

            PrintTree(tree, "\nПосле удаления:");
            Console.ReadLine();
        }

        static void PrintTree(BTree<int, int> tree, string title = null)
        {
            if (title != null)
            {
                Console.WriteLine(title);
            }

            tree.PrintTree(tree.Root);
        }
    }
}