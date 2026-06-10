namespace Domain.Entities.Shared
{
    public abstract class Audits
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
       
    }
}
