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
        private readonly IBidRepository _ibidRepo;

        public ItemController(
            ILogger<UserController> logger,
            IMapper automapper,
            IItemRepository itemRepo,
            IBidRepository ibidRepo
        )
        {
            _automapper = automapper;
            _logger = logger;
            _itemRepo = itemRepo;
            _ibidRepo = ibidRepo;
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
        public async Task<IActionResult> GetItemById([FromRoute] int id)
        {
            Item? item = await _itemRepo.GetItemByIdAsync(id);

            if (item == null)
            {
                _logger.LogInformation("Item not found.");
                return NotFound(new { itemNotFound = true });
            }

            var itemResponseDto = _automapper.Map<ItemResponseDto>(item);

            var biderIds = itemResponseDto.Bids!.Select(bid => bid.BiderId).ToList();

            // Retrieve bidder names using the repository 
            var bidderNames = await _ibidRepo.GetBidderNamesAsync(biderIds);

            // Update BidResponseDto objects with bidder names
            foreach (var bid in itemResponseDto.Bids!)
            {
                if (bidderNames.TryGetValue(bid.BiderId, out var bidderName))
                {
                    bid.BiderName = bidderName;
                }
            }

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
            // whitout pagination
            // List<Item>? items = await _itemRepo.GetItemsAsync();

            // with pagination
            PagedList<Item> items = await _itemRepo.GetItemsPaginatedAsync(parameters);
            var itemsResponseDto = _automapper.Map<List<ItemResponseDto>>(items);
            _logger.LogInformation("Items list pagenated sent.");
            return Ok(new { itemsPaginated = itemsResponseDto, totalElements = items.MetaData.TotalCount });
        }
    }
}