using System.ComponentModel.DataAnnotations;

namespace MusicStore.Web.Models.Domain
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
