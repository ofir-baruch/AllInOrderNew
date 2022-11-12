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
    public class QuestionController : ControllerBase
    {
        private readonly DataContext _context;

        public QuestionController(DataContext context)
        {
            _context = context;
        }

        


        //יצירת שאלה חדשה עובד
        [HttpPost("insertQ/{userId}/{gameId}")]
        public async Task<IActionResult> AddQues(int userId, int gameId, Question newQ)
        {
            
            string sessionContent = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(sessionContent) == false)
            {
                int sessionId = Convert.ToInt32(sessionContent);
                if (sessionId == userId)
                {
                   
                    if (newQ.QuestionText != null)
                    {
                        if (newQ.GameID == 0)
                        {
                            newQ.GameID = gameId;

                        }
                        _context.Questions.Add(newQ);
                        await _context.SaveChangesAsync();
                        return Ok(newQ);
                    }
                    return BadRequest("question not create");
                }
                return BadRequest("user not login");
            }
            return BadRequest("No Session");
        }


        //עריכת שאלה עובד
        [HttpPost("EditQuestion/{userId}/{gameId}")]
        public async Task<IActionResult> QuestionEdit(Question questionToEdit, int userid, int gameId)
        {
            string sessionContent = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(sessionContent) == false)
            {
                int sessionId = Convert.ToInt32(sessionContent);
                if (sessionId == userid)
                {
                    User UserToReturn = await _context.Users.FirstOrDefaultAsync(u => u.ID == userid);
                    if (UserToReturn != null)
                    {

                        Question questionFromDb = await _context.Questions.Include(q => q.QuestionItems).FirstOrDefaultAsync(qt => qt.ID == questionToEdit.ID);
                        if (questionFromDb != null)
                        {
                            questionFromDb.QuestionText = questionToEdit.QuestionText;
                            questionFromDb.QuestionPic = questionToEdit.QuestionPic;
                            questionFromDb.FirstText = questionToEdit.FirstText;
                            questionFromDb.LastText = questionToEdit.LastText;
                            questionFromDb.QuestionItems = questionToEdit.QuestionItems;

                            await _context.SaveChangesAsync();
                            return Ok(questionFromDb);
                        }
                        return BadRequest("no such question..");
                    }
                    return BadRequest("user not found");
                }
                return BadRequest("user not login");
            }
            return BadRequest("Empty Session");

        }

       //מחיקת שאלה מהבסיס נתונים עובד
        [HttpDelete("DeleteQuestion/{userId}/{QuesId}")]
        public async Task<IActionResult> DeleteQuestion(int userId, int QuesId)
        {
            string sessionContent = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(sessionContent) == false)
            {
                int sessionID = Convert.ToInt32(sessionContent);
                if (sessionID == userId)
                {
                    Question QuestionFromDb = await _context.Questions.Include(q => q.QuestionItems).FirstOrDefaultAsync(p => p.ID == QuesId);
                    if (QuestionFromDb != null)
                    {
                        _context.Questions.Remove(QuestionFromDb);
                        await _context.SaveChangesAsync();
                        return Ok(true);
                    }
                    return BadRequest("no such Question...");
                }
                return BadRequest("User not Login");
            }
            return BadRequest("EmptySession");
        }

        //הצגת כל השאלות
        [HttpGet("QustionData/{GameId}")]
        public async Task<IActionResult> GetQustionData(int gameId)
        {
           List<Question> Qdata = await _context.Questions.Include(gq => gq.QuestionItems).Where(g => g.GameID == gameId).ToListAsync();
            if (Qdata != null)
            {
                return Ok(Qdata);
            }
            else
            {
                return BadRequest("Question not found");
            }

        }

    }

}




       




