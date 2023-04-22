using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UsersAndOrdersService.Data.Context;
using UsersAndOrdersService.Data.Interfaces;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> UserExists(int Id)
        {
            return await _context.Users.AnyAsync(i => i.Id == Id);
        }

        public async Task<ICollection<User>> GetUsers()
        {
            return await _context.Users.OrderBy(i => i.Id).ToListAsync();
        }

        public async Task<User> GetSpecificUser(int Id)
        {
            return await _context.Users.Where(i => i.Id == Id).SingleOrDefaultAsync();
        }

        public async Task<bool> CreateUser(User user)
        {
            user.Password = Encrypt(user.Password);
            user.Role = UserRole.User;
            await _context.AddAsync(user);
            return await Save();
        }

        public async Task<bool> UpdateUser(User user)
        {
            user.Password = Encrypt(user.Password);
            user.Role = UserRole.User;
            _context.Update(user);
            return await Save();
        }

        public async Task<bool> DeleteUser(User user)
        {
            _context.Remove(user);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public bool Authorize(string email, string password)
        {
            password = Encrypt(password);

            if (_context.Users.Any(u => u.Email.Equals(email) && u.Password.Equals(password)))
            {
                return true;
            }

            return false;
        }
    }
}
