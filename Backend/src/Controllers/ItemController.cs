using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;
using src.Models.Dtos;

namespace src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _automapper;
        private readonly GreenBayDbContext _context;

        public ItemController(
            ILogger<UserController> logger,
            GreenBayDbContext context,
            IMapper automapper
        )
        {
            _automapper = automapper;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetItems()
        {
            List<Item>? items = await _context.TblItems
                .Where(item => item.IsSellable)
                .ToListAsync();


            var itemsDto = _automapper.Map<List<ItemResponseDto>>(items);
            _logger.LogInformation("Tickets list sent.");
            return Ok(itemsDto);
        }
    }
}