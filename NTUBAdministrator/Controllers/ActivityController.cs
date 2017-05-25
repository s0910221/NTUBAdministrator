using System;
using System.Net;
using System.Web.Mvc;
using NTUBAdministrator.Services;
using NTUBAdministrator.Services.Interface;
using NTUBAdministrator.ViewModels;

namespace NTUBAdministrator.Controllers
{
    public class ActivityController : Controller
    {
        private IActivityService activityService;

        public ActivityController()
        {
            this.activityService = new ActivityService();
        }

        // GET: Activity/ActivityManagement
        public ActionResult ActivityManagement()
        {
            string accountID = "";
            try { accountID = Session["AccountID"].ToString(); }
            catch (NullReferenceException) { return new HttpStatusCodeResult(HttpStatusCode.Unauthorized); }
            return View(activityService.GetActivityManagement(accountID));
        }

        // GET: Activity/ActivityCalendar
        public ActionResult ActivityCalendar()
        {
            return View();
        }

        // GET: Activity/ActivityCalendarEvent
        public JsonResult ActivityCalendarEvent()
        {
            return activityService.GetActivityCalendarEvent();
        }

        // GET: Activity/ActivityCalendarDetail/ID
        public ActionResult ActivityCalendarDetail(int? id)
        {
            string accountID = "";
            try { accountID = Session["AccountID"].ToString(); }
            catch (NullReferenceException) { return new HttpStatusCodeResult(HttpStatusCode.Unauthorized); }
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ActivityCalendarDetailViewModel viewModel = activityService.GetActivityCalendarDetail(accountID, id.Value);
            if (viewModel == null) return HttpNotFound();
            return View(viewModel);
        }

        // POST: Activity/ApplyActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyActivity([Bind(Include = "ActivityID,Meal,NeedMeal,Attend")] ActivityCalendarDetailViewModel viewModel)
        {
            string accountID = "";
            try { accountID = Session["AccountID"].ToString(); }
            catch (NullReferenceException) { return new HttpStatusCodeResult(HttpStatusCode.Unauthorized); }
            if (viewModel.Attend.Equals("0")) return RedirectToAction("ActivityCalendar");

            if (!activityService.ApplyActivity(accountID, viewModel).Success) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return RedirectToAction("ActivityCalendar");
        }

        // POST: Activity/CancelApplyActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelApplyActivity([Bind(Include = "ActivityID,Attend")] ActivityCalendarDetailViewModel viewModel)
        {
            string accountID = "";
            try { accountID = Session["AccountID"].ToString(); }
            catch (NullReferenceException) { return new HttpStatusCodeResult(HttpStatusCode.Unauthorized); }
            if (viewModel.Attend.Equals("0")) return RedirectToAction("ActivityCalendar");

            if (!activityService.CancelApplyActivity(accountID, viewModel).Success) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return RedirectToAction("ActivityCalendar");
        }

        // GET: Activity/AddActivity
        public ActionResult AddActivity()
        {
            var query = activityService.GetActivityTypeList();
            SelectList activityTypeList = new SelectList(query, "ActivityTypeID", "TypeName");
            ViewBag.activityTypeList = activityTypeList;
            return View();
        }

        // POST: Activity/AddActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddActivity([Bind(Include = "ActivityName,Description,ActivityType,ApplicantLimit,Meal,StudyProof,StudyHours,ActivityStartTime,ActivityEndTime,ApplyStartTime,ApplyEndTime,ActivityOrganizer,ActivityCoOrganizer,ActivitySponsor")] ActivityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string accountID = "";
                try { accountID = Session["AccountID"].ToString(); }
                catch (NullReferenceException) { return new HttpStatusCodeResult(HttpStatusCode.Unauthorized); }
                if (activityService.AddActivity(accountID, viewModel).Success) return RedirectToAction("ActivityManagement");
            }
            return View(viewModel);
        }

        // GET: Activity/CheckActivity/id
        public ActionResult CheckActivity(int? id)
        {
            string accountID = "";
            try { accountID = Session["AccountID"].ToString(); }
            catch (NullReferenceException) { return new HttpStatusCodeResult(HttpStatusCode.Unauthorized); }
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ActivityCheckViewModel viewModel = activityService.GetActivityCheckView(accountID, id.Value);
            if (viewModel == null) return HttpNotFound();

            if (viewModel.SystemLevel == null) ViewBag.contentTitle = "活動內容";
            else ViewBag.contentTitle = "審核活動";
            return View(viewModel);
        }

        // POST: Activity/CheckActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckActivity([Bind(Include = "ActivityID,Status")] ActivityCheckViewModel viewModel)
        {
            if (!"-1".Equals(viewModel.Status))//按返回按鈕Status為-1
            {
                string accountID = "";
                try { accountID = Session["AccountID"].ToString(); }
                catch (NullReferenceException) { return new HttpStatusCodeResult(HttpStatusCode.Unauthorized); }
                if (!activityService.CheckActivity(accountID, viewModel).Success) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("ActivityManagement");
        }

        // GET: Activity/AddActivityType
        public ActionResult AddActivityType()
        {
            return View();
        }

        // POST: Activity/AddActivityType        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddActivityType([Bind(Include = "TypeName,Description")] ActivityTypeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string accountID = "";
                try { accountID = Session["AccountID"].ToString(); }
                catch (NullReferenceException) { return new HttpStatusCodeResult(HttpStatusCode.Unauthorized); }
                if (activityService.AddActivityType(accountID, viewModel).Success) return RedirectToAction("ActivityManagement");
            }
            return View(viewModel);
        }

        /*
        // GET: Activity/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activity.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activity/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActivityID,ActivityName,Description,ApplyType,ApplicantLimit,Meal,StudyProof,StudyHours,ActivityStartTime,ActivityEndTime,ApplyStartTime,ApplyEndTime,CreateID,CreateTime,ModifyID,ModifyTime,Record")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ActivityManagement");
            }
            return View(activity);
        }

        // GET: Activity/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activity.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activity.Find(id);
            db.Activity.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/

    }
}
