using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class DataContext:IdentityDbContext<IdentityUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<Todo> Todos { get; set; }
    
}