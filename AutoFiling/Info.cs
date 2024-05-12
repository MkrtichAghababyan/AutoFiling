using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFiling
{
    internal class Info
    {
        public int Id { get; set; }

        public bool? Status { get; set; } = true; 

        public string SectionName { get; set; } = string.Empty;

        public string Date { get; set; } = string.Empty;

        public string Hours { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string AllInfo { get; set; } = string.Empty;
        public string ImageLink { get; set; } = string.Empty;
        public string VideoLink { get; set; } = string.Empty;
    }
}
