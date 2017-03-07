using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CodingAssignment.DataStore
{
    /// <summary>
    /// Simple in memory store.
    /// Basically, cutting down on libs used in a simple code project.
    /// In a real world app, this would probably use Dapper, Raven, or a Linq To SQL style lib behind the scenes. 
    /// </summary>
    public class InMemoryNoteStore : INoteStore
    {
        private IList<NoteModel> _notes;
        private IdGenerator _gen;

        public InMemoryNoteStore(IList<NoteModel> ctx, IdGenerator gen)
        {
            _gen = gen;
            _notes = ctx;
        }

        public IQueryable<NoteModel> Notes => _notes.AsQueryable();
        
        public NoteModel Add(NewNoteModel m)
        {
            var newNote = new NoteModel
            {
                Id = _gen.Next,
                Body = m.Body
            };
            _notes.Add(newNote);
            return newNote;
        }
        public NoteModel Get(int id)
        {
            return _notes.FirstOrDefault(n => n.Id == id);
        }
    }

    /// <summary>
    /// A simple way to safely generate ids for an in memory data store.
    /// </summary>
    public class IdGenerator
    {
        private int _counter = 0;
        public int Next => Interlocked.Increment(ref _counter);
    }
}
