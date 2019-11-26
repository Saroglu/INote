using INote.API.Dtos;
using INote.API.Extensions;
using INote.API.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace INote.API.Controllers
{
    [Authorize]
    public class NotesController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public IHttpActionResult GetNotes()
        {
            var id = User.Identity.GetUserId();
            var user = db.Users.Find(id);
            return Ok(user.Notes.Select(x=> new GetNoteDto {
                Id=x.id,
                Title= x.Title,
                Content= x.Content,
                CreatedTime= x.CreatedTime,
                ModifiedTime= x.ModifiedTime
            }).ToList());
        }
        public IHttpActionResult GetNote(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            return Ok(user.Notes.Where(x=> x.id==id).Select(x => new GetNoteDto
            {
                Id = x.id,
                Title = x.Title,
                Content = x.Content,
                CreatedTime = x.CreatedTime,
                ModifiedTime = x.ModifiedTime
            }).FirstOrDefault());
        }
        public IHttpActionResult PostNote(PostNoteDto dto)
        {
            if (ModelState.IsValid)
            {
                Note note = new Note
                {
                    Title = dto.Title,
                    Content = dto.Content,
                    CreatedTime = DateTime.Now,
                    ModifiedTime= DateTime.Now,
                    AuthorId=User.Identity.GetUserId()

                };
                db.Notes.Add(note);
                db.SaveChanges();
                return Ok(note.ToGetNoteDto());
            }
            return BadRequest(ModelState);
        }

        public IHttpActionResult PutNote(int id, PutNoteDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var note = db.Notes.Find(id);
                note.Title = dto.Title;
                note.Content = dto.Content;
                note.ModifiedTime = DateTime.Now;
                db.SaveChanges();

                return Ok(note.ToGetNoteDto());
            }

            return BadRequest(ModelState);
        }
        public IHttpActionResult DeleteNote(int id)
        {
            var note = db.Notes.Find(id);

            if(note == null)
            {
                return NotFound();
            }

            if (note.AuthorId != User.Identity.GetUserId())
            {

                return Unauthorized();
            }
            db.Notes.Remove(note);
            db.SaveChanges();

            return Ok(note.ToGetNoteDto());
        }
    }
}
