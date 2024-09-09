namespace back_end.Models // Ajuste conforme o namespace do seu projeto
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty; // Inicializa com um valor padrão
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty; // Inicializa com um valor padrão
        public string Category { get; set; } = string.Empty; // Inicializa com um valor padrão
        public string Image { get; set; } = string.Empty; // Inicializa com um valor padrão
        public string? Barcode { get; set; } // Permite valor nulo
    }
}
