﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace money.web.Models
{
    public class UpdateEntryViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Category")]
        public int? CategoryID { get; set; }

        [Display(Name = "Party")]
        public int? PartyID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        [StringLength(64)]
        public string Note { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        public IEnumerable<SelectListItem> Parties { get; set; }
    }
}
