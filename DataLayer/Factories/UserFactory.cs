using DataLayer.Context;
using DataLayer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Factories
{
    //Получение полного списка пользователей из контекста

    public interface IUserList
    {
        IEnumerable<Account> AllUsers();
    }

    class UserList : IUserList
    {
        AppDbContext db = new AppDbContext();
        public IEnumerable<Account> AllUsers()
        {

            return db.Accounts.Include("Tickets");
        }
    }

    //Вход в систему

    public interface ILogin
    {
        string LoginUser(string name, IAuthenticationManager manager, string password);
    }

    class Login : ILogin
    {
        AppDbContext db = new AppDbContext();

        public string LoginUser(string name, IAuthenticationManager manager, string password)
        {

            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(db));
            AppUser user = userMgr.Find(name, password);

            if (user == null)
            {
                return "Некорректное имя или пароль";
            }
            else
            {
                ClaimsIdentity identity = userMgr.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                manager.SignOut();
                manager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, identity);
                return string.Empty;
            }
        }

    }

    //Редактирование пользователя

    public interface IUserEditor
    {
        string EditUser(AppUser user, string password);
    }

    class UserEdit : IUserEditor
    {
        AppDbContext db ;

        public UserEdit(AppDbContext context)
        {

            db = context;

        }


        public string EditUser(AppUser user, string password = null)
        {
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(db));
            var us = userMgr.FindById(user.Id);
            var acc = db.Accounts.FirstOrDefault(a => a.Id == user.Id);
            if (us != null && acc !=null)
            {
                us.UserName = user.UserName;
                us.Email = user.Email;
                us.PhoneNumber = user.PhoneNumber;

                if (!String.IsNullOrEmpty(password))
                {
                    us.PasswordHash = userMgr.PasswordHasher.HashPassword(password);
                }

                acc.UserName = user.UserName;
                acc.Email = user.Email;
                acc.PhoneNumber = user.PhoneNumber;

                db.Entry(acc).State = EntityState.Modified;
                db.Entry(us).State = EntityState.Modified;
                return null;
            }
            return "Пользователь не найден";
        }
    }

    //Удаление пользователя

    public interface IDeleteUser
    {
        IEnumerable<string> DelteUser(string id);
    }

    class DeleteUser : IDeleteUser
    {
        AppDbContext db;

        public DeleteUser(AppDbContext context)
        {

            db = context;

        }

        public IEnumerable<string> DelteUser(string id)
        {
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(db));
            var user = userMgr.FindById(id);
            if (user != null)
            {

                IdentityResult result = userMgr.Delete(user);
                if (result.Succeeded)
                {
                    return null;
                }
                else
                {
                    return result.Errors;
                }
            }
            IEnumerable<string> ls = new List<string> { "Ползователь не найден" };
            return ls;
        }
    }
    
    //Регистрация пользователя

    public interface IUserCreator
    {
        IEnumerable<string> CreateUser(AppUser user, string password, IAuthenticationManager manager);
    }

    class UserCreator : IUserCreator
    {
        AppDbContext db;

        public UserCreator(AppDbContext context) {

            db = context;

        }

        public IEnumerable<string> CreateUser(AppUser user, string password, IAuthenticationManager manager)
        {
           

            

            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(db));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<UserRoles>(db));


            IdentityResult res = userMgr.Create(user, password);
            userMgr.CheckPassword(user, password);
            if (password.Contains("12345"))
            {
                List<string> errors = res.Errors.ToList();
                errors.Add("Пароль не должен содержать последовательность чисел");
                return errors;
            }
            else

            if (res.Succeeded && roleMgr.RoleExists("user"))
            {
                IdentityResult role = userMgr.AddToRole(user.Id, "user");
                db.Accounts.Add(new Account
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                });

                ClaimsIdentity identity = userMgr.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                manager.SignOut();
                manager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, identity);

                return null;
            }
            else
            {
                return res.Errors;
            }

        }
    }

    //Создание кассира

    public interface ICashier {
        string CreateCashier(Account us);
    }

    class CashierCreator : ICashier {
        AppDbContext db;

            public CashierCreator(AppDbContext data) {
            db = data;
        }

        public string CreateCashier(Account us)
        {
           
            var user = db.Users.FirstOrDefault(u => u.Id==us.Id);

            if (user != null)
            {
                AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(db));
                AppRoleManager roleMgr = new AppRoleManager(new RoleStore<UserRoles>(db));

                var usr = userMgr.Users.FirstOrDefault(u => u.Id == us.Id);

                if (usr != null)
                {
                    userMgr.RemoveFromRole(usr.Id, "user");
                    IdentityResult role = userMgr.AddToRole(user.Id, "cashier");

                    return null;
                }
                else
                {
                    return "Пользователь с такой ролью не найден";
                }

            }
            else {
                return "Пользователь не найден";
            }
          
        }
    }

    public interface IUserFactoryManager
    {
        IUserCreator Create();
        IDeleteUser Delete();
        IUserEditor Edit();
        ILogin Enter();
        IUserList GetUsers();
        ICashier CreateCashier();
    }

    public class UserFactory : IUserFactoryManager
    {
        private AppDbContext db;

        public UserFactory() { }

        public UserFactory(AppDbContext context)
        {
            db = context;
        }

        public IUserCreator Create()
        {
            return new UserCreator(db);
        }

        public ICashier CreateCashier()
        {
            return new CashierCreator(db);
        }

        public IDeleteUser Delete()
        {
            return new DeleteUser(db);
        }

        public IUserEditor Edit()
        {
            return new UserEdit(db);
        }

        public ILogin Enter()
        {
            return new Login();
        }

        public IUserList GetUsers()
        {
            return new UserList();
        }
    }
}
