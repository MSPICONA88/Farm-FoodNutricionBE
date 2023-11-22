using System;
using System.Net.Mime;
using Frontend.Comandos.Usuarios;
using Frontend.Models;
using Frontend.Resultados;
using Frontend.Resultados.Usuarios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Frontend.Controllers;

[ApiController]

public class UsuarioController : ControllerBase
{
    private readonly FarmFoodNutricionContext _context;

    public UsuarioController(FarmFoodNutricionContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("api/usuario/login")]

    public async Task<ActionResult<ResultadoLogin>> Login([FromBody] ComandoLogin comando)
    {
        try
        {
            var result = new ResultadoLogin();
            var usuario = await _context.Usuarios.Include(u => u.IdRolNavigation).Where(
                c => c.Usuario1.Equals(comando.Usuario) &&
                c.Password.Equals(comando.Password)).FirstOrDefaultAsync();
            if (usuario != null)
            {
                result.NombreUsuario = usuario.Usuario1;
                result.Rol = usuario.IdRolNavigation.NombreRol;
                result.StatusCode = "200";
                return Ok(result);
            }
            else
            {
                result.SetError("Usuario no encontrado");
                result.StatusCode = "500";
                return Ok(result);
            }
        }
        catch (Exception e)
        {
            return BadRequest("Error al obtener el usuario");

        }
    }

    // [HttpPost]
    // [Route("api/usuario/alta")]

    // public async Task<ActionResult<ResultadoAltaUsuario>> AltaUsuario([FromBody] ComandoUsuario comando)
    // {
    //     // try
    //     {
    //         var result = new ResultadoAltaUsuario();

    //         var usuario = await _context.Usuarios.Include(u => u.IdRolNavigation).SingleOrDefaultAsync(u => u.IdRol == comando.IdRol);

    //         //usuario.Id= Guid.NewGuid();
    //         usuario.NombreApellido = comando.NombreApellido;
    //         usuario.Usuario1 = comando.Usuario1;
    //         usuario.Password = comando.Password;
    //         usuario.Email = comando.Email;
    //         usuario.IdRol = comando.IdRol;
    //         usuario.FechaCreacion = DateOnly.FromDateTime(DateTime.Today);

    //         if (usuario.NombreApellido == "" || usuario.Usuario1 == "" || usuario.Password == "" || usuario.Email == "" || usuario.IdRol.Equals(0))
    //         {
    //             result.SetError("Error al crear el usuario verificar campos");
    //             result.StatusCode = "500";
    //             return Ok(result);
    //         }
    //         else
    //         {
    //             await _context.AddAsync(usuario);
    //             await _context.SaveChangesAsync();

    //             result.NombreApellido = usuario.NombreApellido;
    //             result.Usuario1 = usuario.Usuario1;
    //             result.Email = usuario.Email;
    //             result.Rol = usuario.IdRolNavigation.NombreRol;

    //             result.StatusCode = "200";
    //             return Ok(result);
    //         }




    //     }
    //     // catch(Exception ex)
    //     // {

    //     //     return BadRequest("Error al crear el usuario: " +ex.Message);
    //     // }

    // }


    // gpt

    [HttpPost]
    [Route("api/usuario/alta")]

    public async Task<ActionResult<ResultadoAltaUsuario>> AltaUsuario([FromBody] ComandoUsuario comando)
    {
        var rol = await _context.Roles.FindAsync(comando.IdRol);
        var result = new ResultadoAltaUsuario();
        if (rol == null)
        {
            result.SetError("Rol no encontrado");
            result.StatusCode = "500";
            return Ok(result);
        }

        var usuario = new Usuario
        {
            NombreApellido = comando.NombreApellido,
            Usuario1 = comando.Usuario1,
            Password = comando.Password,
            Email = comando.Email,
            IdRol = comando.IdRol,
            FechaCreacion = DateOnly.FromDateTime(DateTime.Today),
            //IdRolNavigation = rol // asigna el rol al usuario
        };

        await _context.AddAsync(usuario);
        await _context.SaveChangesAsync();

        result.NombreApellido = usuario.NombreApellido;
        result.Usuario1 = usuario.Usuario1;
        result.Email = usuario.Email;
        result.Rol = usuario.IdRolNavigation?.NombreRol ?? "";

        result.StatusCode = "200";
        return Ok(result);
    }

[HttpGet]
[Route("api/usuario/todos")]
    public async Task<ActionResult<ResultadoListUsuarios>> GetUsuarios()
    {
        // try
        // {
            var result= new ResultadoListUsuarios();

            
            var usuarios=  await _context.Usuarios.Include(u => u.IdRolNavigation).ToListAsync();
            if(usuarios!=null){
                foreach (var user in usuarios){
                    var resultAux = new ResultadoListUsuariosItem
                    {
                        NombreApellido= user.NombreApellido,
                        NombreUsuario = user.Usuario1,
                        Email = user.Email,
                        Rol= user.IdRolNavigation.NombreRol
                        
                    };
                    result.listaUsuarios.Add(resultAux);
                    result.StatusCode= "200";
                }
                return Ok(result);
            }

            else
            {
                return Ok(result);
            }
        //}

        // catch(Exception e)
        // {
        //     return BadRequest("Error al obtener los usuarios");
        // }

        
        

    }
}






