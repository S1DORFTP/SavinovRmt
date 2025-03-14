using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rmt.Classes;

namespace rmt.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Получение пользователя по ID
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _userRepository.GetUserById(id);
            return user == null ? NotFound($"User with id: {id} was not found!") : Ok(user);
        }

        // Создание нового пользователя с хешированием пароля
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Password cannot be empty!");
            }

            // Хешируем пароль перед сохранением
            user.Password = Crypto.HashPassword(user.Password);

            _userRepository.AddUser(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // Обновление пользователя (логин, email, имя, пароль)
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            var existingUser = _userRepository.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound($"User with id: {id} was not found!");
            }

            existingUser.Login = user.Login;
            existingUser.Email = user.Email;
            existingUser.Name = user.Name;

            // Обновляем пароль, если он передан в запросе
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                existingUser.Password = Crypto.HashPassword(user.Password);
            }

            _userRepository.UpdateUser(existingUser);
            return Ok(existingUser);
        }

        // Удаление пользователя
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var existingUser = _userRepository.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound($"User with id: {id} was not found!");
            }

            _userRepository.DeleteUser(id);
            return NoContent();
        }

        // Информация о доступных методах API
        [HttpGet]
        public IActionResult Info()
        {
            return Ok(new
            {
                GET = "api/user/{id} - получить пользователя",
                POST = "api/user - создать нового пользователя (с хешированием пароля)",
                PUT = "api/user/{id} - обновить пользователя (если передан пароль, он хешируется)",
                DELETE = "api/user/{id} - удалить пользователя"
            });
        }
    }
}
