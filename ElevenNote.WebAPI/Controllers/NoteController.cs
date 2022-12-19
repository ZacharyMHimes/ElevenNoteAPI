using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElevenNote.Models.Note;
using ElevenNote.Services.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        public NoteController(INoteService noteService)
            {
                _noteService = noteService;
            }
        //POST api/Note
        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NoteCreate request)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                if (await _noteService.CreateNoteAsync(request))
                    return Ok("Note created successfully.");
                
                return BadRequest("Note could note be created.");
            }       
        //GET api/Note
        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
            {
                var notes = await _noteService.GetAllNotesAsync();
                return Ok();
            }
    }
    
}
