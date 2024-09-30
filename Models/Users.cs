using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YourIdentityProject.Resources;
public class Users
{
    [Key]  // Birincil anahtar olduğunu belirtir
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "UsernameRequired")]
    public required string Username { get; set; }
    [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "EmailRequired")]
    [EmailAddress(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "InvalidEmailFormat")]
    public required string Email { get; set; }
    [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "PasswordRequired")]
    public required string Password { get; set; }

}
