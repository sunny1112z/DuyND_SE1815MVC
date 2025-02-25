using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DuyND_SE1815_Data.Entities;

public partial class SystemAccount
{
    public short AccountId { get; set; }
    [Required(ErrorMessage = "Account name is required.")]
    public string? AccountName { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? AccountEmail { get; set; }
    [Required(ErrorMessage = "Role is required.")]
    public int? AccountRole { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string? AccountPassword { get; set; }
    [Required(ErrorMessage = "Status is required.")]
    public int? IsActive { get; set; }
    [NotMapped]
    public string? OldPassword { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
