﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class TokenRequest
    {
        public string Token { get; set; }
        public string ClaimType { get; set; }
    }
}
