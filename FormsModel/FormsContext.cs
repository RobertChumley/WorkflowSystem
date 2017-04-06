using FormsModel.FormDTOs;
using Microsoft.EntityFrameworkCore;

namespace FormsModel
{
    public class FormsContext : DbContext
    {
        public FormsContext(DbContextOptions<FormsContext> options)
            : base(options)
        { }

        public DbSet<Form> Forms { get; set; }
    }
}
