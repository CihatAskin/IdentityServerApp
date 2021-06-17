using IdentityServerApp.AuthServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerApp.AuthServer.Repository
{
    public class CustomUserRepository : ICustomUserRepository
    {
        private readonly CustomDbContext _context;

        public CustomUserRepository(CustomDbContext context)
        {
            _context = context;
        }
        public async Task<CustomUser> FindByEmail(string email)
        {
            return await _context.CustomUser.FirstOrDefaultAsync(x => x.Email.Equals(email));
        }

        public async Task<CustomUser> FindById(int id)
        {
            return await _context.CustomUser.FindAsync(id);
        }

        public async Task<bool> Validate(string email, string pass)
        {
            return await _context.CustomUser.AnyAsync(x=>x.Email.Equals(email) && x.Password.Equals(pass));
        }
    }
}
