//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hotel_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Accounts
    {
        public int account_id { get; set; }

        [Display(Name = "Username")]
        [StringLength(50, ErrorMessage = "Your login has to be longer than 3 characters", MinimumLength = 3)]
        public string account_login { get; set; }

        [Display(Name = "Password")]
        [StringLength(500, ErrorMessage = "Your password has to be longer than 3 characters", MinimumLength = 3)]
        public string account_password { get; set; }

        [Display(Name = "Type")]
        public string account_type { get; set; }
    }
}
