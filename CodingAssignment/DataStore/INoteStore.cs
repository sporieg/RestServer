using System.Linq;

namespace CodingAssignment.DataStore
{
    /// <summary>
    /// A common note data store.  Follows a very basic data access pattern.
    /// Works well enough in a traditional full stack app.
    /// A more complex app might be better served by a CQS style interface.
    /// </summary>
    public interface INoteStore
    {
        /// <summary>
        /// Expose a method to build dynamic queries.
        /// </summary>
        IQueryable<NoteModel> Notes { get; }
        /// <summary>
        /// Add notes to the Store.
        /// </summary>
        /// <param name="m">The note to be added.</param>
        /// <returns>The note stored, with its id.</returns>
        NoteModel Add(NewNoteModel m);
        /// <summary>
        /// Get an individual note.
        /// Returns null if none are found.
        /// </summary>
        /// <param name="id">Id of the note you want.</param>
        /// <returns>The note if found, null otherwise.</returns>
        NoteModel Get(int id);
    }
}