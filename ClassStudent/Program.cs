// Class Student
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClassStudent
{
    class Person
    {
        protected string Name, Surname;
        private readonly ushort max_age = 150;
        protected byte age;

        public Person(string first, string last, byte age)
        {
            if (!first.All(s => char.IsLetter(s)) || !last.All(s => char.IsLetter(s)))
            {
                throw new Exception("Неправильное имя.");
            }
            this.Name = first;
            this.Surname = last;
            if (age >= max_age && age <= 0)
            {
                throw new Exception("Неправильный возраст. Возраст должен быть больше 0 и меньше 150.");
            }
            this.age = age;
        }

        public void change_name(string name)
        {
            this.Name = name;
        }

        public void change_surname(string surname)
        {
            this.Surname = surname;
        }

        public void change_age(byte age)
        {
            this.age = age;
        }

        public void upgrade_age()
        {
            this.age += 1;
        }

        public string get_name()
        {
            return this.Name + " " + this.Surname;
        }

        public int get_age()
        {
            return this.age;
        }

        public void write_person_info()
        {
            Console.WriteLine("Имя: " + this.Name + " " + this.Surname);
            Console.WriteLine("Возраст: " + this.age);
        }
    }

    public enum StudyYear 
    { 
        first = 1,
        second = 2,
        third = 3,
        fourth = 4
    }

    enum status : int
    {
        Studying = 1,
        Retaking = 2,
        Expelled = 3
    }

    enum speciality
    {
        CompModeling,
        PhysOptic
    }

    class Student : Person
    {
        public status Status;
        protected readonly int id;
        public speciality Speciality;
        private ushort min_age = 17;
        List<(StudyYear year, string disc)> list_of_disciplines = new List<(StudyYear year, string diss)>();
        Dictionary<(StudyYear year, string disp), int> DispList = new Dictionary<(StudyYear year, string disp), int>();
        public StudyYear year;
        protected double average_grade;
        protected double average_year_grade;

        public Student(int id, string first, string last, byte age, speciality Speciality) :
            base(first, last, age)
        {
            if (this.age < min_age)
            {
                throw new Exception($"Неправильный возраст, возраст должен быть больше {this.min_age}");
            }
            this.Speciality = Speciality; 
            this.id = id;
            this.year = StudyYear.first;
            Status = status.Studying;
            set_disciplines();
        }

        public status get_status()
        {
            return this.Status;
        }

        public void show_id()
        {
            Console.WriteLine("ID: " + id);
        }

        public void upgrade_year()
        { 
            year += 1;
        }

        public void set_disciplines()
        {
            list_of_disciplines.Clear();
            list_of_disciplines.AddRange(new[] 
            {
                (StudyYear.first, "math"),
                (StudyYear.first, "linear algebra"),
                (StudyYear.first, "english"),
                (StudyYear.first, "programming on c++"),
                (StudyYear.first, "mechanics"),
                (StudyYear.second, "assembler"),
                (StudyYear.second, "english"),  
                (StudyYear.second, "electricity"),
                (StudyYear.second, "programming on python"),
                (StudyYear.third, "numerical nethods"),
                (StudyYear.third, "phylosophy"),
                (StudyYear.third, "thermodynamics"),
                (StudyYear.fourth, "dataBases"),
                (StudyYear.fourth, "statistical analisys"),
                (StudyYear.fourth, "diploma")
            } );
            if (Speciality == speciality.CompModeling)
            {
                list_of_disciplines.AddRange(new[]
                {
                    (StudyYear.second, "programming on c++"),
                    (StudyYear.third, "numerical nethods specialization"),
                    (StudyYear.third, "atomic physics"),
                    (StudyYear.fourth, "parallel programming"),
                    (StudyYear.fourth, "programmingCsharp")
                });
            }
            else if (Speciality == speciality.PhysOptic)
            {
                list_of_disciplines.AddRange(new[]
                {
                    (StudyYear.second, "optics"),
                    (StudyYear.third, "optics specialization"),
                    (StudyYear.third, "nuclear physics"),
                    (StudyYear.fourth, "lazer physics"),
                    (StudyYear.fourth, "lazer physics specialization")
                });
            }
        }

        public void session()
        {
            if (this.Status != status.Expelled)
            {
                put_marks();
                check_for_bad_marks();
                this.average_grade = get_average_grade();
                this.average_year_grade = get_average_year_grade();
                if (Status == status.Studying)
                {
                    write_person_info();
                }
            }
        }

        private void put_marks()
        {
            foreach (var discipline in list_of_disciplines)
            {
                if (discipline.year == this.year)
                { 
                    int mark = get_mark(discipline.year, discipline.disc);
                    DispList.Add((year: this.year, discipline.disc), value: mark);
                }
            }   
        }
        
        private int get_mark(StudyYear year, string disc)
        {
            int mark;
            Console.WriteLine($"Год: {year}\nДисциплина: {disc}\nОценка для студента {Name} {Surname}: ");
            while (!int.TryParse(Console.ReadLine(), out mark) || mark < 0 || mark > 10)
            {
                Console.WriteLine("Это не число, либо оно больше 10 или меньше 0!");
                Console.Write("Введите число заново: ");
            }
            return mark;         
        }

        private void check_for_bad_marks()
        {
            int number_of_bad_marks = 0;
            foreach (var discipline in DispList)
            {
                if (discipline.Key.year == this.year)
                {
                    if (discipline.Value < 4)
                    {
                        this.Status = status.Retaking;
                        number_of_bad_marks++;

                        int mark = get_mark(year: this.year, discipline.Key.disp);
                        if (mark < 4)
                        {
                            this.Status = status.Expelled;
                            Console.WriteLine("Студент отчислен из-за того, что не сдал пересдачу");
                        }
                        else
                        {
                            this.Status = status.Studying;
                            DispList[(discipline.Key.year, discipline.Key.disp)] = mark;
                            Console.WriteLine("Студент успешно пересдал экзамен");
                        }                    
                        if (number_of_bad_marks > 3)
                        {
                            this.Status = status.Expelled;
                            Console.WriteLine("Студент отчислен из-за количества оценок ниже 4");
                            break;
                        }
                    }
                }
            }
        }
        public void see_marks()
        {
            foreach (var dispe in DispList)
            {
                Console.WriteLine($"year: {dispe.Key.year}  disp: {dispe.Key.disp} mark: {dispe.Value}");
            }
        }
        
        public double get_average_grade() // считает средний балл за все года
        {
            double sum = 0;
            foreach (var discmark in DispList)
            {
                sum += discmark.Value;
            }
            double avg = sum / DispList.Count;
            return avg;
        }

        public double get_average_year_grade() // считает средний балл за год
        {
            double sum = 0;
            int i = 0;
            foreach (var discmark in DispList)
            {
                if (discmark.Key.year == this.year)
                {
                    sum += discmark.Value;
                    i++;
                }
            }
            double avg = sum / i;
            return avg;
        }
        public new void write_person_info()
        {
            Console.WriteLine("--------------");
            Console.WriteLine("ID: " + id);
            Console.WriteLine("Имя: " + this.Name + " " + this.Surname);
            Console.WriteLine("Возраст: " + this.age);
            //Console.WriteLine("Специальность: " + speciality);
            Console.WriteLine("Курс: " + this.year);
            Console.WriteLine("Средний балл за весь курс обучения: " + this.average_grade);
            Console.WriteLine("Средний балл за этот курс обучения: " + this.average_year_grade);
            Console.WriteLine("Статус: " + this.Status);
            Console.WriteLine("--------------");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Student> students = new List<Student>()
            {
                new(1920080, "Терри", "Пратчетт", 19, speciality.CompModeling),
                new(1920081, "Вирджиния", "Вулф", 23, speciality.PhysOptic),
                //new(1920082, "Джаред", "Лето", 18),
                //new(1920083, "Тим", "Минчин", 20),
                //new(1920084, "Марк", "Аврелий", 17),
            };
            
            int YearsToStudy = Enum.GetNames(typeof(StudyYear)).Length;
            StudyYear year = StudyYear.first;

            while ((int)year <= 4)
            {
                foreach (Student student in students)
                {
                    if (student.Status != status.Expelled) {
                        student.session();
                        //student.see_marks();
                        student.upgrade_year();
                        student.upgrade_age();
                    }
                }
                year++;
                Console.WriteLine("------------Конец учебного года-----------------");
            }
            Console.WriteLine("------------Конец обучения в университете-----------------");
        }
    }
}