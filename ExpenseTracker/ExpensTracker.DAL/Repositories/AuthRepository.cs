﻿using ExpensTracker.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensTracker.DAL
{
    public class AuthRepository :  BaseRepository<ExpenseTrackerEntities, Users>, IAuthRepository
    {
    }
}
