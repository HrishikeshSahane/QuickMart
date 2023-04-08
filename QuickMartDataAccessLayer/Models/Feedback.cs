using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuickMartDataAccessLayer.Models;

public partial class Feedback
{
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public long FeedbackId { get; set; }

    public string EmailId { get; set; }
    public string Comments { get; set; }

    public int Rating { get; set; }

    public virtual Users Email { get; set; }
}
