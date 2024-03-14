using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace httprequest;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IHttpClientFactory _httpClientFactory;

    public UsuarioController(IHttpContextAccessor contextAccessor, IHttpClientFactory httpClientFactory)
    {
        _contextAccessor = contextAccessor;
        _httpClientFactory = httpClientFactory;
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

    [HttpGet("realizarChamadaHttpExemplo1")]
    public IActionResult RealizarChamadaHttpExemplo1()
    {
        using(HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", "value");
            client.DefaultRequestHeaders.Add("User-Agent", "Value");
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/posts");

            var result = client.GetAsync(client.BaseAddress).Result;
        
            return Ok(result);
        }
    }

    [HttpGet("realizarChamadaHttpExemplo2")]
    public IActionResult RealizarChamadaHttpExemplo2()
    { 
        HttpClient client = _httpClientFactory.CreateClient();  

        client.DefaultRequestHeaders.Add("Authorization", "value");
        client.DefaultRequestHeaders.Add("User-Agent", "Value");
        client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/posts");

        try
        {
            var result = client.GetAsync(client.BaseAddress).Result;
        
            return Ok(result);
        }
        catch
        {
            return null;
        }
    }

    [HttpGet("realizarChamadaHttpExemplo3")]
    public IActionResult RealizarChamadaHttpExemplo3()
    { 
        HttpClient client = _httpClientFactory.CreateClient();  
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://jsonplaceholder.typicode.com/posts"),
            Headers = {
                            { HttpRequestHeader.Accept.ToString(), "application/json" },
                            { "User", "User"},
                            { "Password", "Password" },
                            { "subscription-key", "subscription-key" }
                      }
        };
        HttpResponseMessage response = client.Send(httpRequestMessage);

        try
        {
            var result = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);

             return Ok(result);
        }
        catch
        {
            return null;
        }       
    }
}
