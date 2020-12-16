using System;

namespace example9
{
    public class PrTreeNode<T>
    {
        private T _data;
        public bool Ltag;
        public PrTreeNode<T> Left;
        public bool Rtag;
        public PrTreeNode<T> Rigth;

        public T GetData()
        {
            return _data;
        }

        public void SetData(T data)
        {
            _data = data;
        }

        public PrTreeNode(T data, PrTreeNode<T> priv, PrTreeNode<T> next)
        {
            _data = data;
            Ltag = false;
            Left = priv;
            Rtag = false;
            Rigth = next;
        }

        public PrTreeNode()
        {
            _data = default;
            Ltag = false;
            Left = this;
            Rtag = false;
            Rigth = this;
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

    public class PrTree<T>
    {
        private PrTreeNode<T> _head;

        public PrTree()
        {
            _head = new PrTreeNode<T>();
        }

        public void Add(T data)
        {
            if (_head.Ltag)
                AddNode(_head.Left, _head, _head, data);
            else
            {
                _head.Left = new PrTreeNode<T>(data, _head, _head);
                _head.Ltag = true;
            } 
        }

        private void AddNode(PrTreeNode<T> parent, PrTreeNode<T> priv, PrTreeNode<T> next, T data)
        {
            switch (parent.Compare(Convert.ToInt32(data)))
            {
                //rigth
                case -1:
                    if (parent.Rtag)
                        AddNode(parent.Rigth, parent, next, data);
                    else
                    {
                        parent.Rigth = new PrTreeNode<T>(data, parent, next);
                        parent.Rtag = true;
                    }
                    break;
                //left
                case 1:
                    if (parent.Ltag)
                        AddNode(parent.Left, priv, parent, data);
                    else
                    {
                        parent.Left = new PrTreeNode<T>(data, priv, parent);
                        parent.Ltag = true;
                    }
                    break;
            }
        }

        public void Remove(T data)
        {
            try
            {
                if (_head.Ltag)
                {
                    RemoveNode(_head.Left, _head, _head, _head, data);
                }
                else
                {
                    Console.WriteLine("Empty tree");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void RemoveNode(PrTreeNode<T> current, PrTreeNode<T> parent, PrTreeNode<T> priv, PrTreeNode<T> next, T data)
        {
            switch (current.Compare(Convert.ToInt32(data)))
            {
                case -1:
                    if (current.Rtag)
                        RemoveNode(current.Rigth, current, current, next, data);
                    else
                    {
                        throw new Exception($"No node with {data}");
                    }
                    break;
                case 1:
                    if (current.Ltag)
                        RemoveNode(current.Left, current, priv, current, data);
                    else
                    {
                        throw new Exception($"No node with {data}");
                    }
                    break;
                case 0:
                    //нет детей
                    if (!current.Ltag && !current.Rtag)
                    {
                        switch (parent.Compare(Convert.ToInt32(current.GetData())))
                        {
                            case -1:
                                parent.Rtag = false;
                                parent.Rigth = current.Rigth;
                                break;
                            case 1:
                                parent.Ltag = false;
                                parent.Left = current.Left;
                                break;
                        }
                        if (!priv.Rtag && priv != parent)
                            priv.Rigth = parent;
                        if (!next.Ltag && next != parent)
                            next.Left = parent;
                    }
                    //только праый ребенок
                    else if (current.Rtag && !current.Ltag)
                    {

                        switch (parent.Compare(Convert.ToInt32(current.GetData())))
                        {
                            case -1:
                                if (parent == _head)
                                {
                                    parent.Left = current.Rigth;
                                    if (!current.Rtag)
                                        parent.Ltag = false;
                                }
                                else
                                {
                                    parent.Rigth = current.Rigth;
                                    if (!current.Rtag)
                                        parent.Rtag = false;
                                }
                                break;
                            case 1:
                                parent.Left = current.Rigth;
                                if (!current.Rtag)
                                    parent.Ltag = false;
                                break;
                        }
                        if (!priv.Rtag && priv != parent)
                            priv.Rigth = current.Rigth;
                        current.Rigth.Left = parent;
                    }
                    //только левый ребенок
                    else if (current.Ltag && !current.Rtag)
                    {
                        switch (parent.Compare(Convert.ToInt32(current.GetData())))
                        {
                            case -1:
                                if (parent == _head)
                                {
                                    parent.Left = current.Left;
                                    if (!current.Ltag)
                                        parent.Ltag = false;
                                }
                                else
                                {
                                    parent.Rigth = current.Left;
                                    if (!current.Ltag)
                                        parent.Rtag = false;
                                }
                                break;
                            case 1:
                                parent.Left = current.Left;
                                if (!current.Ltag)
                                    parent.Ltag = false;
                                break;
                        }

                        current.Left.Rigth = parent;
                        if (!next.Ltag && next != parent)
                            next.Left = current.Left;
                    }
                    //есть два ребенка
                    else
                    {
                        var replacement = parent.Left;
                        while (replacement.Rtag)
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
                return "path: " + SearchNode(_head.Left, data);
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        private string SearchNode(PrTreeNode<T> current, T data)
        {                    
            switch (current.Compare(Convert.ToInt32(data)))
            {
                case -1:
                    if (current.Rtag)
                        return current + " " + SearchNode(current.Rigth, data);
                    else
                        throw new Exception($"No node with {data}"); ;
                case 1:
                    if (current.Ltag)
                        return current + " " + SearchNode(current.Left, data);
                    else
                        throw new Exception($"No node with {data}"); ;
                case 0:
                    return current + " ";
                default:
                    throw new Exception("Compare error");

            }
        }

        public string Show()
        {
            if (!_head.Ltag)
                return "Empty tree";

            int i = 0 , j = 0;
            var output = "";
            var current = _head.Left;
            while (current != _head)
            {
                if (current.Ltag)
                    current = current.Left;
                else
                {
                    while (!current.Rtag && current != _head)
                    {
                        output += current + " ";
                        current = current.Rigth;
                    }
                    output += current + " ";
                    current = current.Rigth;
                }
            }
            return output;
        }

    }
}