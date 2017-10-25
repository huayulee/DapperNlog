using DapperDemo.DbUtil;
using DapperDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
