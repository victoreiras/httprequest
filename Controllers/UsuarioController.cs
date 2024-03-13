using Microsoft.AspNetCore.Mvc;

namespace httprequest;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UsuarioController(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    [HttpPost("cadastrarUsuarios")]
    public IActionResult CadastrarUsuarios(Usuario usuario)
    {        
        ObterEmpresaIdPeloHeader(_contextAccessor.HttpContext);
        return Ok();
    }

    private void ObterEmpresaIdPeloHeader(HttpContext? context)
    {
        if(context is not null)
        {
            var header = context.Request.Headers.Where(q => q.Key == "EmpresaId").FirstOrDefault();

            int empresaId = 0;

            if(header.Key is not null)
                empresaId = int.Parse(header.Value.ToString());
        }
    }

    [HttpGet("realizarChamadaHttp")]
    public IActionResult RealizarChamadaHttp()
    {
        using(var client = new HttpClient())
        {
            var endpoint = new Uri("https://jsonplaceholder.typicode.com/posts");
            var result = client.GetAsync(endpoint).Result;
        
            return Ok(result);
        }
    }
}
