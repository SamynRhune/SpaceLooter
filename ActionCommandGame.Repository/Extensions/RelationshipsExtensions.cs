using ActionCommandGame.Model;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Repository.Extensions
{
    public static class RelationshipsExtensions
    {
        public static void ConfigureRelationships(this ModelBuilder builder)
        {
            builder.ConfigurePlayerItem();
            builder.ConfigurePlayer();
        }

        private static void ConfigurePlayerItem(this ModelBuilder builder)
        {
            builder.Entity<PlayerItem>()
                /*.HasOne(a => a.ItemId)
                .WithMany()
                .HasForeignKey(a => a.ItemId)*/
                .Property(a => a.PlayerId)
                .IsRequired();

            builder.Entity<PlayerItem>()
                /*.HasOne(a => a.Player)
                .WithMany()
                .HasForeignKey(a => a.PlayerId)*/
                .Property(a => a.ItemId)
                .IsRequired();
        }

        private static void ConfigurePlayer(this ModelBuilder builder)
        {

            builder.Entity<Player>()
                .Property(p => p.CurrentFuelPlayerItemId)
                /*.HasOne(a => a.CurrentFuelPlayerItem)
                .WithMany(u => u.FuelPlayers)
                .HasForeignKey(a => a.CurrentFuelPlayerItemId)*/;

            builder.Entity<Player>()
                .Property(p => p.CurrentAttackPlayerItemId);
            /*.HasOne(a => a.CurrentAttackPlayerItem)
            .WithMany(u => u.AttackPlayers)
            .HasForeignKey(a => a.CurrentAttackPlayerItemId);*/

        builder.Entity<Player>()
                .Property(p => p.CurrentDefensePlayerItemId);
            /*.HasOne(a => a.CurrentDefensePlayerItem)
            .WithMany(u => u.DefensePlayers)
            .HasForeignKey(a => a.CurrentDefensePlayerItemId)*/
            ;
        }
    }
}
