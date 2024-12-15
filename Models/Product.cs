namespace MyGoldenFood.Models
{
    public class Product : BaseEntity
    {

        public string Name { get; set; }

        public string? Description { get; set; }


        // Fotoğraf için dosya yolu
        public string? ImagePath { get; set; }
    }
}
