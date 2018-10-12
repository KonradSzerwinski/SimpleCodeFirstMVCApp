using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class RoleService
    {
        RoleRepository roleRepo;

        public RoleService()
        {
            roleRepo = new RoleRepository();
        }

        public bool AddRole(RoleModel entity)
        {
            if (entity == null)
                return false;

            RoleModel role;

            if (entity.ID == 0)
                role = new RoleModel();

            else
                role = roleRepo.GetByID(entity.ID);

            role.Rolee = entity.Rolee;

            return roleRepo.AddRole(role);
        }

        public RoleModel GetByID(int id)
        {
            return roleRepo.GetByID(id);
        }

        public RoleModel GetByName(string name)
        {
            return roleRepo.GetByName(name);
        }
    }
}