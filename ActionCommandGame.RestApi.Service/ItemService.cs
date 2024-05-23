using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class ItemService: IItemService
    {
        private readonly ActionButtonGameDbContext _database;

        public ItemService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<ItemResult> Get(int id)
        {
            return await _database.Items
                .Select(l => new ItemResult
                {
                    Id = l.Id,
                    Name = l.Name,
                    Description = l.Description,
                    Price = l.Price,
                    Fuel = l.Fuel,
                    Attack = l.Attack,
                    Defense = l.Defense,
                    ActionCooldownSeconds = l.ActionCooldownSeconds
                })
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IList<ItemResult>> Find()
        {
            return await _database.Items
                .Select(l => new ItemResult
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                Price = l.Price,
                Fuel = l.Fuel,
                Attack = l.Attack,
                Defense = l.Defense,
                ActionCooldownSeconds = l.ActionCooldownSeconds
            }).ToListAsync();
        }

        public async Task<ItemResult> Create(ItemRequest request)
        {
            var item = new Item
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Fuel = request.Fuel,
                Attack = request.Attack,
                Defense = request.Defense,
                ActionCooldownSeconds = request.ActionCooldownSeconds
            };

            _database.Items.Add(item);
            await _database.SaveChangesAsync();

            return await Get(item.Id);
        }

        public async Task<ItemResult> Update(int id, ItemRequest request)
        {
            var db_item = await _database.Items
                .FirstOrDefaultAsync(a => a.Id == id);

            if (request is null)
            {
                return null;
            }

            db_item.Name = request.Name;
            db_item.Description = request.Description;
            db_item.Price = request.Price;
            db_item.Fuel = request.Fuel;
            db_item.Attack = request.Attack;
            db_item.Defense = request.Defense;
            db_item.ActionCooldownSeconds = request.ActionCooldownSeconds;

            await _database.SaveChangesAsync();

            return await Get(id);
        }

        public async Task<bool> Delete(int id)
        {
            var item = await _database.Items
               .FirstOrDefaultAsync(a => a.Id == id);

            if (item is null)
            {
                return false;
            }

            _database.Items.Remove(item);

            await _database.SaveChangesAsync();
            return true;
        }
    }
}
