using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Money.Models
{
    public class UpdateMonthlyBudgetViewModel
    {
        public int ID { get; set; }

        public int AccountID { get; set; }

        public string AccountName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public IEnumerable<MonthlyBudgetCategoryViewModel> Categories { get; set; }
    }
}
