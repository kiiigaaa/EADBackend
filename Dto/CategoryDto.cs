namespace WebApi.Dto
{
    public class CategoryDto
    {
        public string Id { get; set; } = string.Empty;

        public string? CategoryName { get; set; }

        public string? CategoryDescription { get; set; }

        public int? CategoryId { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool? IsActive { get; set; }
    }
}
