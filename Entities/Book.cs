using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace PractiseTest1.Entities
{ 
    public class Book
    {
        [Key]
    public int BookId { get; set; }
        [Required]
    public string Title { get; set; }
        [Required]
    public string Author { get; set; }
        [Required]
    public string Genre { get; set; }
        [Required]
    public string ISBN { get; set; }
        [Required]
    public DateTime PublishDate { get; set; }
        
    }
}