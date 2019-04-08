using ExpensTracker.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensTracker.DAL
{
    public interface IAuthRepository : IBaseRepository<Users>
    {
    }
}
