using System;
using System.Collections.Generic;
using System.Text;

namespace CtsContestBoard.Dto
{
    public class PrizeAndApplicantDto : PrizeDto
    {
        public ParticipantDto Applicant { get; set; }
    }
}
