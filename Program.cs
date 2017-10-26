using DapperDemo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            StudentRepository sr = new StudentRepository(2);
            foreach (var item in sr.GetAllStudents())
            {
                Console.WriteLine($"{item.Name}  {item.Age}");
            }
            sr.Insert();
            LogUtil.Log.Info("ahahahah");
        }
    }
}
