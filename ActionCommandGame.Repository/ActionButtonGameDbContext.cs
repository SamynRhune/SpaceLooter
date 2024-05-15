using ActionCommandGame.Model;
using ActionCommandGame.Repository.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ActionCommandGame.Repository
{
    public class ActionButtonGameDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ActionButtonGameDbContext(DbContextOptions<ActionButtonGameDbContext> options): base(options)
        {
            
        }

        public DbSet<PositiveGameEvent> PositiveGameEvents { get; set; }
        public DbSet<NegativeGameEvent> NegativeGameEvents { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerItem> PlayerItems { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureRelationships();

            base.OnModelCreating(modelBuilder);
        }

        public static void Seed(ActionButtonGameDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Items.Any())
            {
                context.Items.AddRange(new List<Item>()
                {
                    new Item { Name = "Lightsaber", Attack = 50, Price = 50 },
                    new Item { Name = "Basic Space Rifle", Attack = 300, Price = 300 },
                    new Item { Name = "Basic Rocket", Attack = 500, Price = 500 },
                    new Item { Name = "Starcore Rifle", Attack = 5000, Price = 15000 },
                    new Item { Name = "Starcore Rocket", Attack = 50, Price = 1000000 }
                });

                context.SaveChanges();

                
            }
        }

        /*public void Initialize()
        {
            GeneratePositiveGameEvents();
            GenerateNegativeGameEvents();
            GenerateAttackItems();
            GenerateDefenseItems();
            GenerateFuelItems();
            GenerateDecorativeItems();

            //God Mode Item
            Items.Add(new Item
            {
                Name = "GOD MODE",
                Description = "This is almost how a GOD must feel.",
                Attack = 1000000,
                Defense = 1000000,
                Fuel = 1000000,
                ActionCooldownSeconds = 1,
                Price = 10000000
            });

            Players.Add(new Player { Name = "John Doe", Money = 100 });
            Players.Add(new Player { Name = "John Francks", Money = 100000, Experience = 2000 });
            Players.Add(new Player { Name = "Luc Doleman", Money = 500, Experience = 5 });
            Players.Add(new Player { Name = "Emilio Fratilleci", Money = 12345, Experience = 200 });

            SaveChanges();
        }

        private void GeneratePositiveGameEvents() 
        {
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Nothing but boring space rocks", Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "The biggest Oil Asteroid you ever saw.", Description = "A sunflare hits your spacecraft and you lose sight of the asteroid", Probability = 500 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Nothing but a big void", Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "You find an abandoned spaceship", Description = "It has already been looted", Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "You find a nice comet with a waterfall on it", Description = "The water flows on your spaceship. Your spaceship now shines", Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Space junk", Money = 1, Experience = 1, Probability = 2000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Washing machine blueprint", Money = 1, Experience = 1, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Harry Potter movie", Money = 1, Experience = 1, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Map from a destroyed planet", Money = 1, Experience = 1, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Broken Lightsaber", Money = 5, Experience = 3, Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Old Rocket", Money = 10, Experience = 5, Probability = 800 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Old Spacegun", Money = 10, Experience = 5, Probability = 800 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Shiny Comet", Money = 10, Experience = 5, Probability = 800 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Oily comet", Money = 12, Experience = 6, Probability = 700 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Oil Filled Rock", Money = 20, Experience = 8, Probability = 650 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Floating RGB Blender", Money = 30, Experience = 10, Probability = 500 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Little piece of Starcore", Money = 50, Experience = 13, Probability = 400 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Working Lightsaber", Money = 60, Experience = 15, Probability = 400 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "SpaceBlaster 1000", Money = 100, Experience = 40, Probability = 350 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Old Spacewreck", Money = 140, Experience = 50, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Deserted Spacegarden", Money = 160, Experience = 80, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Basic Oil Pump", Money = 160, Experience = 80, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Comet Filled With Oil", Money = 180, Experience = 80, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Broken Starcore", Money = 300, Experience = 100, Probability = 110 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Floating Bed", Money = 300, Experience = 100, Probability = 80 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "SpaceBlaster 300", Money = 400, Experience = 150, Probability = 200 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Space Rocket", Money = 500, Experience = 150, Probability = 150 });


            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Asteroid Filled To The Brim With Oil", Money = 10000, Experience = 200, Probability = 100 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Brand new Spaceship", Money = 1000, Experience = 200, Probability = 100 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Asteroid Crawler Eggs", Money = 60000, Experience = 1500, Probability = 5 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "The Heart of an Asteroid Crawler", Money = 3000, Experience = 400, Probability = 30 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "The head of an Asteroid Crawler", Money = 2000, Experience = 350, Probability = 30 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "FULL STARCORE", Money = 20000, Experience = 1000, Probability = 10 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Advanced Bio Tech Lab", Money = 30000, Experience = 1500, Probability = 10 });
        }

        public void GenerateNegativeGameEvents()
        {
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Rockfall",
                Description = "As you are mining, the cave walls rumble and rocks tumble down on you",
                DefenseWithGearDescription = "Your mining gear allows you and your tools to escape unscathed",
                DefenseWithoutGearDescription = "You try to cover your face but the rocks are too heavy. That hurt!",
                DefenseLoss = 2,
                Probability = 100
            });
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Cave Rat",
                Description = "As you are mining, you feel something scurry between your feet!",
                DefenseWithGearDescription = "It tries to bite you, but your mining gear keeps the rat's teeth from sinking in.",
                DefenseWithoutGearDescription = "It tries to bite you and nicks you in the ankles. It already starts to glow dangerously.",
                DefenseLoss = 3,
                Probability = 50
            });
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Sinkhole",
                Description = "As you are mining, the ground suddenly gives way and you fall down into a chasm!",
                DefenseWithGearDescription = "Your gear grants a safe landing, protecting you and your pickaxe.",
                DefenseWithoutGearDescription = "You tumble down the dark hole and take a really bad landing. That hurt!",
                DefenseLoss = 2,
                Probability = 100
            });
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Ancient Bacteria",
                Description = "As you are mining, you uncover a green slime oozing from the cracks!",
                DefenseWithGearDescription = "Your gear barely covers you from the noxious goop. You are safe.",
                DefenseWithoutGearDescription = "The slime covers your hands and arms and starts biting through your flesh. This hurts!",
                DefenseLoss = 3,
                Probability = 50
            });
        }

        private void GenerateAttackItems()
        {
            Items.Add(new Item { Name = "Lightsaber", Attack = 50, Price = 50 });
            Items.Add(new Item { Name = "Basic Space Rifle", Attack = 300, Price = 300 });
            Items.Add(new Item { Name = "Basic Rocket", Attack = 500, Price = 500 });
            Items.Add(new Item { Name = "Starcore Rifle", Attack = 5000, Price = 15000 });
            Items.Add(new Item { Name = "Starcore Rocket", Attack = 50, Price = 1000000 });
        }

        private void GenerateDefenseItems()
        {
            Items.Add(new Item { Name = "Basic Shield", Defense = 20, Price = 20 });
            Items.Add(new Item { Name = "Reflecting Shield", Defense = 150, Price = 200 });
            Items.Add(new Item { Name = "Kinetic Defensive System", Defense = 500, Price = 1000 });
            Items.Add(new Item { Name = "Asteroid Crawler's Armor", Defense = 2000, Price = 10000 });
            Items.Add(new Item { Name = "Nanobot Defensive System", Defense = 2000, Price = 10000 });
            Items.Add(new Item { Name = "Invisibility Shield", Defense = 20000, Price = 10000 });
        }

        private void GenerateFuelItems()
        {
            Items.Add(new Item { Name = "Basic Fuel Canister", ActionCooldownSeconds = 50, Fuel = 4, Price = 8 });
            Items.Add(new Item { Name = "Enriched Fuel Canister", ActionCooldownSeconds = 45, Fuel = 5, Price = 10 });
            Items.Add(new Item { Name = "Power Generator", ActionCooldownSeconds = 30, Fuel = 30, Price = 300 });
            Items.Add(new Item { Name = "Solar Panels", ActionCooldownSeconds = 30, Fuel = 100, Price = 400 });
            Items.Add(new Item { Name = "Thermal Energy Collector", ActionCooldownSeconds = 25, Fuel = 80, Price = 500 });
            Items.Add(new Item { Name = "Solarflare Fusion Reactor Core", ActionCooldownSeconds = 15, Fuel = 500, Price = 10000 });
            Items.Add(new Item { Name = "Starcore", ActionCooldownSeconds = 15, Fuel = 1000, Price = 18000 });

            Items.Add(new Item { Name = "Developer Food", ActionCooldownSeconds = 1, Fuel = 1000, Price = 1 });

        }

        private void GenerateDecorativeItems()
        {
            Items.Add(new Item { Name = "Balloon", Description = "Does nothing. Do you feel special now?", Price = 10 });
            Items.Add(new Item { Name = "Blue Medal", Description = "For those who cannot afford the Crown of Flexing.", Price = 100000 });
            Items.Add(new Item { Name = "Crown of Flexing", Description = "Yes, show everyone how much money you are willing to spend on something useless!", Price = 500000 });
        }*/

    }
}
