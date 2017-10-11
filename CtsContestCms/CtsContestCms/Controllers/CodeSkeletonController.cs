using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CtsContestCms.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace CtsContestCms.Controllers
{
    public class CodeSkeletonController : UmbracoApiController
    {
        // GET api/codeskeleton/get/{id}
        public CodeSkeletonDto Get(string id)
        {
            var skeletonDtos = new List<CodeSkeletonDto>();
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var skeletons = umbracoHelper.Content(1518).Children;

            foreach (var skeleton in skeletons)
            {
                skeletonDtos.Add(new CodeSkeletonDto
                {
                    Language = skeleton.Name,
                    Skeleton = skeleton.GetPropertyValue("skeleton")
                });
            }
            var skeletonDto = skeletonDtos.FirstOrDefault(s => s.Language.Equals(id));

            if (skeletonDto == null)
                return new CodeSkeletonDto
                {
                    Language = id,
                    Skeleton = string.Empty
                };

            return skeletonDto;
        }
    }
}
