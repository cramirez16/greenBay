using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.JsonConverters;
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
                .Include(item => item.Seller)
                .Include(item => item.Bids)
                .Where(item => item.IsSellable)
                .ToListAsync();


            var itemsDto = _automapper.Map<List<ItemResponseDto>>(items);

            var serializerOptions = new JsonSerializerOptions
            {
                Converters = { new ItemResponseDtoConverter() },
                WriteIndented = true, // Format the JSON for readability
                //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Ignore null values
            };

            string jsonResult = JsonSerializer.Serialize(itemsDto, serializerOptions);

            _logger.LogInformation("Items list sent.");
            return Ok(jsonResult);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetTicket([FromRoute] int id)
        {
            Item? item = await _context.TblItems
                            .Include(item => item.Seller)
                            .Include(item => item.Bids)
                            .FirstOrDefaultAsync(item => item.Id == id);

            if (item == null)
            {
                _logger.LogInformation("Item not found.");
                return NotFound();
            }

            var itemResponseDto = _automapper.Map<ItemResponseDto>(item);

            var serializerOptions = new JsonSerializerOptions
            {
                Converters = { new ItemResponseDtoConverter() },
                WriteIndented = true, // Format the JSON for readability
                //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Ignore null values
            };

            string jsonResult = JsonSerializer.Serialize(itemResponseDto, serializerOptions);

            _logger.LogInformation("Item data sent.");
            return Ok(jsonResult);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> PostItem([FromBody] ItemRequestDto item)
        {
            var ItemEntities = _automapper.Map<Item>(item);
            await _context.TblItems.AddAsync(ItemEntities);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created new item.");
            return Ok();
        }
    }
}