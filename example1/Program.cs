﻿using System;
using System.Collections.Generic;
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
            var students = new List<Student>()
            {
                new Student()
                {
                    Surname = "Шичко",
                    BirthPlace = "Место1",
                    Year = 2000,
                    Subjects = new[]
                    {
                        new Subject() { Mark = 8, Name = "Химия" },
                        new Subject() { Mark = 2, Name = "Физра" },
                        new Subject() { Mark = 3, Name = "Матан" },
                    }
                },
                new Student()
                {
                    Surname = "Зеленко",
                    BirthPlace = "Место2",
                    Year = 2000,
                    Subjects = new[]
                    {
                        new Subject() { Mark = 5, Name = "Химия" },
                        new Subject() { Mark = 4, Name = "Физра" },
                        new Subject() { Mark = 8, Name = "Матан" },
                    }
                },
            };

            students.Where(x => x.Subjects.All(y => y.Mark > 2))
                .OrderBy(ord => ord.Surname).ToList()
                .ForEach(x => x.Print());

            Console.Read();
        }
    }
}