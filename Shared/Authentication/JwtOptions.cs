﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Authentication
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public int DurationInDays { get; set; }
    }
}
