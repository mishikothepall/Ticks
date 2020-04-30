using AutoMapper;
using BusLayer.DTO;
using DataLayer.Models;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.Service
{
    public interface IUserService {
        List<UserDTO> AllUsers();
        IEnumerable<string> CreateUser(UserDTO user, string password, IAuthenticationManager manager);
        string EditUser(UserDTO user, string password = null);
        List<string> DeleteUser(string id);
        string LoginUser(string name, IAuthenticationManager manager, string password);
        UserDTO RetrieveUser(string id);
    }

    public class UserService : IUserService
    {
        private UnitOfWork Unit { get; set; }

        private IMapper mapper = new Mapper(AutomapperConfig.Config);

        public UserService(UnitOfWork unit) {
            Unit = unit;
        }

        public List<UserDTO> AllUsers()
        {
            return mapper.Map<List<Account>, List<UserDTO>>(Unit.userFactory.GetUsers().AllUsers().ToList());
        }

        public UserDTO RetrieveUser(string id) {
            return AllUsers().FirstOrDefault(u=>u.Id==id);
        }

        public IEnumerable<string> CreateUser(UserDTO user, string password, IAuthenticationManager manager)
        {
            var account = new AppUser
            {
                UserName= user.UserName,
                Email = user.Email
            };
             return Unit.userFactory.Create().CreateUser(account, password, manager);
            
        }

        public string EditUser(UserDTO user, string password = null)
        {
            var account = new AppUser
            {
                Id= user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return Unit.userFactory.Edit().EditUser(account, password);
        }

        public List<string> DeleteUser(string id)
        {
            return Unit.userFactory.Delete().DelteUser(id).ToList();
        }

        public string LoginUser(string name, IAuthenticationManager manager, string password)
        {
            return Unit.userFactory.Enter().LoginUser(name, manager, password);
        }

        public string CreateCashier(string id) {
            var us = Unit.userFactory.GetUsers().AllUsers().FirstOrDefault(u=>u.Id==id);
            return Unit.userFactory.CreateCashier().CreateCashier(us);
        }
    }
}
