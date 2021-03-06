﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtsContestWeb.Db.Entities
{
    public class TaskTestCase : IAuditable
    {
        public int TaskTestCaseId { get; set; }
        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public virtual Task Task { get; set; }
        public byte[] Input { get; set; }
        public byte[] Output { get; set; }
        public bool IsSample { get; set; }
        public DateTime Created { get; set; }
    }
}
