using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BidMyCar.Models
{
    public class resetPassword
    {

        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string NewPassword { get; set; }

        [DisplayName("ConfirmPassword")]
        [Required(ErrorMessage = "Confirm is required!")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Confirm password and password do not match")]
        public string ConfirmPassword { get; set; }


        [Required]
        public string resetCode { get; set; }
    }
}