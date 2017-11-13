﻿using money.web.Models.DTO;
using System.Collections.Generic;

namespace money.web.Models
{
    public class ListMonthlyBudgetsViewModel
    {
        public int AccountID { get; set; }

        public IEnumerable<MonthlyBudgetDTO> MonthlyBudgets { get; set; }
    }
}