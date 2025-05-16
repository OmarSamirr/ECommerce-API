using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Authentication
{
    public class RegisterRequest
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
    }
}
