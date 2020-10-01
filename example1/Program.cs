using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace example1
{
    public class Student
    {
        public string Surname { get; set; }
        public int Year { get; set; }
        public string BirthPlace { get; set; }
        public Subject[] Subjects { get; set; } = new Subject[3];

        public void Print()
        {
            Console.WriteLine($"{Surname} ({Year})\n");
            Console.WriteLine($"1) {Subjects[0].Name} ({Subjects[0].Mark})\n");
            Console.WriteLine($"2) {Subjects[1].Name} ({Subjects[1].Mark})\n");
            Console.WriteLine($"3) {Subjects[2].Name} ({Subjects[2].Mark})\n");
        }
    }

    public class Subject
    {
        public string Name { get; set; }
        public int Mark { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // var students = new List<Student>()
            // {
            //     new Student()
            //     {
            //         Surname = "Шичко",
            //         BirthPlace = "Место1",
            //         Year = 2000,
            //         Subjects = new[]
            //         {
            //             new Subject() { Mark = 8, Name = "Химия" },
            //             new Subject() { Mark = 4, Name = "Физра" },
            //             new Subject() { Mark = 3, Name = "Матан" },
            //         }
            //     },
            //     new Student()
            //     {
            //         Surname = "Зеленко",
            //         BirthPlace = "Место2",
            //         Year = 2000,
            //         Subjects = new[]
            //         {
            //             new Subject() { Mark = 5, Name = "Химия" },
            //             new Subject() { Mark = 2, Name = "Физра" },
            //             new Subject() { Mark = 8, Name = "Матан" },
            //         }
            //     },
            // };

            var students = new List<Student>();
            for (var i = 0; i < 3; i++)
            {
                var student = new Student();
                Console.WriteLine("Print: Surname");
                student.Surname = Console.ReadLine();

                for (var j = 0; j < 3; j++)
                {
                    Console.WriteLine("Print name of subject:");
                    student.Subjects[j] = new Subject
                    {
                        Name = Console.ReadLine(),
                        Mark = int.Parse(Console.ReadLine() ?? "0")
                    };
                }
                students.Add(student);
            }

            // linq
            //students.Where(x => x.Subjects.All(y => y.Mark > 2))
            //    .OrderBy(ord => ord.Surname).ToList()
            //    .ForEach(x => x.Print());

            Student temp;
            for (int write = 0; write < students.Count; write++)
            {
                for (int sort = 0; sort < students.Count - 1; sort++)
                {
                    if (string.Compare(students[sort].Surname, students[sort + 1].Surname) > 0)
                    {
                        temp = students[sort + 1];
                        students[sort + 1] = students[sort];
                        students[sort] = temp;
                    }
                }
            }
            

            foreach (var item in students)
            {
                bool hasBest = true;
                foreach (var subject in item.Subjects)
                {
                    if (subject.Mark < 3)
                    {
                        hasBest = false;
                        break;
                    }
                }

                if (hasBest)
                    item.Print();
            }
            
            Console.ReadLine();
        }
    }
}
