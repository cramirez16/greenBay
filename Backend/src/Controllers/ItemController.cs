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

namespace src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _automapper;
        private readonly GreenBayDbContext _context;
        private readonly IItemRepository _villaRepo;

        public ItemController(
            ILogger<UserController> logger,
            GreenBayDbContext context,
            IMapper automapper,
            IItemRepository villaRepo
        )
        {
            _automapper = automapper;
            _context = context;
            _logger = logger;
            _villaRepo = villaRepo;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetItems()
        {
            List<Item>? items = await _villaRepo.GetItemsAsync();

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

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> BidItem([FromBody] BidRequestDto bidRequestDto)
        {
            var bider = await _context.TblUsers.FindAsync(bidRequestDto.BiderId);
            if (bider == null)
            {
                _logger.LogInformation("User not found.");
                return NotFound();
            }
            // --- user exists ---
            var itemToBid = await _context.TblItems.FindAsync(bidRequestDto.ItemId);
            if (itemToBid == null)
            {
                _logger.LogInformation("Item not found.");
                return NotFound();
            }
            // --- user && item exists ----
            if (!itemToBid.IsSellable)
            {
                // the item is not Sellabel.
                _logger.LogInformation("Item not sellable.");
                return Ok(new { notSallabel = true });
            }
            // --- user, item exists && IsSellable ----
            if (bider.Money < bidRequestDto.BidAmount)
            {
                _logger.LogInformation("Bider not enought money.");
                return Ok(new { notEnoughtMoneyToBid = true });
            }
            // get the maxBbid of an specific item.
            var maxBid = await _context.TblBids
                .Where(bid => bid.ItemId == bidRequestDto.ItemId)
                .OrderByDescending(bid => bid.BidAmount)
                .Select(bid => new
                {
                    Id = bid.Id,
                    BidAmount = bid.BidAmount,
                    BiderId = bid.BiderId
                })
                .FirstOrDefaultAsync();
            if (maxBid != null)
            {
                if (bidRequestDto.BidAmount <= maxBid.BidAmount)
                {
                    _logger.LogInformation("Bid too low.");
                    return Ok(new { bidLow = true });
                }

                if (bidRequestDto.BidAmount < itemToBid.Price)
                {
                    // discount the bid from the account of the user
                    bider.Money -= bidRequestDto.BidAmount;
                    _context.Entry(bider).State = EntityState.Modified;

                    // update the bid list
                    var newBid = new Bid
                    {
                        BidAmount = bidRequestDto.BidAmount,
                        BiderId = bidRequestDto.BiderId,
                        ItemId = bidRequestDto.ItemId,
                    };
                    _context.TblBids.Add(newBid);

                    _logger.LogInformation("Bid added.");
                    await _context.SaveChangesAsync();
                    return Ok(new { bidSuccess = true });
                }
                if (bider.Money >= bidRequestDto.BidAmount)
                {
                    // user buy the item
                    itemToBid.IsSellable = false;
                    itemToBid.BuyerId = bider.Id;
                    _context.Entry(itemToBid).State = EntityState.Modified;
                    // update user money.
                    bider.Money -= bidRequestDto.BidAmount;
                    _context.Entry(bider).State = EntityState.Modified;
                    // update the bid list
                    var newBid = new Bid
                    {
                        BidAmount = bidRequestDto.BidAmount,
                        BiderId = bidRequestDto.BiderId,
                        ItemId = bidRequestDto.ItemId,
                    };
                    _context.TblBids.Add(newBid);

                    _logger.LogInformation("Item Sold.");
                    await _context.SaveChangesAsync();
                    return Ok(new { itemSold = true });
                }
            }
            else
            {
                // update user money.
                bider.Money -= bidRequestDto.BidAmount;
                _context.Entry(bider).State = EntityState.Modified;
                // no bids for the item yet --> add the bid
                var newBid = new Bid
                {
                    BidAmount = bidRequestDto.BidAmount,
                    BiderId = bidRequestDto.BiderId,
                    ItemId = bidRequestDto.ItemId,
                };

                // Add the new bid to the context
                _context.TblBids.Add(newBid);

                // Save the changes to the database
                await _context.SaveChangesAsync();
                return Ok(new { bidSuccess = true });
            }



            // _automapper.Map(ticket, ticketToUpdate);
            // await _context.SaveChangesAsync();
            // _logger.LogInformation("Updated ticket data.");
            return Ok(new { whatHappenned = true });
        }

    }
}