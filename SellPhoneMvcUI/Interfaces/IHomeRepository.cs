namespace SellPhoneMvcUI.Interfaces
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(string sTerm = "", int category = 0);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Product> GetProductById(int id);
    }
}