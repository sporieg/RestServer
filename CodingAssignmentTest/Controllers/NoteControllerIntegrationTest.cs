using CodingAssignment;
using CodingAssignment.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CodingAssignmentTest.Controllers
{
    [TestClass]
    public class NoteControllerIntegrationTest
    {

        private readonly TestServer _server;
        private readonly HttpClient _client;

        public NoteControllerIntegrationTest()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();

        }

        [TestMethod]
        public void CanPostNewNote()
        {
            Task.Run(() => CanPostNewNoteAsync()).Wait();
        }

        private async Task CanPostNewNoteAsync()
        {
            NoteViewModel note = await PostNote("CanPostNewNoteAsync");
            Assert.AreEqual("CanPostNewNoteAsync", note.Body);
            Assert.IsTrue(note.Id > 0);
        }

        [TestMethod]
        public void CanGetNote()
        {
            Task.Run(() => CanGetNoteAysnc()).Wait();

        }

        private async Task CanGetNoteAysnc()
        {
            NoteViewModel note = await PostNote("CanGetNoteAysnc");
            var testNote = GetNote(note.Id);
            Assert.AreEqual("CanGetNoteAysnc", note.Body);
            Assert.IsTrue(note.Id > 0);
        }

        [TestMethod]
        public void CanGetAllNotes()
        {
            Task.Run(() => CanGetAllNotesAysnc()).Wait();
        }

        private async Task CanGetAllNotesAysnc()
        {
            await PostNote("CanGetAllNotesAysnc");
            await PostNote("CanGetAllNotesAysnc1");
            await PostNote("CanGetAllNotesAysnc2");
            var notes = await GetNotes();
            //Do a greater than check, as other tests may have added notes.
            Assert.IsTrue(notes.Count > 2);
        }

        [TestMethod]
        public void CanQueryNotes()
        {
            Task.Run(() => CanQueryNotesAsync()).Wait();

        }

        private async Task CanQueryNotesAsync()
        {
            await PostNote("CanQueryNotesAsync");
            await PostNote("CanQueryNotesAsync1");
            await PostNote("CanQueryNotesAsync2");
            var notes = await QueryNotes("CanQueryNotesAsync");
            //Do an exact match, should only return our specific notes.
            Assert.IsTrue(notes.Count == 3);
            Assert.IsTrue(notes.All(n => n.Body.Contains("CanQueryNotesAsync")));

            notes = await QueryNotes("CanQueryNotesAsync1");
            Assert.IsTrue(notes.Count == 1);
            Assert.IsTrue(notes.All(n => n.Body.Contains("CanQueryNotesAsync1")));

        }


        private async Task<NoteViewModel> PostNote(string body)
        {
            var msg = new StringContent($"{{\"Body\":\"{body}\"}}", Encoding.UTF8, "application/json");
            var resp = await _client.PostAsync("/Api/Notes", msg);
            return await ToNote(resp);
        }

        private async Task<NoteViewModel> GetNote(int id)
        {
            var resp = await _client.GetAsync($"/Api/Notes/{id}");
            return await ToNote(resp);
        }

        private async Task<IList<NoteViewModel>> GetNotes()
        {
            var resp = await _client.GetAsync($"/Api/Notes");
            return await ToList(resp);
        }

        private async Task<IList<NoteViewModel>> QueryNotes(string q)
        {
            var resp = await _client.GetAsync($"/Api/Notes?query={q}");
            return await ToList(resp);
        }

        private async Task<NoteViewModel> ToNote(HttpResponseMessage resp)
        {
            resp.EnsureSuccessStatusCode();
            var str = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<NoteViewModel>(str);
        }

        private static async Task<IList<NoteViewModel>> ToList(HttpResponseMessage resp)
        {
            resp.EnsureSuccessStatusCode();
            var str = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<NoteViewModel>>(str);
        }
    }
}
