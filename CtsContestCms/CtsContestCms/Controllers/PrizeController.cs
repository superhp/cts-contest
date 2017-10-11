using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CtsContestCms.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace CtsContestCms.Controllers
{
    public class PrizeController : UmbracoApiController
    {
        // GET api/prize/getAll
        public List<PrizeDto> GetAll()
        {
            var prizeDtos = new List<PrizeDto>();
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var prizes = umbracoHelper.Content(1164).Children;

            foreach (var prize in prizes)
            {
                var prizeDto = GetPrizeDto(prize);
                prizeDtos.Add(prizeDto);
            }

            return prizeDtos;
        }

        // GET api/prize/get/{id}
        public PrizeDto Get(int id)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var prize = umbracoHelper.Content(id);
            if (prize.Id == 0 || !prize.DocumentTypeAlias.Equals("prize"))
                throw new ArgumentException("No prize found with given ID");

            var prizeDto = GetPrizeDto(prize);

            return prizeDto;
        }

        private PrizeDto GetPrizeDto(dynamic prize)
        {
            return new PrizeDto
            {
                Id = prize.Id,
                Name = prize.Name,
                Price = prize.GetPropertyValue("price"),
                Quantity = prize.GetPropertyValue("quantity"),
                Picture = prize.GetPropertyValue("picture").Url,
                Category = prize.GetPropertyValue("category")
            };
        }
    }
}