using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickMartCoreMVCApp.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string EmailId { get; set; }
        public string Comments { get; set; }
        public string FeedbackType { get; set; }
    }
}
