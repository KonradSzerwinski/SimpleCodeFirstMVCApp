using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApplication1.ViewModels;
using WebApplication1.Services;
using WebApplication1.Models;
using System.IO;
using System.Web;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        UserService userService;
        RoleService roleService;

        public AccountController()
        {
            userService = new UserService();
            roleService = new RoleService();
        }

        #region Register

        /// <summary>
        /// Wyswietla formularz rejestracyjny uzytkownika.
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.Message = "Register";

            return View();
        }

        /// <summary>
        /// Metoda na podstawie danych wprowadzonych przez uzytkownika sprawdza czy nazwa uzytkownika i adres email nie znajduje sie juz w bazie. Gdy znajdzie takowe dane w bazie wyswietla odpowieni komunikat.
        /// Jesli zadne dane nie znajduja sie w bazie zostaje utowrzony uzytkownik z podanymi w formularzu danymi.
        /// </summary>
        /// <param name="model">Obiekt posiadajace dane podane w formularzu<see cref="RegisterViewModel"/></param>
        /// <returns>Jesli dane w bazie istnieja wyswietla bledy z nimi zwiazane, w przeciwnym razie otrzymujemy komunikat o prawidlowym zarejestrowaniu.</returns>

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            var errors = new List<string>();

            if (ModelState.IsValid)
            {
                if (userService.UserNameIsExist(model.UserName))
                    errors.Add($"Username \"{model.UserName}\" already exist");

                if (userService.UserEmailIsExist(model.Email))
                    errors.Add($"Email address \"{model.Email}\" already exist");
            }

            if (errors.Count == 0)
            {
                UserModel user = new UserModel
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    Email = model.Email
                };

                userService.AddUpdate(user);

                ViewBag.Status = true;
                ViewBag.Message = $"Profile {model.UserName} successfully registered";

                return View(new RegisterViewModel());
            }
            //AK: Po co te odstepy?  //KS: No ze tak powiem, byl tu kod i jak usunalem to zostalo miejsce i zapomnialem usunac
            else
            {
                ViewBag.Status = false;

                foreach (var item in errors)
                {
                    ModelState.AddModelError("", item);
                }
            }

            return View(model);
        }
        #endregion

        #region Login/Logout

        /// <summary>
        /// Wyswietla formularz logowania
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Metoda loguje uzytkownika na strone. Sprawdzane jest czy zalogowany uzytkownik proboje dostac sie na strone (jesli tak, zostaje przekierowany na strone informujaca o tym ze jest zalogowany).
        /// Sprawdzane jest czy uzytkownik istnieje oraz czy haslo zgdadza sie z tym w bazie danych. Pobiera ID uzytkownika i zapisuje jego ID oraz nazwe uzytkownika w sesji przegladarki.
        /// </summary>
        /// <param name="model">Obiekt posiadajace dane podane w formularzu<see cref="LoginViewModel"/></param>
        /// <returns>Po udanym zalogowaniu przekierowywuje do strony pokazujacej prawidlowe zalogowanie, w przeciwnym wypadku zostaje wyswietlony blad.</returns>

        [HttpPost]
        public ActionResult Login(LoginViewModel model) //AK: W komentarzach dokumentacyjnych mozesz uzywac see cref="LoginViewModel"/>
        {
            if (Session["USerID"] != null)
                return RedirectToAction("Index", "Home");

            var errors = new List<string>();

            if (ModelState.IsValid)
            {
                if (!userService.UserNameIsExist(model.UserName) || !userService.PasswordIsMatched(model.UserName, model.Password))
                {
                    errors.Add("Username or password not match");
                }
            }

            if (errors.Count == 0)
            {
                UserModel user = userService.GetByUserName(model.UserName);

                ViewBag.Status = true;

                Session["UserID"] = user.ID.ToString();
                Session["Username"] = user.UserName;
                Session["Role"] = user.Role;
                Session["ProfileHavePicture"] = (user.ProfilePicture == null) ? false : true;

                return RedirectToAction("LoggedIn");
            }

            else
            {
                ViewBag.Status = false;
                foreach (var item in errors)
                {
                    ModelState.AddModelError("", item);
                }
            }


            return View(model);
        }

        /// <summary>
        /// Metoda pokazujaca stone z komunikatem o udanym zalogowaniu
        /// </summary>
        /// <returns></returns>

        public ActionResult LoggedIn()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            else
                return View();
        }

        /// <summary>
        /// Metoda wylogowujaca z sesji przeglarki.
        /// </summary>
        /// <returns>Sesja zostaje wymazana w celu wylogowania uzytkownika.</returns>

        public ActionResult Logout()
        {
            TempData["Status"] = true;
            TempData["Message"] = "User successfully logout";
            if (Session["UserID"] != null)
            {
                Session.Clear();
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Edit Data

        /// <summary>
        /// Metoda pokazuje strone wymajaca podanie przez uzytkownika nazwy uzytkownika i adresu email
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            ViewBag.Message = "Forgot Password";

            return View();
        }

        /// <summary>
        /// Metoda sprawdza, czy podane dane znajduja sie w bazie. Jesli nie, wyswietlane sa bledy na stronie. Jesli istnieja ale nie pasuja do tego samego uzytkownika tez zostanie wyswietlony odpowieni blad.
        /// Jesli wszystkie dane sa poprawne, uzytkownik zostaje przekierowany na strone zmiany hasla.
        /// </summary>
        /// <param name="model">Obiekt posadajacy nazwe uzytkownika i adres email<see cref="ForgotPasswordViewModel"/></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            var errors = new List<string>();

            if (ModelState.IsValid)
            {
                if (!userService.UserEmailIsExist(model.Email))
                {
                    errors.Add($"User's email {model.Email} does not exist.");
                }

                if (!userService.UserNameIsExist(model.UserName))
                {
                    errors.Add($"User's username {model.UserName} does not exist.");
                }

                else if (userService.GetByUserName(model.UserName).Email != model.Email)
                {
                    errors.Add("Username and Email not mach");
                }
            }

            if (errors.Count == 0)
            {
                return RedirectToAction("NewPassword", "Account", new { id = model.UserName });

            }

            else
            {
                foreach (var item in errors)
                {
                    ModelState.AddModelError("", item);

                }
                ViewBag.Status = false;
            }

            return View(model);
        }

        /// <summary>
        /// Stona wyswietla okno zmiany hasla, jesli poprzez strone "Forgot Password" zostal przekazany klucz ID.
        /// </summary>
        /// <param name="id">Parametr identyfikujacy uzytkownika</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult NewPassword(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        /// <summary>
        /// Metoda sprawdza poprawnosc wprowadzonego hasla. Czy w pole haslo i potwierdz haslo zostal wprowadzony ten sam ciag znakow
        /// </summary>
        /// <param name="model">Objekt posiadajacy dane wprowadzone w formularzu przez uzytkownika.<see cref="NewPasswordViewModel"/></param>
        /// <param name="id">ID obiektu ktoremu chcemy zmienic haslo.</param>
        /// <returns>Komunikat o prawidlowym zmianie hasla jesli dane byly prawidlowo wprowadzone, badz komunikat o bledzie jesli wprowadzone dane byly niepoprawne.</returns>

        [HttpPost]
        public ActionResult NewPassword(NewPasswordViewModel model, string id)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Password and Confirm Password not match");
                ViewBag.Status = false;
                return View();
            }

            UserModel user = userService.GetByUserName(id);

            user.Password = model.Password;

            if (userService.AddUpdate(user))
            {
                ViewBag.Status = true;
                ViewBag.Message = "Password successfully updated.";
            }

            return View();
        }

        /// <summary>
        /// Wyswietla strone edycji profilu. Na podstawie Session["UserID"] jest pobierany uzytkownik i w miejsca loginu i maila sa umieszczane akrualne dane.
        /// Wyswietlana jest rowniez data i czas ostatniej modyfikacji uzytkownika.
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public ActionResult EditAccount(int id = 0)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            UserModel user;

            if (id == 0)
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                user = userService.GetByID(userID);
            }

            else
            {
                user = userService.GetByID(id);
            }

            id = user.ID;

            TempData["ID"] = user.ID;

            EditAccountViewModel model = new EditAccountViewModel
            {
                NewUserName = user.UserName,
                NewEmail = user.Email,
                NewRole = user.Role,
                ModifyDate = user.ModifyDate,
            };
            model.ID = user.ID;

            return View(model);
        }

        /// <summary>
        /// Metoda weryfikuje wprowadzone dane przez uzytkownika a nastepnie wywoluje metode aktualizowania/dodawania uzytkownika.
        /// </summary>
        /// <param name="model"><see cref="EditAccountViewModel"/></param>
        /// <returns>Gdy dane sa prawidlowe uzytkownik zsotaje przekierowany na strone o zalogowanym uzytkowniku z zedytowanymi wartosciami i komunikatem o powodzeniu zmiany.
        /// Gdy dane sa nieprawidlowe, strona jest przeladowywana oraz zostaje wyswietlone komunikaty bledow</returns>

        [HttpPost]
        public ActionResult EditAccount([Bind(Exclude = "UserPhoto")] EditAccountViewModel model)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int sessionID = Convert.ToInt32(Session["UserID"]);

            UserModel user;
            if (model.ID == 0)
                user = userService.GetByID(sessionID);

            else
                user = userService.GetByID(model.ID);


            var errors = new List<string>();

            if (ModelState.IsValid || Session["Role"].ToString() == "Admin")
            {

                if (Session["Role"].ToString() != "Admin" || sessionID == user.ID)
                {
                    if (!userService.PasswordIsMatched(user.UserName, model.OldPassword))
                        errors.Add("Old password is not correct");
                }

                if (Session["Role"].ToString() != "Admin" || sessionID == user.ID)
                {
                    if (model.NewPassword != model.ConfirmNewPassword)
                        errors.Add("Passwords must match");
                }

                else if (Session["Role"].ToString() == "Admin")
                {
                    if (model.NewPassword == null || model.NewPassword == String.Empty)
                        errors.Add("Passwords is required!");
                }

                if (userService.UserNameIsExist(model.NewUserName) && model.NewUserName.ToUpper() != user.UserName.ToUpper())
                    errors.Add("Username is already in use");

                if (userService.UserEmailIsExist(model.NewEmail) && model.NewEmail != user.Email)
                    errors.Add("Email is already in use");

            }

            if (errors.Count != 0)
            {
                ModelState.Clear();

                ViewBag.Status = false;
                foreach (var item in errors)
                {
                    ModelState.AddModelError("", item);
                }

                model.NewUserName = user.UserName;
                model.NewEmail = user.Email;
                model.ModifyDate = user.ModifyDate;

                return View(model);
            }

            else
            {

                //KS: Kod odpowiada ze przekonvertowanie obrazka na ciag binarny by mozna bylo go umiescic w tabeli sql
                if (Request.Files.Count > 0 && sessionID == user.ID && Request.Files["UserPhoto"]?.FileName != String.Empty)
                {
                    HttpPostedFileBase imgFile = Request.Files["UserPhoto"];

                    byte[] imageData = null;
                    if (imgFile != null)
                        using (var binary = new BinaryReader(imgFile.InputStream))
                        {
                            imageData = binary.ReadBytes(imgFile.ContentLength);
                        }

                    user.ProfilePicture = imageData;
                }


                user.UserName = model.NewUserName;
                user.Email = model.NewEmail;

                if (Session["Role"].ToString() == "Admin")
                    user.Role = model.NewRole;

                user.Password = !string.IsNullOrEmpty(model.NewPassword) ? model.NewPassword : model.OldPassword;
            }

            if (userService.AddUpdate(user))
            {
                if (sessionID == user.ID)
                    Session["Username"] = user.UserName;

                TempData["Status"] = true;
                TempData["Message"] = "User successfully edited";
            }

            return RedirectToAction("AccountDetails", new { id = user.ID });
        }



        /// <summary>
        /// Metoda wyswietla strone do podania hasla by usunac uzytkownika
        /// </summary>
        /// <param name="id">ID uzytkownika jesli przekazywany jest inny uzytkownik niz zalogowany</param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DeleteAccount(int id = 0)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            if (id == 0)
            {
                ViewBag.Message = $"Are you sure you want to delete account called {Session["Username"]}?";
                return View();
            }

            DeleteAccountViewModel model = new DeleteAccountViewModel
            {
                ID = id
            };

            return DeleteAccount(model);
        }

        /// <summary>
        /// Metoda weryfikuje poprawnosc wpisanego hasla, sprawdzac czy w sesji jest zalogowany uzytkownik, wylogowuje go z sesji i przekierowuje na strone startowa.
        /// </summary>
        /// <param name="model">Obiekt zaweirajacy dane wprowadzone w formularzu<see cref="DeleteAccountViewModel"/></param>
        /// <returns>Bledy gdy usuniecie badz znalezienie uzytkownika sie nie powiedzie lub powort do strony glownej z komunikatem potwierdzajacym usuniecie konta</returns>

        [HttpPost]
        public ActionResult DeleteAccount(DeleteAccountViewModel model)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            UserModel user;

            if (model.ID == 0)
            {
                user = userService.GetByUserName(Session["Username"].ToString());
            }

            else
                user = userService.GetByID(model.ID);

            if (user == null)
            {
                ViewBag.Status = false;
                ModelState.AddModelError("", "User not found");

                return View();
            }

            else if (Session["Role"].ToString() == "Admin")
            {
                if (userService.Delete(user))
                {
                    TempData["Status"] = true;
                    TempData["Message"] = "User Account successfully deleted";

                    return RedirectToAction("ListUsers");
                }
            }

            else if (userService.PasswordIsMatched(user.UserName, model.Password))
            {
                if (userService.Delete(user))
                {
                    Logout();

                    TempData["Status"] = true;
                    TempData["Message"] = "User Account successfully deleted";

                    return RedirectToAction("Index", "Home");
                }

                else
                    ModelState.AddModelError("", "Can't delete user");
            }
            return View();

        }

        /// <summary>
        /// Metoda wyswietla zdjecie uzytkownika.
        /// </summary>
        /// <returns></returns>

        public FileContentResult UserPhoto()
        {
            int userID = Convert.ToInt32(Session["UserID"]);

            UserModel user = userService.GetByID(userID);
            return new FileContentResult(user.ProfilePicture, "image/jpeg");
        }

        #endregion

        #region Details About Account / Accounts

        /// <summary>
        /// Metoda wyswietla szczegoly zalogowanego konta.
        /// </summary>
        /// <returns>Jesli nie jest zalogowany zaden uzytkownik, zostaje zaladowana strona logowania.
        /// W przeciwnym wypadku zostaja wyswietlone szczegoly dotyczace uzytkownika</returns>

        [HttpGet]
        public ActionResult AccountDetails(int id = 0)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            UserModel user;

            if (id == 0)
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                user = userService.GetByID(userID);
            }

            else
                user = userService.GetByID(id);



            ViewBag.Status = TempData["Status"];
            ViewBag.Message = TempData["Message"];

            AccountDetailsViewModel item = new AccountDetailsViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                CreateDate = user.CreateDate,
                ModifyDate = user.ModifyDate,
                ID = user.ID,
                ProfilePicture = user.ProfilePicture
            };

            return View(item);
        }

        /// <summary>
        /// Metoda wyswietla strone z danymi uzytkownika
        /// </summary>
        /// <param name="model">Obiekt z danymi jakie maja zostac wyswietlone<see cref="AccountDetailsViewModel"/></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult AccountDetails(AccountDetailsViewModel model)
        {
            return View(model);
        }

        [HttpGet]
        public ActionResult ListUsers()
        {
            ViewBag.Status = TempData["Status"];
            ViewBag.Message = TempData["Message"];

            if (Session["Role"] == null)
                return RedirectToAction("Index", "Home");


            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() != "Admin")
                {
                    TempData["Status"] = false;
                    TempData["Message"] = "You Are not Admin";
                    return RedirectToAction("Index", "Home");
                }
            }

            List<UserModel> userList = userService.GetUsers();
            var modelList = new List<UserListViewModel>();
            UserListViewModel model;

            foreach (var user in userList)
            {
                model = new UserListViewModel
                {
                    Username = user.UserName,
                    Email = user.Email,
                    CreateDate = user.CreateDate,
                    ModifyDate = user.ModifyDate,
                    Role = user.Role,
                    ID = user.ID
                };

                modelList.Add(model);
            }
            return View(modelList);
        }

        [HttpGet]
        public ActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRole(AddRoleVewModel model)
        {
            if (ModelState.IsValid)
            {
                RoleModel role = new RoleModel {Rolee = model.RoleName};

                if (roleService.AddRole(role))
                {
                    ViewBag.Status = true;
                    ViewBag.Message = "Role Successfully added to database.";

                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "Role can not been added to database.");
                    return View(model);
                }

            }

            ModelState.AddModelError("", "Model state is invalid");

            return View(model);
        }

        #endregion

    }
}
