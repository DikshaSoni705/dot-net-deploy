using Microsoft.AspNetCore.Mvc;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll() =>
            Ok(await _service.GetAll());

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            var user = await _service.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> Create(User user)
        {
            await _service.Create(user);
            return Ok("User created");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, User user)
        {
            var existing = await _service.GetById(id);
            if (existing == null) return NotFound();

            user.Id = id;
            await _service.Update(id, user);

            return Ok("User updated");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _service.GetById(id);
            if (user == null) return NotFound();

            await _service.Delete(id);
            return Ok("User deleted");
        }
    }
}
