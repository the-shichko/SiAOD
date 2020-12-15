using System;
using System.Collections.Generic;

namespace example7
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Enter:");
                var input = Console.ReadLine();
                Console.WriteLine(WriteInPolish(input));
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        private static string WriteInPolish(string str)
        {
            var output = "";
            var stack = new Stack<PolishNode>();
            foreach (var ch in str)
            {
                output += AddToStack(stack, ch);
            }

            while (stack.Count != 0)
            {
                output += stack.Pop().Item;
            }

            return output;
        }

        private static string AddToStack(Stack<PolishNode> stack, char input)
        {
            var output = "";
            var newNode = new PolishNode(input);
            switch (newNode.Priority)
            {
                case 0:
                    stack.Push(newNode);
                    break;
                case 1:
                    while (stack.Peek().Priority != 0)
                    {
                        output += stack.Pop().Item;
                    }

                    stack.Pop();
                    break;
                default:
                    while (stack.Count != 0 && stack.Peek().Priority >= newNode.Priority)
                    {
                        output += stack.Pop().Item;
                    }

                    stack.Push(newNode);
                    break;
            }

            return output;
        }
    }

    public class PolishNode
    {
        public readonly char Item;
        public readonly byte Priority;

        public PolishNode(char input)
        {
            Item = input;
            switch (Item)
            {
                case '(':
                    Priority = 0;
                    break;
                case ')':
                    Priority = 1;
                    break;
                case '+':
                case '-':
                    Priority = 2;
                    break;
                case '*':
                case '/':
                    Priority = 3;
                    break;
                default:
                    Priority = 4;
                    break;
            }
        }
    }
}