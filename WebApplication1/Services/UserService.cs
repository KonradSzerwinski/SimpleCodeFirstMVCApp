using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class UserService
    {
        #region Constructor and global parameters

        private UserRepository userRepo; //AK: Po co tak? W konstruktorze to robisz

        /// <summary>
        /// Konstruktor
        /// </summary>

        public UserService()
        {
            userRepo = new UserRepository();
        }

        #endregion

        #region AddUpdate

        /// <summary>
        /// Metoda weryfikuje czy pdoawany uzytkownik jest juz w bazie czy jest to nowy uzytkownik.
        /// Jezeli uzytkownik istnieje zapisuje go i przypisuje mu nowe wartosci, jesli metoda nie znajdzie uzytkownika w bazie, tworzy nowy obiekt uyztkownika i przypisuje mu wartosci.
        /// </summary>
        /// <param name="entity">Obiekt posiadajacy dane podane przez uzytkownika.</param>
        /// <returns>Jesli z jakiegos powodu uzytkownik znaleziony w bazie badz nowo utowrzony jest pusty, zwraca false.
        /// W przeciwnym wypadku zwroci wartosc zwracana przez metode AddUpdate zamieszczona w repozytorium. <see cref="UserRepository"/></returns>

        public bool AddUpdate(UserModel entity) //AK: Nie podales jasno co bedzie zwrocone, przeniosles odpowiedzialnosc na AddUpdate z repo
        {
            UserModel user;

            if (entity.ID > 0)
                user = userRepo.GetByID(entity.ID);

            else
            {
                user = new UserModel
                {
                    CreateDate = DateTime.Now,
                    Role = "User"
                };
            }

            if (user == null)
                return false;

            user.UserName = entity.UserName;

            if (entity.Password != String.Empty && entity.Password != null)
                user.Password = Helper.ComputeHash(entity.Password, "SHA512", null); // Haslo jest szyfrowane metoda zadeklarowana w klasie Helper. 

            user.Email = entity.Email;
            user.ModifyDate = DateTime.Now;
            user.ProfilePicture = entity.ProfilePicture;

            return userRepo.AddUpdate(user);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Metoda pozwala usunac uzytkownika z bazy danych. 
        /// </summary>
        /// <param name="entity">Obiekt posiadajacy dane podane przez uzytkownika.</param>
        /// <returns>Jesli z jakiegos powodu podany uzytkownik jest pusty badz nie ma go w bazie zwracana jest wartosc false.
        /// Jesli uzytkownik zostal znaleziony, zostaje zwrocona warosc zwracana przez metode Delete zamieszczona w repozytorium</returns>

        public bool Delete(UserModel entity) //AK: Nie podales jasno co bedzie zwrocone, przeniosles odpowiedzialnosc na Delete z repo
        {
            if (entity != null)
            {
                UserModel user;

                if (entity.ID > 0)
                    user = userRepo.GetByID(entity.ID);

                else
                    return false;

                return userRepo.Delete(user);

            }

            else
                return false;

        }

        private bool Delete(string userName = null, string password = null, int id = -1)
        {
            UserModel user;

            if (id == -1)
                user = userRepo.GetByUserName(userName);

            if (id < 0)
                user = userRepo.GetByID(id);

            else
                return false;

            return userRepo.Delete(user);
        }

        #endregion

        #region Get User/Users

        /// <summary>
        /// Metoda wyszukuje w bazie uzytkownika poprzez podanie jego nazwy uzytkownika
        /// </summary>
        /// <param name="userName">Nazwa uzytkownika.</param>
        /// <returns>Jesli nazwa uzytkownika nie jest pusta zwracany jest uzytkownik, natomiast jesli nie zostanie znaleziony otrzymamy wartosc null.
        /// Jesli nazwa uzytkownika jest pusta od razu zostanie zwrocona wartosc null</returns>

        public UserModel GetByUserName(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
                return userRepo.GetByUserName(userName);

            else
                return null;
        }

        /// <summary>
        /// Zwraca model uzytkownika na podstawie podanego ID uzytkownika.
        /// </summary>
        /// <param name="id">Parametr okreslajacy uzytkownika w bazie.</param>
        /// <returns>Jesli ID > 0 zwraca wartosc metody GetByID z repozytorium, w przeciwnym wypadku zwraca null.</returns>

        public UserModel GetByID(int id)
        {
            return id > 0 ? userRepo.GetByID(id) : null;
        }

        /// <summary>
        /// Zwraca liste uzytkownikow zarejestrowanych w bazie danych
        /// </summary>
        /// <returns>Zwraca liste z danymi uzytkownikow</returns>

        public List<UserModel> GetUsers()
        {
            return userRepo.GetAll();
        }

        #endregion

        #region Validation
        /// <summary>
        /// Zwraca wartosc true lub false odpowidajace czy metoda znalazla uzytkownika poprzez podanie jego nazwy w parametrze metody.
        /// </summary>
        /// <param name="userName">Nazwa uzytkownika</param>
        /// <returns>Jesli uzytkownik istnieje, zwraca true. Jesli nie istnieje, zwraca false.</returns>

        public bool UserNameIsExist(string userName)
        {
            UserModel entity = userRepo.GetByUserName(userName);

            if (entity == null)
                return false;

            if (userName.ToUpper() == entity.UserName.ToUpper())
                return true;

            return false;
        }

        /// <summary>
        /// Zwraca wartosc true lub false odpowidajace czy metoda znalazla uzytkownika poprzez podanie jego adresu email w parametrze metody.
        /// </summary>
        /// <param name="email">Adres email uzytkownika</param>
        /// <returns>Jesli uzytkownik istnieje, zwraca true. Jesli nie istnieje, zwraca false.</returns>

        public bool UserEmailIsExist(string email)
        {
            UserModel entity = userRepo.GetByUserEmail(email);

            if (entity == null)
                return false;

            if (email.ToUpper() == entity.Email.ToUpper())
                return true;

            else
                return false;

        }

        /// <summary>
        /// Metoda porownuje czy haslo podane przez uzytkownika zgadza sie z tym zapisanym w bazie danych.
        /// Poniewaz w bazie danych znajduje sie zakodowane haslo wywolywana jest funkcja sprawdzajaca czy podane haslo po zakodowaniu jest takie samo jak zakodowane haslo w bazie.
        /// </summary>
        /// <param name="username">Nazwa uzytkownika podana w formularzu</param>
        /// <param name="password">Haslo uzytkownika podane w formularzu</param>
        /// <returns>Zwraca true gdy haslo podane w formularzu odpowiada haslu zapisanemu w bazie danych</returns>

        public bool PasswordIsMatched(string username, string password)
        {
            UserModel entity = userRepo.GetByUserName(username);

            if (entity == null)
                return false;

            bool passwordIsMatched = Helper.VerifyHash(password, "SHA512", entity.Password);

            return passwordIsMatched;

        }

        #endregion
    }
}