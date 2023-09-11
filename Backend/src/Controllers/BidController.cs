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
using src.Models;
using src.Models.Dtos;
using src.Repository.IRepository;

namespace src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _automapper;
        private readonly IItemRepository _itemRepo;
        private readonly IBidRepository _bidRepo;
        private readonly IUserRepository _userRepo;
        private readonly GreenBayDbContext _context;

        public BidController(
            ILogger<UserController> logger,
            IMapper automapper,
            IItemRepository itemRepo,
            IBidRepository bidRepo,
            IUserRepository userRepo,
            GreenBayDbContext context
        )
        {
            _automapper = automapper;
            _logger = logger;
            _itemRepo = itemRepo;
            _bidRepo = bidRepo;
            _userRepo = userRepo;
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> BidItem([FromBody] BidRequestDto bidRequestDto)
        {
            if (bidRequestDto.BidAmount <= 0)
            {
                _logger.LogInformation("Invalid Bid Amount.");
                return BadRequest(new { bidAmountInvalid = true });
            }
            var bider = await _userRepo.FindUserById(bidRequestDto.BiderId);
            if (bider == null)
            {
                _logger.LogInformation("User not found.");
                return BadRequest(new { userNotFound = true });
            }
            // --- user exists ---
            var itemToBid = await _itemRepo.FindItemById(bidRequestDto.ItemId);
            if (itemToBid == null)
            {
                _logger.LogInformation("Item not found.");
                return BadRequest(new { itemNotFound = true });
            }
            // --- user && item exists ----
            if (!itemToBid.IsSellable)
            {
                // the item is not Sellabel.
                _logger.LogInformation("Item not sellable.");
                return BadRequest(new { notSallabel = true });
            }
            // --- user, item exists && IsSellable ----
            if (bider.Money < bidRequestDto.BidAmount)
            {
                _logger.LogInformation("Bider not enought money.");
                return StatusCode(StatusCodes.Status402PaymentRequired, new
                { notEnoughtMoneyToBid = true });
            }

            Bid? maxBid = await _bidRepo.GetMaxBidByItemId(bidRequestDto.ItemId);

            if (maxBid != null)
            {
                // bid list is not empty    
                if (bidRequestDto.BidAmount <= maxBid.BidAmount)
                {
                    _logger.LogInformation("Bid too low.");
                    return Ok(new { bidLow = true });
                }
            }

            if (bidRequestDto.BidAmount < itemToBid.Price)
            {
                // save the bid ---> insert data into bid table
                Bid newBid = _automapper.Map<Bid>(bidRequestDto);
                _logger.LogInformation("Bid added.");
                await _bidRepo.AddBidAsync(newBid);
                return Ok(new { bidSuccess = true });
            }

            if (bider.Money >= bidRequestDto.BidAmount)
            {
                // user buy the item ---> update item table
                itemToBid.IsSellable = false;
                itemToBid.BuyerId = bider.Id;
                _context.Entry(itemToBid).State = EntityState.Modified;

                // update user money. ---> update user table
                bider.Money -= bidRequestDto.BidAmount;
                _context.Entry(bider).State = EntityState.Modified;

                // save the bid ---> insert data into bid table
                Bid newBid = _automapper.Map<Bid>(bidRequestDto);
                await _bidRepo.AddBidAsync(newBid);
                _logger.LogInformation("Item Sold.");
                return Ok(new { itemSold = true, userMoney = bider.Money });
            }

            if (maxBid == null)
            {
                // bid list is empty, add the bid.
                Bid newBid = _automapper.Map<Bid>(bidRequestDto);
                await _bidRepo.AddBidAsync(newBid);
                _logger.LogInformation("Bid added.");
                return Ok(new { bidSuccess = true });
            }

            _logger.LogInformation("BidController WhatHappened?");
            return Ok(new { whatHappened = true });
        }
    }
}