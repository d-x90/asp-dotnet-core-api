using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AspDotnetCoreApi.Exceptions;
using AspDotnetCoreApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using static System.Text.Encoding;

namespace AspDotnetCoreApi.Services {
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApiDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationService(ApiDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> IsUserPresent(string username, string email)
        {
            return await _context.Users.AnyAsync(u => u.Username.Equals(username) || u.Email.Equals(email));
        }

        public async Task<string> Login(string usernameOrEmail, string password)
        {
            usernameOrEmail = usernameOrEmail.ToLower().Trim();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(usernameOrEmail) || u.Email.Equals(usernameOrEmail));

            if(user == null) {
                throw new UserNotFoundException();
            }

            var isPasswordCorrect = VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);

            if(!isPasswordCorrect) {
                throw new IncorrectPasswordException();
            }

            return CreateJwtToken(user);
        }

        public async Task<int> Register(User user, string password)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) {
            using(var hmac = new HMACSHA512(passwordSalt)) {
                var computedHash = hmac.ComputeHash(UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordHash[i]) {
                        return false;
                    }
                }

                return true;
            }
        }

        private string CreateJwtToken(User user) {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(UTF8.GetBytes(_configuration["JWT_Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    } 
}