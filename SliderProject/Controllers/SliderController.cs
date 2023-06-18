using SliderProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SliderProject.Controllers
{
    public class SliderController : Controller
    {

        DBSliderEntities db = new DBSliderEntities();
        public ActionResult Index()
        {
            var degerler = db.Slider.ToList();
            return View(degerler);
        }

        [HttpGet]
        public ActionResult YeniSlider()
        {
            return View();
        }

        [HttpPost]
        public ActionResult YeniSlider([Bind(Include = "SliderURL")] Slider s, HttpPostedFileBase SliderURL)
        {
            if (SliderURL != null)
            {
                WebImage img = new WebImage(SliderURL.InputStream);
                FileInfo imginfo = new FileInfo(SliderURL.FileName);

                string sliderimgname = Guid.NewGuid().ToString() + imginfo.Extension;
                img.Resize(1336, 380);
                img.Save("~/Upload/Slider/" + sliderimgname);

                s.sliderURL = "/Upload/Slider/" + sliderimgname;
            }

            db.Slider.Add(s);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SliderSil(int id)
        {
            var slider = db.Slider.Find(id);
            if (System.IO.File.Exists(Server.MapPath(slider.sliderURL)))
            {
                System.IO.File.Delete(Server.MapPath(slider.sliderURL));
            }
            db.Slider.Remove(slider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}