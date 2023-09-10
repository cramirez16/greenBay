using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Models.Dtos;
using src.Models.Specifications;
using src.Repository.IRepository;

namespace src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _automapper;
        private readonly IItemRepository _itemRepo;

        public ItemController(
            ILogger<UserController> logger,
            IMapper automapper,
            IItemRepository itemRepo
        )
        {
            _automapper = automapper;
            _logger = logger;
            _itemRepo = itemRepo;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetItems()
        {
            List<Item>? items = await _itemRepo.GetItemsAsync();
            var itemsResponseDto = _automapper.Map<List<ItemResponseDto>>(items);
            _logger.LogInformation("Items list sent.");
            return Ok(itemsResponseDto);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetTicket([FromRoute] int id)
        {
            Item? item = await _itemRepo.GetItemByIdAsync(id);

            if (item == null)
            {
                _logger.LogInformation("Item not found.");
                return NotFound();
            }

            var itemResponseDto = _automapper.Map<ItemResponseDto>(item);
            _logger.LogInformation("Item data sent.");
            return Ok(itemResponseDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> PostItem([FromBody] ItemRequestDto itemRequestDto)
        {
            Item? itemEntities = _automapper.Map<Item>(itemRequestDto);
            await _itemRepo.SaveItemAsync(itemEntities);
            _logger.LogInformation("Created new item.");
            return Ok(_automapper.Map<ItemResponseDto>(itemEntities));
        }

        [HttpGet("Paginated")] // /api/item/paginated
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetItemsPaginated([FromQuery] Parameters parameters)
        {
            PagedList<Item> items = await _itemRepo.GetItemsPaginatedAsync(parameters);
            // List<Item>? items = await _itemRepo.GetItemsAsync();
            var itemsResponseDto = _automapper.Map<List<ItemResponseDto>>(items);
            _logger.LogInformation("Items list pagenated sent.");
            return Ok(new { itemsPaginated = itemsResponseDto, totalPages = items.MetaData.TotalPages });
        }
    }
}