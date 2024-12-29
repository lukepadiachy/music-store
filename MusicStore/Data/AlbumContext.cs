using Microsoft.EntityFrameworkCore;
using MusicStore.Model;

namespace MusicStore.Data
{
    public class AlbumContext : DbContext
    {
        public AlbumContext(DbContextOptions<AlbumContext> options): base(options)
        {
        }
        public DbSet<Album> Albums { get; set; }
    }
}
