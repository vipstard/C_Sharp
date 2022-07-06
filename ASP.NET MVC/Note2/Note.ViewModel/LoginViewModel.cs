using System;
using System.ComponentModel.DataAnnotations;

namespace Note.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string  Password { get; set; }
    }
}
