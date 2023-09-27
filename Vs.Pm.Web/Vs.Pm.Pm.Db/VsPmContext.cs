
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vs.Pm.Pm.Db.Models;

namespace Vs.Pm.Pm.Db
{
    public class VsPmContext : DbContext
    {
        public VsPmContext(DbContextOptions<VsPmContext> options) : base(options)
        {

        }

        public DbSet<LogApplicationError> DbSetLog { get; set; }
        public DbSet<Project> DbSetProject { get; set; }
        public DbSet<Status> DbSetStatus { get; set; }
        public DbSet<TaskModel> DbSetTask { get; set; }
        public DbSet<TaskType> DbSetTaskType { get; set;}

        public bool IsClaimDeleteEnabled(int template)
        {
            var sql = $"SELECT * FROM Task WHERE StatusId = {template}";

            var result = DbSetTask.FromSqlRaw(sql).Count();
            if (result == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        

        public bool IsTaskTypeDeleteEnabled(int template)
        {
            var sql = $"SELECT * FROM Task WHERE TaskTypeId = {template}";

            var result = DbSetTask.FromSqlRaw(sql).Count();
            if (result == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsProjectEnabled(int template)
        {
            var sql = $"SELECT * FROM Task WHERE ProjectId = {template}";

            var result = DbSetTask.FromSqlRaw(sql).Count();
            if (result == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
