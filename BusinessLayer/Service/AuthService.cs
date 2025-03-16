using BusinessLayer.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTOs;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IEmailService emailService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _emailService = emailService;
        }

        //
        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDTO request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null) return false;

            // Generate Reset Token (GUID)
            user.ResetToken = Guid.NewGuid().ToString();
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);

            await _userRepository.UpdateUserAsync(user);

            // Send Reset Email
            string resetLink = $"https://yourapp.com/reset-password?token={user.ResetToken}";
            string subject = "Password Reset Request";
            string body = $"Click the link to reset your password: {resetLink}";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO request)
        {
            var user = await _userRepository.GetUserByResetTokenAsync(request.Token);
            if (user == null || user.ResetTokenExpiry < DateTime.UtcNow) return false;

            // Hash New Password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        //
        public async Task<bool> RegisterUser(UserDTO userDto)
        {
            var existingUser = await _userRepository.GetUserByEmail(userDto.Email);
            if (existingUser != null)
                return false; // User already exists

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = PasswordHasher.HashPassword(userDto.Password)
            };

            return await _userRepository.CreateUser(user);
        }

        public async Task<string?> LoginUser(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null || !PasswordHasher.VerifyPassword(password, user.PasswordHash))
                return null; // Invalid credentials

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]);
            var signingKey = new SymmetricSecurityKey(secretKey);
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpirationInMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
