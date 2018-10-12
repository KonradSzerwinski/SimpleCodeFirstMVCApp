using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Database;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class RoleRepository
    {
        MvcDbContext db = new MvcDbContext();

        public bool AddRole(RoleModel entity)
        {
            if(entity != null)
            {
                db.Roles.Add(entity);
                db.SaveChanges();

                return true;
            }
            return false;
        }

        public RoleModel GetByID(int id)
        {
            return db.Roles.Find(id);
        }

        public RoleModel GetByName(string name)
        {
            return db.Roles.FirstOrDefault(d => d.Rolee.ToUpper() == name.ToUpper());
        }

    }
}