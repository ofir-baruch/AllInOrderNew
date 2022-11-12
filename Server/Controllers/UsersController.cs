using AllinOrder_Shahaf_Ofir_Snir.Server.Data;
using AllinOrder_Shahaf_Ofir_Snir.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllinOrder_Shahaf_Ofir_Snir.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        //בניית שיטת התחברות
        [HttpGet("{mail}")]
        public async Task<IActionResult>UserLogin (string mail)
        {
            User UserToReturn = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail.ToLower() == mail.ToLower());
            if (UserToReturn != null)
            {
                HttpContext.Session.SetString("UserId", UserToReturn.ID.ToString());
                return Ok(UserToReturn.ID);
            }

            return BadRequest("User not found");
        }



    }
}
