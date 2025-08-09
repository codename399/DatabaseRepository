using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseRepository.Model
{
    public class Audit
    {
        [BsonElement("_id")]
        public string? Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdationDate { get; set; }
        public bool IsDeleted { get; set; }

        public Audit()
        {
            Id = Guid.NewGuid().ToString();
            CreationDate = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}
