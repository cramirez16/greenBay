using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Src.Models;
using Src.Models.Dtos;
using Src.Models.Specifications;
using Src.Repository.IRepository;

namespace Src.Controllers
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

        [HttpGet] //http://localhost:5000/api/item
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetItems()
        // Using IEnumerable (readonly) instead List ( add & remove methods not needed). 
        {
            try
            {
                IEnumerable<Item>? items = await _itemRepo.GetItemsAsync();
                var itemsResponseDto = _automapper.Map<IEnumerable<ItemResponseDto>>(items);
                _logger.LogInformation("Items list sent.");
                return Ok(itemsResponseDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "An unexpected error occurred. Please try again later.");
            }

        }

        [HttpGet("{id}")] //http://localhost:5000/api/item/{id}
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ItemResponseDto>> GetItemById([FromRoute] int id)
        {
            try
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "An unexpected error occurred. Please try again later.");
            }

        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //in body { 'itemRequestDto', object }, url: http://localhost:5000/api/item 
        public async Task<ActionResult<ItemResponseDto>> PostItem([FromBody] ItemRequestDto itemRequestDto)
        {
            try
            {
                Item? itemEntities = _automapper.Map<Item>(itemRequestDto);
                await _itemRepo.SaveItemAsync(itemEntities);
                _logger.LogInformation("Created new item.");
                return Ok(_automapper.Map<ItemResponseDto>(itemEntities));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("Paginated")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //url: http://localhost:5000/api/item/paginated?pageNumber=1&pageSize=3
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetItemsPaginated([FromQuery] Parameters parameters)
        {
            if (parameters.PageNumber <= 0 || parameters.PageSize <= 0)
            {
                return BadRequest(new { parametersError = "Invalid parameters" });
            }
            try
            {
                // whitout pagination
                // List<Item>? items = await _itemRepo.GetItemsAsync();

                // with pagination
                PagedList<Item> items = await _itemRepo.GetItemsPaginatedAsync(parameters);
                var itemsResponseDto = _automapper.Map<IEnumerable<ItemResponseDto>>(items);
                _logger.LogInformation("Items list pagenated sent.");
                return Ok(new { itemsPaginated = itemsResponseDto, totalElements = items.MetaData.TotalCount });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}