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
using src.Repository.IRepository;
using src.Services;

namespace src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _automapper;
        private readonly IItemRepository _villaRepo;
        private readonly JsonSerializerService _jsonSerializer;

        public ItemController(
            ILogger<UserController> logger,
            IMapper automapper,
            IItemRepository villaRepo,
            JsonSerializerService jsonSerializer
        )
        {
            _automapper = automapper;
            _logger = logger;
            _villaRepo = villaRepo;
            _jsonSerializer = jsonSerializer;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetItems()
        {
            List<Item>? items = await _villaRepo.GetItemsAsync();
            var itemsDto = _automapper.Map<List<ItemResponseDto>>(items);
            _logger.LogInformation("Items list sent.");
            return Ok(_jsonSerializer.ItemResponseDtoToJson(itemsDto));
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetTicket([FromRoute] int id)
        {
            Item? item = await _villaRepo.GetItemByIdAsync(id);

            if (item == null)
            {
                _logger.LogInformation("Item not found.");
                return NotFound();
            }

            var itemResponseDto = _automapper.Map<ItemResponseDto>(item);
            _logger.LogInformation("Item data sent.");
            return Ok(_jsonSerializer.ItemResponseDtoToJson(itemResponseDto));
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> PostItem([FromBody] ItemRequestDto itemRequestDto)
        {
            Item? ItemEntities = _automapper.Map<Item>(itemRequestDto);
            await _villaRepo.SaveItemAsync(ItemEntities);
            _logger.LogInformation("Created new item.");
            return Ok();
        }
    }
}