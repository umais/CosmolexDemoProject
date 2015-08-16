﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectClassLibrary.Model
{
    public interface IEntity
    {
        int Id { get; set; }
        string ErrorMessage { get; set; }
    }
}
