using System;
using System.Collections.Generic;
using System.Linq;
using PractiseTest1.DTO;
using PractiseTest1.Entities;
using PractiseTest1.DTO;
using PractiseTest1.Entities;
using PractiseTest1.Repo;

namespace PractiseTest1.Repo
{
    public class UserImpl : IRepo<UserDTO>
    {
        private readonly BookDbContext _context;

        public UserImpl(BookDbContext context)
        {
            _context = context;
        }

        public bool Add(UserDTO item)
        {
            try
            {
                User user = new User
                {
                    Username = item.Username,
                    Email = item.Email,
                    Password = item.Password,
                    RoleName = item.RoleName 
                };

                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }

        public User ValidateUser(string email, string password) // Adjusted method name
        {
            User user = _context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
            return user;
        }

        public bool Delete(int userId)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<UserDTO> GetAll()
        {
            var users = _context.Users
                .Select(u => new UserDTO
                {
                    UserID = u.UserID,
                    Username = u.Username,
                    Email = u.Email,
                    Password = u.Password,
                    RoleName = u.RoleName 
                })
                .ToList();

            return users;
        }

        public bool Update(UserDTO item)
        {
            try
            {
                var user = _context.Users.Find(item.UserID);
                if (user != null)
                {
                    user.Username = item.Username;
                    user.Email = item.Email;
                    user.Password = item.Password;
                    user.RoleName = item.RoleName; 
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        
        public UserDTO GetById(int userId)
        {
            var user = _context.Users.Find(userId);

            if (user != null)
            {
                var userDTO = new UserDTO
                {
                    UserID = user.UserID,
                    Username = user.Username,
                    Email = user.Email,
                    Password = user.Password,
                    RoleName = user.RoleName 
                };

                return userDTO;
            }

            return null;
        }
    }
}
