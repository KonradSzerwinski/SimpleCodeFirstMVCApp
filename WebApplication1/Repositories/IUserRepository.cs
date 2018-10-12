using System.Collections.Generic;
namespace WebApplication1.Repositories
{
    public interface IUserRepository<T> where T : class
    {
        bool AddUpdate(T entity);
        bool Delete(T entity);
        List<T> GetAll();
        T GetByID(int ID);
        T GetByUserName(string userName);
        T GetByUserEmail(string email);
    }
}