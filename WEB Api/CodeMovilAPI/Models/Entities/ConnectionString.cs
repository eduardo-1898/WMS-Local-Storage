﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class ConnectionString
    {
        public ConnectionString(string value) => Value = value;
        public string Value { get; set; }
    }
}
