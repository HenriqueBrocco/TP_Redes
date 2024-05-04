using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP_Redes.Acessos;
using TP_Redes.Models;

namespace TP_Redes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosContext _context;

        public UsuariosController(UsuariosContext context)
        {
            _context = context;
        }

        /*Resgata todos os usuarios*/
        // GET: api/Usuarios
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuarios>>> GetUsuarios()
        {
            return await _context.UsuariosItems.ToListAsync();
        }

        /*Resgata um usuario pelo ID*/
        // GET: api/Usuarios/id
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuarios>> GetUsuario(int id)
        {
            var usuarios = await _context.UsuariosItems.FindAsync(id);

            if (usuarios == null)
            {
                return NotFound();
            }

            return usuarios;
        }

        /* Comentado, pois não necessidade de atualizar um usuário
        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarios(int id, Usuarios usuarios)
        {
            if (id != usuarios.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuarios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */

        /*Cria o usuário*/
        // POST: api/Usuarios
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Usuarios>> CadastraUsuario(string login, string senha, string nome)
        {
            var usuario = new Usuarios();

            var usua = _context.UsuariosItems.ForEachAsync(delegate (Usuarios user) {
                if (user.login == login)
                {
                    usuario.id = user.id;
                    usuario.nome = user.nome;
                    usuario.login = user.login;
                }
            });

            if (UsuariosExistsLogin(usuario.login ?? "") || UsuariosExistsId(usuario.id))
            {
                return StatusCode(401, "Login ja existe");
            }

            if (usuario.id == 0)
            {
                usuario.login = login;
                usuario.senha = senha;
                usuario.nome = nome;
            }

            if (!login.Contains("adm"))
            {
                usuario.roleId = 1;
            }
            else 
            {
                usuario.roleId = 2;
            }
            var user = _context.UsuariosItems.LastOrDefault();

            usuario.id = user == null ? 1 : user.id + 1;

            _context.UsuariosItems.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuarios", new { id = usuario.id }, usuario);
        }

        /*Login*/
        // POST: api/Usuarios/login
        [AllowAnonymous]
        [HttpPost("{login}")]
        public async Task<ActionResult<Usuarios>> Login(string login, string senha)
        {

            if (login == null)
                return StatusCode(401, "Usuario invalido");
            
            int id = 0;

            var usua = _context.UsuariosItems.ForEachAsync(delegate (Usuarios user) {
                if(user.login == login)
                {
                    id = user.id;
                }
            });

            var usuarios = await _context.UsuariosItems.FindAsync(id);

            if (usuarios == null || usuarios.senha != senha)
                return StatusCode(401, "Usuario ou senha invalido");

            try
            {
                var tokenService  = new TokenService();
                string token = tokenService.GenerateToken(usuarios);
                return Ok(token);
            }
            catch
            {
                return StatusCode(500, "Internal Error");
            }
        }

        /* Comentado, pois não necessidade de excluir um usuário
        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarios(int id)
        {
            var usuarios = await _context.UsuariosItems.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            _context.UsuariosItems.Remove(usuarios);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */

        private bool UsuariosExistsId(int id)
        {
            return _context.UsuariosItems.Any(e => e.id == id);
        }
        
        private bool UsuariosExistsLogin(string login)
        {
            return _context.UsuariosItems.Any(e => e.login == login);
        }
    }
}
