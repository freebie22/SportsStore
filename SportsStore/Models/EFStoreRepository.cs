namespace SportsStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private readonly StoreDbContext ctx;
        public EFStoreRepository(StoreDbContext ctx)
        {
            this.ctx = ctx;
        }
        public IQueryable<Product> Products => ctx.Products;

        public void SaveProduct(Product product)
        {
            ctx.SaveChanges();
        }

        public void CreateProduct(Product product)
        {
            ctx.Products.Add(product);
            ctx.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            ctx.Products.Remove(product);
            ctx.SaveChanges();
        }


    }
}
