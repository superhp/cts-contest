using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CtsContestCms.Filters;
using CtsContestCms.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace CtsContestCms.Controllers
{
    [ContestIPHandler]
    public class CodeSkeletonController : UmbracoApiController
    {
        // GET api/codeskeleton/get/{id}
        public GenericCodeSkeletonDto Get(string id)
        {
            var skeletonDtos = new List<GenericCodeSkeletonDto>();
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var skeletons = umbracoHelper.Content(1518).Children;

            foreach (var skeleton in skeletons)
            {
                skeletonDtos.Add(new GenericCodeSkeletonDto
                {
                    Language = skeleton.Name,
                    Skeleton = skeleton.GetPropertyValue("skeleton"),
                    ReadLine = skeleton.GetPropertyValue("inputReadLine").Replace("\\n", "\n"),
                    WriteLine = skeleton.GetPropertyValue("outputWriteLine"),
                    ReadInteger = skeleton.GetPropertyValue("inputReadInteger").Replace("\\n", "\n"),
                    ReadLineOfIntegers = skeleton.GetPropertyValue("inputReadLineOfIntegers").Replace("\\n", "\n"),
                    ReadInputIntegerNumberOfLinesOfIntegers = skeleton.GetPropertyValue("inputReadInputIntegerNumberOfLinesOfIntegers").Replace("\\n", "\n")
                });
            }
            var skeletonDto = skeletonDtos.FirstOrDefault(s => s.Language.Equals(id));

            if (skeletonDto == null)
                return new GenericCodeSkeletonDto
                {
                    Language = id,
                    Skeleton = string.Empty
                };

            return skeletonDto;
        }
    }
}
