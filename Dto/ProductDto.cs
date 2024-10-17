namespace WebApi.Dto
{
    public class ProductDto
    {
        public string Id { get; set; } = string.Empty;

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public int? CategoryId { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool? IsActive { get; set; }
    }
}
