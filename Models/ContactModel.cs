using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BillTest.Models
{
    public class ContactModel
    {
        [Required, Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [Required, Display(Name = "Email"), EmailAddress]
        public string? SenderEmail { get; set; }
        [Required(ErrorMessage = "Phone number is required."), Display(Name = "Phone Number")]
        [RegularExpression(@"\d{3}-\d{3}-\d{4}", ErrorMessage = "Please enter a valid phone number (e.g., 123-456-7890).")]
        public string? PhoneNumber { get; set; }
        [Required, Display(Name = "Have a particular challenge you're facing or just a general question?")]
        public string? Message { get; set; }
    }
}
