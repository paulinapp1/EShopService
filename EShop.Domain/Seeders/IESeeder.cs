﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Seeders
{
    public interface IESeeder
    {
        public  Task Seed();
    }
}
