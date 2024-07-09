using DWEB_NET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace DWEB_NET.Controllers.API
{

    [Route("api/[controller]")]
    [ApiController]
    public class V1 : ControllerBase
    {
        public ApplicationDbContext _Context;

        public V1(ApplicationDbContext applicationDbContext) 
        { 
            _Context = applicationDbContext;
        }



        [HttpGet]
        [Route("GetUsers")]
        public ActionResult GetUsers()
        {
            var x = _Context.Utilizadores.ToList();
            
            return Ok(x);
        }

        [HttpGet]
        [Route("GetAccount")]

        public ActionResult GetAccount()
        {
            var x = _Context.Contas.ToList();
            return Ok(x);
        }



    }
}
