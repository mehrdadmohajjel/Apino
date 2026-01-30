using Apino.Application.Interfaces;
using Apino.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Queries
{
    public class BranchQueryService
    {
        private readonly IReadDbContext _context;

        public BranchQueryService(IReadDbContext context)
        {
            _context = context;
        }

        public async Task<List<Branch>> GetActiveBranchesAsync()
        {
            return await _context.Branches
                .Where(x => x.IsActive)
                .OrderBy(x => x.Title)
                .ToListAsync();
        }
    }
}
