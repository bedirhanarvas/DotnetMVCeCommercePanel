using eCommercePanel.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();                     
        Task<User> GetByIdAsync(int id);                    
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByEmailAsync(string email,string password);
        Task<bool> IsEmailExistsAsync(string email);        
        Task AddAsync(User user);                           
        void Update(User user);                            
        void Delete(User user);                             
        Task SaveAsync();
    }
}
