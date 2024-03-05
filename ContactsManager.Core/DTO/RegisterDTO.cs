using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ContactsManager.Core.Enums;

namespace ContactsManager.Core.DTO
{
    public class RegisterDTO // for identity of register view, for transferring data from view to controller.
    {
        [Required(ErrorMessage ="Name can't be blank")]
        public string PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage ="Email should be in a proper email address format !")]

        //for remote validation
        [Remote(action: "IsEmailAlreadyRegistered", controller:"Account", ErrorMessage ="Email already exists !...")] // this will automatically generate javascript code .
        //for remote validation

        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number can't be blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage ="Phone Number should contain numbers only !")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password did not matched !")]
        public string ConfirmPassword { get; set; }

        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
    }
}
