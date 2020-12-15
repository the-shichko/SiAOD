using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace example6
{
    internal class ReturnData<T>
    {
        public T Data;
        public string Priority;
    }

    class CombiList<T>
    {
        private byte current = 2;
        private byte removes_left = 3;

        private LinkedList<T>[] prioryty_array = new LinkedList<T>[4];

        public CombiList()
        {
            for (int i = 0; i < 4; i++)
            {
                prioryty_array[i] = new LinkedList<T>();
            }
        }

        public void Add(T data, byte priority)
        {
            switch (priority)
            {
                case 1:
                    prioryty_array[0].Add(data);
                    break;
                case 2:
                    prioryty_array[1].Add(data);
                    break;
                case 3:
                    prioryty_array[2].Add(data);
                    break;
                case 4:
                    prioryty_array[3].Add(data);
                    break;
                default:
                    throw new ArgumentException($"No priority {priority}");
            }
        }

        private void moveCounter(byte sender)
        {
            if (sender == 0 && current == 1)
            {
                current = 2;
                removes_left = 3;
                return;
            }
            if (sender != current)
                return;
            removes_left--;
            if (removes_left == 0)
            {
                switch (current)
                {
                    case 2:
                        current = 1;
                        removes_left = 2;
                        break;
                    case 1:
                        current = 0;
                        removes_left = 1;
                        break;
                    case 0:
                        current = 2;
                        removes_left = 3;
                        break;
                }
            }
        }

        public ReturnData<T> Remove()
        {
            ReturnData<T> returnData = new ReturnData<T>();
            if (prioryty_array[3].count != 0)
            {
                returnData.Data = prioryty_array[3].Remove();
                returnData.Priority = "MAX";
                return returnData;
            }

            switch (current)
            {
                case 2:
                    if(prioryty_array[2].count != 0)
                    {
                        returnData.Data = prioryty_array[2].Remove();
                        returnData.Priority = "3";
                        moveCounter(2);
                        return returnData;
                    }
                    else
                        goto case 1;
                case 1:
                    if (prioryty_array[1].count != 0)
                    {
                        returnData.Data = prioryty_array[1].Remove();
                        returnData.Priority = "2";
                        moveCounter(1);
                        return returnData;
                    }
                    else
                    {
                        goto case 0;
                    }
                case 0:
                    if (prioryty_array[0].count != 0)
                    {
                        returnData.Data = prioryty_array[0].Remove();
                        returnData.Priority = "1";
                        moveCounter(0);
                        return returnData;
                    }
                    else
                    {
                        goto default;
                    }
                default:
                    throw new Exception("list is empty");
                    //return returnData;
            }
        }

        public ReturnData<T> Search(T data)
        {
            ReturnData<T> returnData = new ReturnData<T>();
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    returnData.Data = prioryty_array[i].Search(data);
                    returnData.Priority = (i+1).ToString();
                    break;
                }
                catch (Exception e)
                {
                    if (i == 3)
                        throw new Exception("no item");
                }
            }
            return returnData;
        }
    }
}
