using eCommercePanel.DAL.Context;
using eCommercePanel.DAL.DTOs.AddressDTOs.Requests;
using eCommercePanel.DAL.DTOs.AddressDTOs.Responses;
using eCommercePanel.DAL.Entities;
using eCommercePanel.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eCommercePanel.DAL.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Address> _address;

        public AddressRepository(AppDbContext context)
        {
            _context = context;
            _address = context.Set<Address>();
        }

        public async Task AddAsync(Address address)
        {
            await _address.AddAsync(address);        
            await _context.SaveChangesAsync();       
        }

        public void Delete(Address address)
        {
            _address.Remove(address);
        }

        public async Task<Address> GetByIdAsync(int id)
        {
            return await _address.FindAsync(id);
        }

        public async Task<Address> GetByIdWithUserAsync(int id)
        {
            return await _context.Addresses
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Address>> GetMyAddressesAsync(int id)
        {
            return await _address
                .Where(a => a.UserId == id)
                .ToListAsync();
        }
        
        public async Task<List<Address>> GetUserAddressAsync(int userId)
        {
            return await _address
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }
        public async Task<List<Address>> GetByUserIdAsync(int userId)
        {
            return await _context.Addresses
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateAddress(Address address)
        {
            throw new NotImplementedException();
        }
    }
}
