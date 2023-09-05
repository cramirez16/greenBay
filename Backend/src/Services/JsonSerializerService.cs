using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using src.JsonConverters;
using src.Models.Dtos;

namespace src.Services
{
    public class JsonSerializerService
    {

        public string ItemResponseDtoToJson(Object itemsDto)
        {
            var serializerOptions = new JsonSerializerOptions
            {
                Converters = { new ItemResponseDtoConverter() },
                WriteIndented = true, // Format the JSON for readability
                //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Ignore null values
            };

            return JsonSerializer.Serialize(itemsDto, serializerOptions);
        }
    }
}