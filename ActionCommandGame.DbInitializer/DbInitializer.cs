using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace ActionCommandGame.DbInitializer
{
    public class DbInitializer
    {
        public static void Seed(ActionButtonGameDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Items.Any())
            {
                context.Items.AddRange(new List<Item>()
                {
                    new Item { Id = 1, Name = "Lightsaber", Description = "An elegant weapon for a more civilized age.", Price = 50, Fuel = 0, Attack = 50, Defense = 0, ActionCooldownSeconds = 0 },
                    new Item { Id = 2, Name = "Basic Space Rifle", Description = "A reliable rifle for space skirmishes.", Price = 300, Fuel = 0, Attack = 300, Defense = 0, ActionCooldownSeconds = 0 },
                    new Item { Id = 3, Name = "Basic Rocket", Description = "A simple yet effective rocket for short-range combat.", Price = 500, Fuel = 0, Attack = 500, Defense = 0, ActionCooldownSeconds = 0 },
                    new Item { Id = 4, Name = "Starcore Rifle", Description = "A high-powered rifle using advanced Starcore technology.", Price = 15000, Fuel = 0, Attack = 5000, Defense = 0, ActionCooldownSeconds = 0 },
                    new Item { Id = 5, Name = "Starcore Rocket", Description = "An extremely powerful rocket fueled by Starcore energy.", Price = 1000000, Fuel = 0, Attack = 50, Defense = 0, ActionCooldownSeconds = 0 },
                    new Item { Id = 6, Name = "Basic Shield", Description = "", Price = 20, Fuel = 0, Attack = 0, Defense = 20, ActionCooldownSeconds = 0 },
                    new Item { Id = 7, Name = "Reflecting Shield", Description = "", Price = 200, Fuel = 0, Attack = 0, Defense = 150, ActionCooldownSeconds = 0 },
                    new Item { Id = 8, Name = "Kinetic Defensive System", Description = "", Price = 1000, Fuel = 0, Attack = 0, Defense = 500, ActionCooldownSeconds = 0 },
                    new Item { Id = 9, Name = "Asteroid Crawler's Armor", Description = "", Price = 10000, Fuel = 0, Attack = 0, Defense = 2000, ActionCooldownSeconds = 0 },
                    new Item { Id = 10, Name = "Nanobot Defensive System", Description = "", Price = 10000, Fuel = 0, Attack = 0, Defense = 2000, ActionCooldownSeconds = 0 },
                    new Item { Id = 11, Name = "Invisibility Shield", Description = "", Price = 10000, Fuel = 0, Attack = 0, Defense = 20000, ActionCooldownSeconds = 0 },
                    new Item { Id = 12, Name = "Basic Fuel Canister", Description = "", Price = 8, Fuel = 4, Attack = 0, Defense = 0, ActionCooldownSeconds = 50 },
                    new Item { Id = 13, Name = "Enriched Fuel Canister", Description = "", Price = 10, Fuel = 5, Attack = 0, Defense = 0, ActionCooldownSeconds = 45 },
                    new Item { Id = 14, Name = "Power Generator", Description = "", Price = 300, Fuel = 30, Attack = 0, Defense = 0, ActionCooldownSeconds = 30 },
                    new Item { Id = 15, Name = "Solar Panels", Description = "", Price = 400, Fuel = 100, Attack = 0, Defense = 0, ActionCooldownSeconds = 30 },
                    new Item { Id = 16, Name = "Thermal Energy Collector", Description = "", Price = 500, Fuel = 80, Attack = 0, Defense = 0, ActionCooldownSeconds = 25 },
                    new Item { Id = 17, Name = "Solarflare Fusion Reactor Core", Description = "", Price = 10000, Fuel = 500, Attack = 0, Defense = 0, ActionCooldownSeconds = 15 },
                    new Item { Id = 18, Name = "Starcore", Description = "", Price = 18000, Fuel = 1000, Attack = 0, Defense = 0, ActionCooldownSeconds = 15 },
                    new Item { Id = 19, Name = "Developer Food", Description = "", Price = 1, Fuel = 1000, Attack = 0, Defense = 0, ActionCooldownSeconds = 1 }


                });

                
            }

            if (!context.Players.Any())
            {
                context.Players.AddRange(new List<Player>()
                {
                    new Player { Id = 1, Name = "John Doe", Money = 100, Experience = 0, CurrentFuelPlayerItemId = -1, CurrentAttackPlayerItemId = -1, CurrentDefensePlayerItemId = -1 },
                    new Player { Id = 2, Name = "John Francks", Money = 100000, Experience = 2000, CurrentFuelPlayerItemId = -1, CurrentAttackPlayerItemId = -1, CurrentDefensePlayerItemId = -1 },
                    new Player { Id = 3, Name = "Luc Doleman", Money = 500, Experience = 5, CurrentFuelPlayerItemId = -1, CurrentAttackPlayerItemId = -1, CurrentDefensePlayerItemId = -1 },
                    new Player { Id = 4, Name = "Emilio Fratilleci", Money = 12345, Experience = 200, CurrentFuelPlayerItemId = -1, CurrentAttackPlayerItemId = -1, CurrentDefensePlayerItemId = -1 }
                });
            }

            if (!context.PositiveGameEvents.Any())
            {
                context.PositiveGameEvents.AddRange(new List<PositiveGameEvent>
                {
                    new PositiveGameEvent { Id = 1, Name = "Nothing but boring space rocks", Description = "", Money = 0, Experience = 0, Probability = 1000 },
                    new PositiveGameEvent { Id = 2, Name = "The biggest Oil Asteroid you ever saw.", Description = "A sunflare hits your spacecraft and you lose sight of the asteroid", Money = 0, Experience = 0, Probability = 500 },
                    new PositiveGameEvent { Id = 3, Name = "Nothing but a big void", Description = "", Money = 0, Experience = 0, Probability = 1000 },
                    new PositiveGameEvent { Id = 4, Name = "You find an abandoned spaceship", Description = "It has already been looted", Money = 0, Experience = 0, Probability = 1000 },
                    new PositiveGameEvent { Id = 5, Name = "You find a nice comet with a waterfall on it", Description = "The water flows on your spaceship. Your spaceship now shines", Money = 0, Experience = 0, Probability = 1000 },
                    new PositiveGameEvent { Id = 6, Name = "Space junk", Description = "", Money = 1, Experience = 1, Probability = 2000 },
                    new PositiveGameEvent { Id = 7, Name = "Washing machine blueprint", Description = "", Money = 1, Experience = 1, Probability = 300 },
                    new PositiveGameEvent { Id = 8, Name = "Harry Potter movie", Description = "", Money = 1, Experience = 1, Probability = 300 },
                    new PositiveGameEvent { Id = 9, Name = "Map from a destroyed planet", Description = "", Money = 1, Experience = 1, Probability = 300 },
                    new PositiveGameEvent { Id = 10, Name = "Broken Lightsaber", Description = "", Money = 5, Experience = 3, Probability = 1000 },
                    new PositiveGameEvent { Id = 11, Name = "Old Rocket", Description = "", Money = 10, Experience = 5, Probability = 800 },
                    new PositiveGameEvent { Id = 12, Name = "Old Spacegun", Description = "", Money = 10, Experience = 5, Probability = 800 },
                    new PositiveGameEvent { Id = 13, Name = "Shiny Comet", Description = "", Money = 10, Experience = 5, Probability = 800 },
                    new PositiveGameEvent { Id = 14, Name = "Oily comet", Description = "", Money = 12, Experience = 6, Probability = 700 },
                    new PositiveGameEvent { Id = 15, Name = "Oil Filled Rock", Description = "", Money = 20, Experience = 8, Probability = 650 },
                    new PositiveGameEvent { Id = 16, Name = "Floating RGB Blender", Description = "", Money = 30, Experience = 10, Probability = 500 },
                    new PositiveGameEvent { Id = 17, Name = "Little piece of Starcore", Description = "", Money = 50, Experience = 13, Probability = 400 },
                    new PositiveGameEvent { Id = 18, Name = "Working Lightsaber", Description = "", Money = 60, Experience = 15, Probability = 400 },
                    new PositiveGameEvent { Id = 19, Name = "SpaceBlaster 1000", Description = "", Money = 100, Experience = 40, Probability = 350 },
                    new PositiveGameEvent { Id = 20, Name = "Old Spacewreck", Description = "", Money = 140, Experience = 50, Probability = 300 },
                    new PositiveGameEvent { Id = 21, Name = "Deserted Spacegarden", Description = "", Money = 160, Experience = 80, Probability = 300 },
                    new PositiveGameEvent { Id = 22, Name = "Basic Oil Pump", Description = "", Money = 160, Experience = 80, Probability = 300 },
                    new PositiveGameEvent { Id = 23, Name = "Comet Filled With Oil", Description = "", Money = 180, Experience = 80, Probability = 300 },
                    new PositiveGameEvent { Id = 24, Name = "Broken Starcore", Description = "", Money = 300, Experience = 100, Probability = 110 },
                    new PositiveGameEvent { Id = 25, Name = "Floating Bed", Description = "", Money = 300, Experience = 100, Probability = 80 },
                    new PositiveGameEvent { Id = 26, Name = "SpaceBlaster 300", Description = "", Money = 400, Experience = 150, Probability = 200 },
                    new PositiveGameEvent { Id = 27, Name = "Space Rocket", Description = "", Money = 500, Experience = 150, Probability = 150 },
                    new PositiveGameEvent { Id = 28, Name = "Asteroid Filled To The Brim With Oil", Description = "", Money = 10000, Experience = 200, Probability = 100 },
                    new PositiveGameEvent { Id = 29, Name = "Brand new Spaceship", Description = "", Money = 1000, Experience = 200, Probability = 100 },
                    new PositiveGameEvent { Id = 30, Name = "Asteroid Crawler Eggs", Description = "", Money = 60000, Experience = 1500, Probability = 5 },
                    new PositiveGameEvent { Id = 31, Name = "The Heart of an Asteroid Crawler", Description = "", Money = 3000, Experience = 400, Probability = 30 },
                    new PositiveGameEvent { Id = 32, Name = "The head of an Asteroid Crawler", Description = "", Money = 2000, Experience = 350, Probability = 30 },
                    new PositiveGameEvent { Id = 33, Name = "FULL STARCORE", Description = "", Money = 20000, Experience = 1000, Probability = 10 },
                    new PositiveGameEvent { Id = 34, Name = "Advanced Bio Tech Lab", Description = "", Money = 30000, Experience = 1500, Probability = 10 }
                });
            }

            if (!context.NegativeGameEvents.Any())
            {
                context.NegativeGameEvents.AddRange(new List<NegativeGameEvent>
        {
            new NegativeGameEvent
            {
                Id = 1,
                Name = "Missile Attack",
                Description = "While approaching the space body, a missile attack comes at you",
                DefenseWithGearDescription = "Your shield offers full protection against all incoming missiles.",
                DefenseWithoutGearDescription = "The missiles breach your spacecraft's defenses, hitting their mark. That hurt!",
                DefenseLoss = 2,
                Probability = 100
            },
            new NegativeGameEvent
            {
                Id = 2,
                Name = "Meteorite Strike",
                Description = "While searching for loot, you hear something on your radar!",
                DefenseWithGearDescription = "Your shield protects you from all the meteorites",
                DefenseWithoutGearDescription = "You try to shoot away the meteorites manually but you miss some. That hurt!",
                DefenseLoss = 3,
                Probability = 50
            },
            new NegativeGameEvent
            {
                Id = 3,
                Name = "Space Pirates",
                Description = "As you are roaming around, you are now the target of a group space pirates!",
                DefenseWithGearDescription = "Your defense system took down the Space Pirates, your lucky",
                DefenseWithoutGearDescription = "The Space Pirates loot your spacecraft and you can't do anything",
                DefenseLoss = 4,
                Probability = 100
            },
            new NegativeGameEvent
            {
                Id = 4,
                Name = "Asteroid Crawler's",
                Description = "As you are searching for loot, an Asteroid Crawler grabs your ship!",
                DefenseWithGearDescription = "Your shield barely protects you from this space creature. You are safe.",
                DefenseWithoutGearDescription = "The Asteroid Crawler grabs your spaceship and starts biting through your roof. You can barely alive escape from this beast!",
                DefenseLoss = 5,
                Probability = 30
            }
        });

                //ALLE ITEMS NOG TOEVOEGEN EN TEKST AANPASSEN
            }

            context.SaveChanges();
        }
    }
}
