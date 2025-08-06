using eCommercePanel.DAL.Context;
using eCommercePanel.DAL.Entities;
using eCommercePanel.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommercePanel.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _users;

        public UserRepository(AppDbContext context)
        {
            _context = context;
            _users = context.Set<User>();
        }

        public async Task<List<User>> GetAllAsync() => await _users.ToListAsync();

        public async Task<User> GetByIdAsync(int id) => await _users.FindAsync(id);


        public async Task<User> GetByEmailAsync(string email) =>
            await _users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<bool> IsEmailExistsAsync(string email) =>
            await _users.AnyAsync(u => u.Email == email);

        public async Task AddAsync(User user) => await _users.AddAsync(user);

        public void Update(User user) => _users.Update(user);

        public void Delete(User user) => _users.Remove(user);

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public async Task<User> GetByEmailAsync(string email, string password) => await _users.FirstOrDefaultAsync(x=> x.Email == email && x.Password == password);

        public IQueryable<User> GetQueryable()
        {
            return _context.Set<User>().AsQueryable();
        }
    }
}
