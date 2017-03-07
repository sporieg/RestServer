using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CodingAssignment.DataStore;
using System.Linq;
using System;

namespace CodingAssignment.Controllers
{
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        public INoteStore _store;

        public NotesController(INoteStore store)
        {
            _store = store;
        }


        // GET api/values
        [HttpGet]
        public IList<NoteViewModel> Get(string query) => _store.Notes
            .Where(n => query == null || n.Body.Contains(query))
                .Select(n => new NoteViewModel { Id = n.Id, Body = n.Body})
                .ToList();
        

        // GET api/values/5
        [HttpGet("{id}")]
        public NoteViewModel Get(int id)
        {
            var n = _store.Get(id);
            if(n == null)
            {
                throw new Exception($"No note saved for {id}");
            }
            return new NoteViewModel
            {
                Id = n.Id,
                Body = n.Body
            };
        }

        // POST api/values
        [HttpPost]
        public NoteViewModel Post([FromBody]PostNoteModel n)
        {
            var newNote = _store.Add(new NewNoteModel { Body = n.Body });
            return new NoteViewModel
            {
                Id = newNote.Id,
                Body = newNote.Body
            };
        }
    }
}
