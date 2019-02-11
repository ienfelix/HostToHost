using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Threading.Tasks;

namespace Comun
{
    public static class RenderViewOrPartialView
    {
        public static async Task<string> RenderViewAsync<TModel>(this Controller controller, string viewName, TModel model, bool partial = false)
        {
            try
            {
                if (string.IsNullOrEmpty(viewName))
                {
                    viewName = controller.ControllerContext.ActionDescriptor.ActionName;
                }

                controller.ViewData.Model = model;

                using (var writer = new StringWriter())
                {
                    IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                    ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);

                    if (viewResult.Success == false)
                    {
                        return null;
                    }

                    ViewContext viewContext = new ViewContext(
                        controller.ControllerContext,
                        viewResult.View,
                        controller.ViewData,
                        controller.TempData,
                        writer,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);

                    var stringContent = writer.GetStringBuilder().ToString();
                    return stringContent;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}