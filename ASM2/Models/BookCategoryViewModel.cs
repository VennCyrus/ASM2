using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FPTBOOK_STORE.Models
{
    public class BookCategoryViewModel
    {
        public List<Book>? Books { get; set; }
        public SelectList? Categories { get; set; }
        public string? BookCategory { get; set; }
        public string? Search { get; set; }
    }
}