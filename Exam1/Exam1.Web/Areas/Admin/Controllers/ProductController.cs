using Exam1.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Autofac;
using Exam1.Infrastructure;
using System.Runtime.InteropServices.JavaScript;
using Exam1.Domain.Exceptions;

namespace Exam1.Web.Areas.Admin.Controllers
{
   
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILifetimeScope scope, ILogger<ProductController> logger)
        {
            _scope = scope;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var model = _scope.Resolve<ProductCreateModel>();
            //var model = new ProductCreateModel();
            return View(model);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Resolve(_scope);
                    await model.CreateProductAsync();
                    TempData.Put<ResponseModel>("ResponseMessage",
                        new ResponseModel
                        {
                            Message = "Course Created Successfully",
                            Type = ResponseTypes.Success
                        });
                    return RedirectToAction("Index");
                }
                catch(DuplicateNameException de)
                {
                    TempData.Put<ResponseModel>("ResponseMessage",
                        new ResponseModel
                        {
                            Message = de.Message,
                            Type = ResponseTypes.Danger

                        });
                }
                catch(Exception e)
                {
                    _logger.LogError(e, "Server error");
                    TempData.Put<ResponseModel>("ResponseMessage",
                        new ResponseModel
                        {
                            Message = "There is problem in creating Product",
                            Type = ResponseTypes.Danger

                        });
                }

            }
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetProducts( ProductListModel model)
        {
            var dataTableModel = new DataTablesAjaxRequestUtility(Request);
            model.Resolve(_scope);
            var data = await model.GetPagedProductsAsync(dataTableModel);
            return Json(data);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var model = _scope.Resolve<ProductUpdateModel>();
            await model.LoadAsync(id);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdateModel model)
        {
            model.Resolve(_scope);
            if(ModelState.IsValid)
            {
                try
                {
                    await model.UpdateProductAsync();
                    TempData.Put<ResponseModel>("ResponseMessage",
                        new ResponseModel
                        {
                             Message = "Product Updated Successfully",
                             Type = ResponseTypes.Success
                        });
                    return RedirectToAction("Index");
                }
                catch(DuplicateNameException de)
                {
                    TempData.Put<ResponseModel>("ResponseMessage",
                        new ResponseModel
                        {
                            Message = de.Message,
                            Type = ResponseTypes.Danger

                        });
                }
                catch(Exception e)
                {
                    _logger.LogError(e, "Server Error");
                    TempData.Put<ResponseModel>("ResponseMessage",
                        new ResponseModel
                        {
                            Message = "There is a problem in updating Product",
                            Type = ResponseTypes.Danger
                        });
                }
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = _scope.Resolve<ProductListModel>();
            if(ModelState.IsValid)
            {
                try
                {
                    await model.DeleteProductAsync(id);
                    TempData.Put<ResponseModel>("ResponseMessage",
                        new ResponseModel
                        {
                            Message = "Product Deleted Successfully",
                            Type = ResponseTypes.Success
                        });
                    return RedirectToAction("Index");
                }
                catch (DuplicateNameException de)
                {
                    TempData.Put<ResponseModel>("ResponseMessage",
                        new ResponseModel
                        {
                            Message = de.Message,
                            Type = ResponseTypes.Danger

                        });
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Server Error");
                    TempData.Put<ResponseModel>("ResponseMessage",
                        new ResponseModel
                        {
                            Message = "There is a problem in Deleting Course",
                            Type = ResponseTypes.Danger
                        });
                }

            }
            return RedirectToAction("Index");
        }
    }
}
