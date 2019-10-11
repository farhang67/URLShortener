using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Users.Dto
{
    public class UserViewModel
    {             
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
