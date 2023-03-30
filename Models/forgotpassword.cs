using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BidMyCar.Models
{
    public class forgotPassword
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage ="Invalid email format!")]
        public string Email { get; set; }
    }
}