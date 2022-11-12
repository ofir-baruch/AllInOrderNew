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
    public class GamesController : ControllerBase
    {
        private readonly DataContext _context;

        public GamesController(DataContext context)
        {
            _context = context;
        }



        //שליפת כל המשחקים של העורך
        [HttpGet("{userIdClient}")]
        public async Task<IActionResult> GetAllGames(int userIdClient)
        {
            string sessionContent = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(sessionContent) == false)
            {
                int SessionID = Convert.ToInt32(sessionContent);

                if (SessionID == userIdClient)
                {
                    User UserFromDB = await _context.Users.Include(u => u.UserGames).ThenInclude(q => q.GameQuestions).FirstOrDefaultAsync(u => u.ID == userIdClient);

                    if (UserFromDB != null)
                    {
                        return Ok(UserFromDB);
                    }    
                    else
                    {
                        return BadRequest("User not found");
                    }

                }
                else
                {
                    return BadRequest("User not login");
                }
            }
            else
            {
                return BadRequest("Empty Session");
            }
        }


        // שליפת המשחק לפי קוד המשחק
        [HttpGet("byCode/{gameCodeFromClient}")]
            public async Task<IActionResult> GetGameByCode(int gameCodeFromClient)
            {
                Game gameFromDB = await _context.Games.Include(g => g.GameQuestions).ThenInclude(q => q.QuestionItems).FirstOrDefaultAsync(g => g.GameCode == gameCodeFromClient);
                if (gameFromDB != null)
                {
                    if (gameFromDB.IsPublish == true)
                    {
                        return Ok(gameFromDB);
                    }
                    else
                    {
                        return BadRequest("Game not publish");
                    }
                }
                else
                {
                    return BadRequest("Game not found");
                }
            }

            //יצירת משחק חדש עם קוד משחק אוטומטי
            [HttpPost]
            public async Task<IActionResult> AddNewGame(Game GameToAdd)
            {
                string SessionContent = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(SessionContent) == false)
                {
                    int SessionID = Convert.ToInt32(SessionContent);
                    if (SessionID == GameToAdd.UserID)
                    {
                        User UserFromDB = await _context.Users.Include(u => u.UserGames).FirstOrDefaultAsync(u => u.ID == GameToAdd.UserID);
                        if (UserFromDB != null)
                        {
                            UserFromDB.UserGames.Add(GameToAdd);
                            await _context.SaveChangesAsync();
                            GameToAdd.GameCode = GameToAdd.ID + 100;
                            await _context.SaveChangesAsync();
                            return Ok(GameToAdd);
                        }
                        else
                        {
                            return BadRequest("User not found");
                        }
                    }
                    else
                    {
                        return BadRequest("User not login");
                    }
                }
                else
                {
                    return BadRequest("Empty session");
                }
            }

            //מחיקת משחק מהבסיס נתונים
            [HttpDelete("DeleteGame/{UserId}/{GameID}")]
            public async Task<IActionResult> DeleteGame(int userId, int GameID)
            {
                string sessionContent = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(sessionContent) == false)
                {
                    int sessionID = Convert.ToInt32(sessionContent);
                    if (sessionID == userId)
                    {
                        Game gametodelete = await _context.Games.Include(g => g.GameQuestions).ThenInclude(q => q.QuestionItems).FirstOrDefaultAsync(g => g.ID == GameID);
                        if (gametodelete != null)
                        {
                            _context.Games.Remove(gametodelete);
                            await _context.SaveChangesAsync();
                            return Ok(true);

                        }
                        return BadRequest("no such Game...");
                    }
                    return BadRequest("User not Login");
                }
                return BadRequest("Empty Session");
            }

            //עריכת משחק
            [HttpPost("EditGame")]
            public async Task<IActionResult> GameEdit(Game gametoEdit)
            {
                string sessionContent = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(sessionContent) == false)
                {
                    int sessionId = Convert.ToInt32(sessionContent);
                    if (sessionId == gametoEdit.UserID)
                    {
                        User UserToReturn = await _context.Users.FirstOrDefaultAsync(u => u.ID == gametoEdit.UserID);
                        if (UserToReturn != null)
                        {
                            Game gameFromDb = await _context.Games.Include(g => g.GameQuestions).ThenInclude(q => q.QuestionItems).FirstOrDefaultAsync(g => g.ID == gametoEdit.ID);
                            if (gameFromDb != null)
                            {
                                gameFromDb.GameName = gametoEdit.GameName;
                                gameFromDb.QuestionTime = gametoEdit.QuestionTime;
                                gameFromDb.IsPublish = gametoEdit.IsPublish;
                                gameFromDb.GameCode = gametoEdit.GameCode;

                                await _context.SaveChangesAsync();
                                return Ok(gameFromDb);
                            }
                            return BadRequest("no such game..");
                        }
                        return BadRequest("user not found");
                    }
                    return BadRequest("user not login");
                }
                return BadRequest("Empty Session");
            }

        //שליפת כל נתוני המשחק
        [HttpGet("GameData/{GameId}")]
        public async Task<IActionResult> GetGameData(int gameId)
        {
            Game gameData = await _context.Games.Include(gu => gu.GameUser).Include(gq => gq.GameQuestions).ThenInclude(qi => qi.QuestionItems).FirstOrDefaultAsync(g => g.ID == gameId);
            if (gameData != null)
            {
                return Ok(gameData);
            }
            else
            {
                return BadRequest("game not found");
            }
        }



        //פרסום
        [HttpPost("UpdatePublish")]
            public async Task<IActionResult> UpdatePublish(Game myGame)
            {

                string sessionContent = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(sessionContent) == false)
                {

                    int sessionId = Convert.ToInt32(sessionContent);
                    if (sessionId == myGame.UserID)
                    {
                        User userToReturn = await _context.Users.Include(u => u.UserGames).FirstOrDefaultAsync(u => u.ID == myGame.UserID);
                        if (userToReturn != null)
                        {
                            Game GameFromDb = await _context.Games.FirstOrDefaultAsync(g => g.ID == myGame.ID);

                            if (GameFromDb != null)
                            {

                                GameFromDb.IsPublish = myGame.IsPublish;
                                await _context.SaveChangesAsync();
                                return Ok(GameFromDb);
                            }
                            else
                            {
                                return BadRequest("no such Game...");
                            }
                        }
                        else
                        {
                            return BadRequest("User not login");
                        }
                    }
                    return BadRequest("The user is all ready login ");
                }
                return BadRequest("empty session");


            }

        }
    }





    
