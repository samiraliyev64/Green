using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Areas.AdminPanel.ViewModels
{
    public class SignInVM
    {

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        public bool isPersistent { get; set; }
    }
}
