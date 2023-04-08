using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickMartCoreMVCWeb.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string EmailId { get; set; }

        public string Comments { get; set; }

        public int Rating { get; set; }
    }
}
