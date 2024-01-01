﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string? FirstName { get; set; }
        [DisplayName("Middle Name (Optional)")]
        public string? MiddleName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string? LastName { get; set; }
        [Required]
        [DisplayName("Email")]
        public string? Email { get; set; }
        [Required]
        [DisplayName("Phone Number")]
        public string? PhoneNumber { get; set; }
        [Required]
        [DisplayName("Center Name")]
        public string? CenterName { get; set; }
        [Required]
        [DisplayName("Address")]
        public string? Address { get; set; }
        [Required]
        [DisplayName("Message")]
        public string? Message { get; set; } 
    }
}
