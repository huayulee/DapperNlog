using DapperDemo.DbUtil;
using DapperDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DapperDemo.Repository
{
    public class StudentRepository
    {
        private DapperDBUtil db;

        public StudentRepository(short brandID)
        {
            this.db = new DapperDBUtil(brandID);
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return db.Get<Student>("SELECT * FROM Student WHERE Age > @Age", new { Age = 12 });
        }

        public bool Insert()
        {
            using(var tran = new TransactionScope())
            {
                db.Execute("INSERT INTO Student(Name,Age) VALUES('Test',33);");
                throw new ApplicationException("刻意觸發錯誤!");
                db.Execute("INSERT INTO Student(Name,Age) VALUES('Test',33);");
                tran.Complete();
            }

            return true;
        }
    }
}
