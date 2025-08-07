using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Common.ClientSettings
{
    public class EmailServiceClientSettings
    {
        [Required]
        public string BaseUrl { get; set; }
        [Required]
        public string SendEmail { get; set; }
        
        [Required]
        public string Body { get; set; }  
        
        [Required]
        public string Subject { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string From { get; set; }
        [Required]
        public string SmtpAddress { get; set; }
        [Required]
        public int SmtpPort { get; set; }
    }
}
