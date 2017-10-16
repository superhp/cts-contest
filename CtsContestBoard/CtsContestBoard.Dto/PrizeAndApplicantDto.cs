using System;
using System.Collections.Generic;
using System.Text;

namespace CtsContestBoard.Dto
{
    public class PrizeAndApplicantDto : PrizeDto
    {
        public IEnumerable<ParticipantDto> Applicants { get; set; }
    }
}
