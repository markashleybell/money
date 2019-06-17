using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Money.Entities;
using Money.Models;
using Money.Support;

namespace Money.Controllers
{
    public class CategoriesController : MoneyControllerBase
    {
        public CategoriesController(
            IOptionsMonitor<Settings> optionsMonitor,
            IUnitOfWork unitOfWork,
            IQueryHelper db)
            : base(
                  optionsMonitor,
                  unitOfWork,
                  db)
        {
        }

        public IActionResult Index()
        {
            var sql = @"SELECT 
                            c.ID, 
                            a.Name AS Account, 
                            c.Name 
                        FROM 
                            Categories c 
                        INNER JOIN 
                            Accounts a ON a.ID = c.AccountID
                        ORDER BY
                            a.DisplayOrder,
                            c.DisplayOrder";

            var categories = Db.Query(conn => conn.Query<ListCategoriesCategoryViewModel>(sql));

            return View(new ListCategoriesViewModel {
                Categories = categories.GroupBy(c => c.Account)
            });
        }

        public IActionResult Create() =>
            View(new CreateCategoryViewModel {
                Accounts = AccountsSelectListItems()
            });

        [HttpPost]
        public IActionResult Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Accounts = AccountsSelectListItems();

                return View(model);
            }

            var category = new Category(
                accountID: model.AccountID,
                name: model.Name
            );

            Db.InsertOrUpdate(category);

            UnitOfWork.CommitChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            var dto = Db.Get<Category>(id);

            return View(new UpdateCategoryViewModel {
                ID = dto.ID,
                Name = dto.Name
            });
        }

        [HttpPost]
        public IActionResult Update(UpdateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = Db.Get<Category>(model.ID);

            var updated = dto.WithUpdates(name: model.Name);

            Db.InsertOrUpdate(updated);

            UnitOfWork.CommitChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dto = Db.Get<Category>(id);

            Db.Delete(dto);

            UnitOfWork.CommitChanges();

            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<SelectListItem> AccountsSelectListItems() =>
            Db.Query(conn => conn.Query<Account>("SELECT * FROM Accounts"))
               .Select(a => new SelectListItem { Value = a.ID.ToString(), Text = a.Name });
    }
}
