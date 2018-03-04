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
        [DisplayName("Nickname: ")]
        [Required(ErrorMessage = "Nickname is required")]
        public string Nickname { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Password: ")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}