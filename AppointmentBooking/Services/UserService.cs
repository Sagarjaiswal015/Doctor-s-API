using AppointmentBooking.DTOs;
using AppointmentBooking.Models;
using AppointmentBooking.Repositories;

namespace AppointmentBooking.Services;

public interface IUserService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<User> RegisterAsync(string email, string password, string role);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public UserService(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !_authService.VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        var token = _authService.GenerateToken(user.Id, user.Email, user.Role);
        return new LoginResponse
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<User> RegisterAsync(string email, string password, string role)
    {
        var existingUser = await _userRepository.GetByEmailAsync(email);
        if (existingUser != null)
            throw new InvalidOperationException("Email already registered");

        var user = new User
        {
            Email = email,
            PasswordHash = _authService.HashPassword(password),
            Role = role,
            CreatedAt = DateTime.UtcNow
        };

        return await _userRepository.CreateAsync(user);
    }
}
