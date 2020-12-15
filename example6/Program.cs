using System;

namespace example6
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new CombiList<string>();
            string input = "";
            while (input != "5")
            {
                Console.WriteLine("Action: 1)add 2)remove 3)search 4)flush 5)exit");
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        try
                        {
                            Console.WriteLine("Enter priority(1-4 4-highest):");
                            string val = Console.ReadLine();
                            byte prior = Convert.ToByte(val);
                            Console.WriteLine("Enter data:");
                            string data = Console.ReadLine();
                            list.Add(data, prior);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case "2":
                        try
                        {
                            ReturnData<string> returnData =  list.Remove();
                            Console.WriteLine("priority {0} data: {1}", returnData.Priority, returnData.Data);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case "3":
                        try
                        {
                            Console.WriteLine("Enter data:");
                            string data = Console.ReadLine();
                            ReturnData<string> returnData = list.Search(data);
                            Console.WriteLine("priority {0} data: {1}", returnData.Priority, returnData.Data);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case "4":
                        int i = 1;
                        while (true)
                        {
                            try
                            {
                                ReturnData<string> returnData = list.Remove();
                                Console.WriteLine("{0})priority {1} data: {2}", i, returnData.Priority, returnData.Data);
                                i++;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                break;
                            }
                        }
                        break;
                    case "5":
                        break;
                    default:
                        input = "";
                        break;
                }
            }
        }
    }
}