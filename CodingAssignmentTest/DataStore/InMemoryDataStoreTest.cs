using CodingAssignment.DataStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CodingAssigmentTest.DataStore
{
    [TestClass]
    public class InMemoryDataStoreTest
    {
        [TestMethod]
        public void CanAddNotes()
        {
            var ds = new InMemoryNoteStore(new List<NoteModel>(), new IdGenerator());
            var newNote = ds.Add(new NewNoteModel { Body = "Lalala!" });
            Assert.IsTrue(newNote.Id > 0, "New notes should have a generated id.");
            Assert.AreEqual("Lalala!", newNote.Body);
        }

        [TestMethod]
        public void CanQueryNotes()
        {
            var ds = new InMemoryNoteStore(new List<NoteModel>(), new IdGenerator());
            ds.Add(new NewNoteModel { Body = "Milk" });
            ds.Add(new NewNoteModel { Body = "Lemonade" });
            ds.Add(new NewNoteModel { Body = "Ice Cream" });
            var milks = ds.Notes.Where(m => m.Body.Contains("Milk")).Count();
            Assert.AreEqual(1, milks, "Should have found one with milk.");
        }

        [TestMethod]
        public void CanGetById()
        {
            var ds = new InMemoryNoteStore(new List<NoteModel>(), new IdGenerator());
            ds.Add(new NewNoteModel { Body = "Milk" });
            var lemons = ds.Add(new NewNoteModel { Body = "Lemonade" }).Id;
            ds.Add(new NewNoteModel { Body = "Ice Cream" });
            var found = ds.Get(lemons);
            Assert.IsNotNull(found, "Should have found the lemonade");
            Assert.AreEqual("Lemonade", found.Body, "Should have found the lemonade");
        }

        [TestMethod]
        public void ReturnNullIfNotFound()
        {
            var ds = new InMemoryNoteStore(new List<NoteModel>(), new IdGenerator());
            ds.Add(new NewNoteModel { Body = "Milk" });
            ds.Add(new NewNoteModel { Body = "Lemonade" });
            ds.Add(new NewNoteModel { Body = "Ice Cream" });
            Assert.IsNull(ds.Get(10000), "Should not have found anything at id 10000");
            Assert.IsNull(ds.Get(-1), "Should not have found anything at id -1");
            Assert.IsNull(ds.Get(0), "Should not have found anything at id 0");

        }
    }
}
