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
    public class ItemsController : ControllerBase
    {
        private readonly DataContext _context;

        public ItemsController(DataContext context)
        {
            _context = context;
        }

     //יצירת פריט
        [HttpPost("AddItem/{userId}/{quesId}")]
        public async Task<IActionResult> AddItem(Item NewItem, int userId, int quesId)
        {
            string sessionContent = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(sessionContent) == false)
            {
                int sessionId = Convert.ToInt32(sessionContent);
                if (sessionId == userId)
                {
                    Item itemntoadd = await _context.Items.FirstOrDefaultAsync(q => q.ID == NewItem.ID);
                    if (itemntoadd == null)
                    {
                        _context.Items.Add(NewItem);
                        await _context.SaveChangesAsync();
                        return Ok(NewItem);
                    }
                    else
                    {
                        return BadRequest("item not create");
                    }
                }
                else
                {
                    return BadRequest("user not login");
                }
            }
            else
            {
                return BadRequest("Empty Session");
            }
        }


       //עריכת פריט עובד
        [HttpPost("EditItem/{userId}/{quesId}/{itemId}")]
        public async Task<IActionResult> ItemEdit(Item itemToEdit, int userId, int quesId, int itemId)
        {
            string sessionContent = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(sessionContent) == false)
            {
                int sessionId = Convert.ToInt32(sessionContent);
                if (sessionId == userId)
                {
                    Item ItemToReturn = await _context.Items.FirstOrDefaultAsync(q => q.ID == itemToEdit.QuestionID);
                    if (ItemToReturn != null)
                    {

                        Item itemFromDb = await _context.Items.Include(q => q.ItemQuestion).ThenInclude(i => i.QuestionItems).FirstOrDefaultAsync(i => i.ID == itemToEdit.ID);
                        if (itemFromDb != null)
                        {
                            itemFromDb.ItemContent = itemToEdit.ItemContent;
                            itemFromDb.IsPic = itemToEdit.IsPic;
                            itemFromDb.ItemPlace = itemToEdit.ItemPlace;
                            itemFromDb.ItemQuestion = itemToEdit.ItemQuestion;

                            await _context.SaveChangesAsync();
                            return Ok(itemFromDb);
                        }
                        return BadRequest("no such item..");
                    }
                    return BadRequest("no such question..");
                }
                return BadRequest("no question");
            }
            return BadRequest("Empty Session");


        }

       //מחיקת פריט מהבסיס נתונים עובד
        [HttpDelete("DeleteItem/{userId}/{quesId}/{itemId}")]
        public async Task<IActionResult> DeleteQuestion(int userId, int itemId, int quesId)
        {
            string sessionContent = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(sessionContent) == false)
            {
                int sessionID = Convert.ToInt32(sessionContent);
                if (sessionID == userId)
                {
                    Item ItemFromDb = await _context.Items.FirstOrDefaultAsync(u => u.ID == userId);
                    if (ItemFromDb != null)
                    {
                        _context.Items.Remove(ItemFromDb);
                        await _context.SaveChangesAsync();
                        return Ok(true);
                    }
                    return BadRequest("no such Item...");
                }
                return BadRequest("User not Login");
            }
            return BadRequest("EmptySession");
        }
    }

}
