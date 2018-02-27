using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Server.Models.Admin
{
    public class LoginModel
    {
        [DisplayName("Username: ")]
        [Required(ErrorMessage = "Username is required ;3")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Password: ")]
        [Required(ErrorMessage = "Password is required ;3")]
        public string Password { get; set; }
    }
}