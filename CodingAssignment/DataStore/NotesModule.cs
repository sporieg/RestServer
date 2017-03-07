using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace CodingAssignment.DataStore
{
    public static class NotesModule
    {
        public static void RegisterNoteDataStore(this IServiceCollection services)
        {
            services.AddSingleton<IdGenerator>(new IdGenerator());
            services.AddSingleton<IList<NoteModel>>(new List<NoteModel>());
            services.AddTransient<INoteStore, InMemoryNoteStore>();
        }
    }
}
