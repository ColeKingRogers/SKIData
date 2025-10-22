using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using SKIData.Data;
using SKIData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SKIData.Services
{
    public class SkiResortService
    {
        private readonly string _skiResortListUrl = "https://www.colorado.com/articles/colorado-ski-resorts-snows-perfect-state"; // Replace with your URL
        private readonly int _delaySeconds = 2;
        private readonly SkiResortContext _context;

        public SkiResortService(SkiResortContext context)
        {
            _context = context;
        }
    }
}
