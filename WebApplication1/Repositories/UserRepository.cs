using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Database;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class UserRepository : IUserRepository<UserModel>
    {

        
        private MvcDbContext db = new MvcDbContext();

        /// <summary>
        /// Metoda dodajaca uzytkownika do listy i zapisujaca zmiany w bazie danych.
        /// W przypadku gdy dodawany jest nowy uzytkownik zostaje oddany on do bazy danych.
        /// W przypadku gdy uzytkownik istnieje zostaja zapisane nowe dane danego uzytkownika.
        /// </summary>
        /// <param name="entity">Obiekt przechowujacy dane uzytkownika</param>
        /// <returns>Jesli obiekt przekazany przez uzytkownika nie jest pusty, po operacjach metoda zwroci wartosc true,
        /// w przeciwnym wypadku zwroci false.</returns>

        public bool AddUpdate(UserModel entity)
        {
            if (entity != null)
            {
                if (entity.ID == 0)
                    db.Users.Add(entity);

                db.SaveChanges();
                return true;
            }

            else
                return false;
        }

        /// <summary>
        /// Metoda usuwa uzytkownika z bazy danych i zapisuje zmiany w bazie.
        /// </summary>
        /// <param name="entity">Obiekt przechowujacy dane uzytkownika</param>
        /// <returns>Jeslli metoda usunie uzytkownika, zwraca true.
        /// <returns>Jesli obiekt przekazany przez uzytkownika nie jest pusty, po operacjach metoda zwroci wartosc true,
        /// w przeciwnym wypadku zwroci false.</returns>

        public bool Delete(UserModel entity)
        {
            if (entity != null)
            {
                db.Users.Remove(entity);

                db.SaveChanges();

                return true;
            }

            else
                return false;
        }

        /// <summary>
        /// Metoda zwraca wszystkich uzytkownikow z bazy danych w postaci listy.
        /// </summary>
        /// <returns>Zwraca liste uzytkownikow.</returns>

        public List<UserModel> GetAll()
        {
            return db.Users.ToList();
        }

        /// <summary>
        /// Wyszukuje w bazie danych i zwraca uzytkownika po podaniu jego ID.
        /// </summary>
        /// <param name="ID">Parametr okreslajacy uzytkownika w bazie.</param>
        /// <returns>Zwraca uzytkownika w postaci obiektu klasy UserModel.</returns>

        public UserModel GetByID(int ID)
        {
            return db.Users.Find(ID);
        }

        /// <summary>
        /// Wyszukuje w bazie danych i zwraca uzytkownika po podaniu jego UserName (nazwy uzytkownika).
        /// </summary>
        /// <param name="userName">Nazwa uzytkownika.</param>
        /// <returns>Zwraca uzytkownika w postaci obiektu klasy UserModel.</returns>

        public UserModel GetByUserName(string userName)
        {
            return db.Users.FirstOrDefault(d => d.UserName == userName);
        }

        /// <summary>
        /// Wyszukuje w bazie danych i zwraca uzytkownika po podaniu jego Email.
        /// </summary>
        /// <param name="email">Adres Email uzytkownika.</param>
        /// <returns>Zwraca uzytkownika w postaci obiektu klasy UserModel.</returns>

        public UserModel GetByUserEmail(string email)
        {
            return db.Users.FirstOrDefault(d => d.Email == email);
        }


    }
}