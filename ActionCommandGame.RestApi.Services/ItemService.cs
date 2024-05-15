using ActionCommandGame.Model;

namespace ActionCommandGame.RestApi.Services
{
    public class ItemService : IItemService
    {
        private readonly ActionButtonGameDbContext _database;

        public ItemService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public Item Get(int id)
        {
            return _database.Items.SingleOrDefault(i => i.Id == id);
        }

        public IList<Item> Find()
        {
            return _database.Items.ToList();
        }

        public Item Create(Item item)
        {
            throw new NotImplementedException();
        }

        public Item Update(int id, Item item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
