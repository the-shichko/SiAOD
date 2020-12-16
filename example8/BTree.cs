using System;

namespace example8
{
    public class BinTreeNode<T>
    {
        private T _data;
        public BinTreeNode<T> Left;
        public BinTreeNode<T> Rigth;

        public T GetData()
        {
            return _data;
        }

        public void SetData(T data)
        {
            _data = data;
        }

        public BinTreeNode(T data)
        {
            _data = data;
        }

        public int Compare(int inp)
        {
            var data = Convert.ToInt32(_data);
            if (inp > data)
                return -1;
            return inp < data ? 1 : 0;
        }

        public override string ToString()
        {
            return _data.ToString();
        }
    }

    public class BinTree<T>
    {
        private BinTreeNode<T> _head;

        private static void Replace(BinTreeNode<T> parent, T target, BinTreeNode<T> replacement)
        {
            switch (parent.Compare(Convert.ToInt32(target)))
            {
                case -1:
                    parent.Rigth = replacement;
                    break;
                case 1:
                    parent.Left = replacement;
                    break;
            }
        }

        public void Add(T data)
        {
            AddNode(_head, _head, data);
        }

        private void AddNode(BinTreeNode<T> current, BinTreeNode<T> parent, T data)
        {
            if (current == null)
            {
                if (parent == null)
                    _head = new BinTreeNode<T>(data);
                else
                    Replace(parent, data, new BinTreeNode<T>(data));
            }
            else
            {
                switch (current.Compare(Convert.ToInt32(data)))
                {
                    case -1:
                        AddNode(current.Rigth, current, data);
                        break;
                    case 1:
                        AddNode(current.Left, current, data);
                        break;
                }
            }
        }

        public void Remove(T data)
        {
            try
            {
                if(_head.Compare(Convert.ToInt32(data)) == 0)
                {
                    if (_head.Left == null)
                    {
                        _head = _head.Rigth;
                    }
                    else if (_head.Rigth == null)
                    {
                        _head = _head.Left;
                    }
                    else
                    {
                        var replacement = _head.Left;
                        while (replacement.Rigth != null)
                        {
                            replacement = replacement.Rigth;
                        }
                        Remove(replacement.GetData());
                        _head.SetData(replacement.GetData());
                    }
                }
                else
                {
                    RemoveNode(_head, null, data);
                }

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void RemoveNode(BinTreeNode<T> current, BinTreeNode<T> parent, T data)
        {
            if (current == null)
                throw new Exception($"No node with {data}");
            switch (current.Compare(Convert.ToInt32(data)))
            {
                case -1:
                    RemoveNode(current.Rigth, current, data);
                    break;
                case 1:
                    RemoveNode(current.Left, current, data);
                    break;
                case 0:
                    if (current.Left == null)
                    {
                        Replace(parent, current.GetData(), current.Rigth);
                    }
                    else if(current.Rigth == null)
                    {
                        Replace(parent, current.GetData(), current.Left);
                    }
                    else
                    {
                        var replacement = current.Left;
                        while (replacement.Rigth != null)
                        {
                            replacement = replacement.Rigth;
                        }
                        Remove(replacement.GetData());
                        current.SetData(replacement.GetData());
                    }
                    break;
            }           
        }

        public string Search(T data)
        {
            try
            {
                return "path: " + _head +" "+ SearchNode(_head, data);
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        private string SearchNode(BinTreeNode<T> current, T data)
        {
            if (current == null)
                throw new Exception($"No node with {data}");
            return current.Compare(Convert.ToInt32(data)) switch
            {
                -1 => current + SearchNode(current.Rigth, data),
                1 => current + SearchNode(current.Left, data),
                0 => current + " ",
                _ => throw new Exception("Compare error")
            };
        }

        public string Show(int type)
        {
            return type switch
            {
                1 => ShowNodePr(_head),
                2 => ShowNodeRev(_head),
                _ => ShowNodeSim(_head)
            };
        }

        private static string ShowNodePr(BinTreeNode<T> current)
        {
            if (current == null)
                return "";
            var output = "";
            output += current + " ";
            output += ShowNodePr(current.Left);
            output += ShowNodePr(current.Rigth);
            return output;
        }

        private static string ShowNodeSim(BinTreeNode<T> current)
        {
            if (current == null)
                return "";
            var output = "";
            output += ShowNodeSim(current.Left);
            output += current + " ";
            output += ShowNodeSim(current.Rigth);
            return output;
        }

        private static string ShowNodeRev(BinTreeNode<T> current)
        {
            if (current == null)
                return "";
            var output = "";
            output += ShowNodeRev(current.Left);
            output += ShowNodeRev(current.Rigth);
            output += current + " ";
            return output;
        }
    }
}