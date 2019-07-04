using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WW.MPLN.Admin.Models;
using WW.MPLN.Core;

namespace WW.MPLN.Admin.Controllers
{
    public class MealController : Controller
    {
        private BizMeal _bizMeal;

        public MealController(BizMeal bizMeal)
        {
            _bizMeal = bizMeal;
        }

        public IActionResult Index(MealPlanQueryModel queryModel)
        {
            if (queryModel == null)
                queryModel = new MealPlanQueryModel();
            var result = _bizMeal.Query(queryModel.Key, queryModel.Page, queryModel.PageSize);

            MealPlanResultModel resultModel = new MealPlanResultModel()
            {
                Key = queryModel.Key,
                Page = queryModel.Page,
                PageSize = queryModel.PageSize,
                Items = result,
                ItemCount = result.Count,
            };
            return View(resultModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(MealPlanViewModel model)
        {
            JsonResult jsonResult = new JsonResult("");
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    jsonResult.Value = new { success = false, message = "Request error, please check input" };
                    return jsonResult;
                }
                if (string.IsNullOrEmpty(model.Name))
                {
                    jsonResult.Value = new { success = false, message = "Request error, please check input" };
                    return jsonResult;
                }
                if (model.Tags != null)
                    model.Tags = model.Tags.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct().ToArray();
                if (model.MealItems != null)
                {
                    foreach (var p in model.MealItems)
                    {
                        if (p.Tags == null)
                            continue;
                        p.Name = UpcaseString(p.Name);
                        p.Tags = p.Tags.Where(t => !string.IsNullOrWhiteSpace(t)).Distinct().ToArray();
                    }
                }
                var planID = _bizMeal.CreatePlan(model.Name, model.Description, model.Tags, model.MealItems);
                jsonResult.Value = new { success = true, message = "Success", data = planID };
                return jsonResult;
            }
            catch (Exception ex)
            {
                jsonResult.Value = new { success = false, message = ex.ToString() };
                return jsonResult;
            }
        }

        private string UpcaseString(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length < 2)
            {
                return input;
            }
            return string.Concat(input.Substring(0, 1).ToUpper(), input.Substring(1).ToLower());
        }

        public ActionResult Info(long id)
        {
            var plan = _bizMeal.GetPlan(id);
            if (plan == null)
                return View();
            var mealItems = _bizMeal.GetMealItems(plan.ID);
            MealPlanViewModel viewModel = new MealPlanViewModel()
            {
                Name = plan.Name,
                Description = plan.Description,
                ID = plan.ID,
                Tags = plan.Tags,
                Createtime = plan.Createtime,
                UpdateTime = plan.UpdateTime,
                MealItems = mealItems
            };
            return View(viewModel);
        }

        [HttpPost, HttpGet]
        public ActionResult AddMealItem(int planID, string mealType)
        {
            MealItem model = new MealItem() { PlanID = planID, Type = mealType };
            return View("EditMealItem", model);
        }

        [HttpPost, HttpGet]
        public ActionResult EditMealItem(int itemID)
        {
            MealItem model;
            if (itemID == 0)
            {
                model = null;
            }
            else
            {
                model = _bizMeal.GetMealItem(itemID);
            }
            if (model == null)
            {
                ViewBag.Message = "item not found";
            }
            return View("EditMealItem", model);
        }

        [HttpPost]
        public ActionResult UpsertMealItem(MealItem model)
        {
            JsonResult jsonResult = new JsonResult("");
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    jsonResult.Value = new { success = false, message = "Request error, please check input" };
                    return jsonResult;
                }
                if (model.PlanID == 0 || model.Name == null || model.Count <= 0 || string.IsNullOrEmpty(model.Unit) || string.IsNullOrEmpty(model.Type))
                {
                    jsonResult.Value = new { success = false, message = "Request error, please check input" };
                    return jsonResult;
                }
                model.Name = UpcaseString(model.Name);
                if (model.Tags != null)
                {
                    model.Tags = model.Tags.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct().ToArray();
                }
                bool success = false;
                if (model.ID > 0)
                {
                    success = _bizMeal.UpdateItem(model);
                }
                else
                {
                    success = _bizMeal.CreateMealItem(model);
                }
                if (success)
                {
                    jsonResult.Value = new { success = true, message = "Success" };
                }
                else
                {
                    jsonResult.Value = new { success = false, message = "No item Changed" };
                }
                return jsonResult;
            }
            catch (Exception ex)
            {
                jsonResult.Value = new { success = false, message = ex.ToString() };
                return jsonResult;
            }
        }

        [HttpPost]
        public ActionResult DeleteMealItem(int id)
        {
            JsonResult jsonResult = new JsonResult("");
            var result = _bizMeal.DeleteMealItem(id);
            jsonResult.Value = new { success = result, message = "" };
            return jsonResult;
        }
    }
}