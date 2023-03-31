//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BidMyCar.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class UsersInfo
    {

        public int UserID { get; set; }

        [DisplayName("Fullname")]
        [Required(ErrorMessage = "Fullname is required!")]
        public string Name { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Email is required!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string Password { get; set; }

        [DisplayName("ConfirmPassword")]
        [Required(ErrorMessage = "Confirm is required!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password and password do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "")]
        public string resetCode { get; set; }
    }
}
