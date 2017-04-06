using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FormsModel
{
    public class TemporaryDbContextFactory : IDbContextFactory<FormsContext>
    {
        public FormsContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<FormsContext>();
            builder.UseSqlServer("Server=(local)\\SQLEXPRESS;Database=formsDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new FormsContext(builder.Options);
        }
    }
    public class TemporaryWorkflowDbContextFactory : IDbContextFactory<WorkflowContext>
    {
        public WorkflowContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<WorkflowContext>();
            builder.UseSqlServer("Server=(local)\\SQLEXPRESS;Database=formsDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new WorkflowContext(builder.Options);
        }
    }
}
