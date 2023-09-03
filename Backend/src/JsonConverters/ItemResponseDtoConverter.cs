using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using src.Models.Dtos;

namespace src.JsonConverters
{
    public class ItemResponseDtoConverter : JsonConverter<ItemResponseDto>
    {
        public override ItemResponseDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, ItemResponseDto value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteNumber("id", value.Id);
            writer.WriteString("name", value.Name);
            writer.WriteString("photoUrl", value.PhotoUrl);
            writer.WriteNumber("bid", value.Bid);
            writer.WriteBoolean("isSellable", value.IsSellable);
            writer.WriteString("creationDate", value.CreationDate);
            writer.WriteString("updateDate", value.UpdateDate);
            writer.WriteNumber("sellerId", value.SellerId);
            writer.WriteString("sellerName", value.SellerName);
            writer.WriteNumber("buyerId", value.BuyerId);
            writer.WriteString("buyerName", value.BuyerName);

            // Include the Bids property
            writer.WriteStartArray("bids");
            foreach (var bid in value.Bids!)
            {
                writer.WriteStartObject();
                writer.WriteNumber("id", bid.Id);
                // Include other properties of Bid as needed
                writer.WriteNumber("biderId", bid.BiderId);
                writer.WriteString("biderName", bid.User?.Name != null ? bid.User.Name : "");
                writer.WriteNumber("itemId", bid.ItemId);
                writer.WriteEndObject();
            }


            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}