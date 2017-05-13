using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using NTUBAdministrator.Models;
using NTUBAdministrator.ViewModels;
using NTUBAdministrator.Utils;
using System.Text;

namespace NTUBAdministrator.Controllers
{
    public class ActivitiesController : Controller
    {
        private Entities db = new Entities();

        // GET: Activities/ActivityManagement
        public ActionResult ActivityManagement()
        {
            string accountID = Session["AccountID"] == null ? null : Session["AccountID"].ToString();
            if (accountID == null) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            int? systemLevel = (from user in db.UserAccount
                                where user.AccountID == accountID
                                select new { user.SystemLevel }).FirstOrDefault().SystemLevel;

            if (systemLevel == null)//學生 教職員
            {
                //var activities = db.Activity;
                var viewModel = db.Activity.Where(c => c.CreateID == accountID).Select(c => new ActivitiesManagementViewModel()
                {
                    ActivityID = c.ActivityID,
                    ActivityName = c.ActivityName,
                    Status = c.Status
                });
                return View(viewModel);
            }
            else//管理者
            {
                return View(db.Activity.Select(c => new ActivitiesManagementViewModel()
                {
                    ActivityID = c.ActivityID,
                    ActivityName = c.ActivityName,
                    Status = c.Status
                }));
            }
        }

        // GET: Activities/ActivityCalendar
        public ActionResult ActivityCalendar()
        {
            return View();
        }

        // GET: Activities/ActivityCalendarEvent
        public JsonResult ActivityCalendarEvent()
        {
            var activities = (from s in db.Activity
                              where s.Status == "1"
                              select new
                              {
                                  ActivityID = s.ActivityID,
                                  ActivityName = s.ActivityName,
                                  ActivityStartTime = s.ActivityStartTime,
                                  ActivityEndTime = s.ActivityEndTime
                              }).ToList().Select(c => new ActivitiesCalendarViewModel
                              {
                                  ActivityName = c.ActivityName,
                                  ActivityStartTime = c.ActivityStartTime,
                                  ActivityEndTime = c.ActivityEndTime,
                                  Url = "/Activities/ActivityCalendarDetail/" + c.ActivityID
                              });

            JsonResult j = new JsonResult();
            j.Data = activities;
            j.ContentEncoding = Encoding.UTF8;
            j.ContentType = "application/json";
            return j;
        }

        // GET: Activities/ActivityCalendarDetail/ID
        public ActionResult ActivityCalendarDetail(int? id)
        {

            string accountID = Session["AccountID"] == null ? null : Session["AccountID"].ToString();
            if (accountID == null) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Activity activity = db.Activity.Find(id);
            if (activity == null) return HttpNotFound();

            string organizer = "";
            string coOrganizer = "";
            string sponsor = "";
            List<ActivityOrganizer> organizerList = activity.ActivityOrganizer.ToList();
            List<ActivityCoOrganizer> coOrganizerList = activity.ActivityCoOrganizer.ToList();
            List<ActivitySponsor> sponsorList = activity.ActivitySponsor.ToList();

            foreach (ActivityOrganizer a in organizerList)
            {
                organizer += a.Name + ",";
            }
            foreach (ActivityCoOrganizer a in coOrganizerList)
            {
                coOrganizer += a.Name + ",";
            }
            foreach (ActivitySponsor a in sponsorList)
            {
                sponsor += a.Name + ",";
            }
            if (organizer.EndsWith(",")) organizer = organizer.Substring(0, organizer.Length - 1);
            if (coOrganizer.EndsWith(",")) coOrganizer = coOrganizer.Substring(0, coOrganizer.Length - 1);
            if (sponsor.EndsWith(",")) sponsor = sponsor.Substring(0, sponsor.Length - 1);

            int? systemLevel = db.UserAccount.Where(c => c.AccountID == accountID).Select(c => c.SystemLevel).FirstOrDefault();
            var applicant = (from a in db.ActivityApplicant
                             where a.ActivityID == id && a.ApplyID == accountID
                             select new { a.IsCancel });
            string isCancel = null;
            if (applicant.Count() > 0) isCancel = applicant.FirstOrDefault().IsCancel;

            ActivityCalendarDetailViewModel viewModel = new ActivityCalendarDetailViewModel
            {
                ActivityID = activity.ActivityID,
                ActivityName = activity.ActivityName,
                Description = activity.Description,
                ActivityType = activity.ActivityType,
                ApplicantLimit = activity.ApplicantLimit,
                Meal = activity.Meal,
                StudyProof = activity.StudyProof,
                StudyHours = activity.StudyHours,
                ActivityStartTime = activity.ActivityStartTime,
                ActivityEndTime = activity.ActivityEndTime,
                ApplyStartTime = activity.ApplyStartTime,
                ApplyEndTime = activity.ApplyEndTime,
                ActivityOrganizer = organizer,
                ActivityCoOrganizer = coOrganizer,
                ActivitySponsor = sponsor,
                SystemLevel = systemLevel,
                IsCancel = isCancel
            };

            return View(viewModel);
        }

        // POST: Activities/ApplyActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyActivity([Bind(Include = "ActivityID,Meal,NeedMeal,Attend")] ActivityCalendarDetailViewModel activityView)
        {
            string accountID = Session["AccountID"] == null ? null : Session["AccountID"].ToString();
            if (accountID == null) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            if (activityView.Attend.Equals("0")) return RedirectToAction("ActivityCalendar");

            var systemLevel = (from u in db.UserAccount
                               where u.AccountID == accountID
                               select new { systemLevel = u.SystemLevel });
            if (systemLevel.Count() < 1) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            if (systemLevel.FirstOrDefault().systemLevel != null) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            
            if (!activityView.Meal.Equals("1")) activityView.NeedMeal = null;

            var query = (from a in db.ActivityApplicant
                         where a.ActivityID == activityView.ActivityID && a.ApplyID == accountID
                         select new { a });
            
            if (query.Count() > 0)
            {
                ActivityApplicant oldApplicant = new ActivityApplicant();
                oldApplicant = query.FirstOrDefault().a;
                oldApplicant.IsCancel = "0";
                oldApplicant.ActivityApplicantInformation.FirstOrDefault().Meal = activityView.NeedMeal;
                db.Entry(oldApplicant).State = EntityState.Modified;
            }

            else
            {
                Activity activity = db.Activity.Find(activityView.ActivityID);

                string userName = (from k in db.UserAccount
                                   where k.AccountID == accountID
                                   select new { k.UserName }).FirstOrDefault().UserName;
                ActivityApplicantInformation information = new ActivityApplicantInformation() { Name = userName, Meal = activityView.NeedMeal, Phone = "0912345678", Sex = 1 };
                List<ActivityApplicantInformation> informationList = new List<ActivityApplicantInformation>();
                informationList.Add(information);

                ActivityApplicant applicant = new ActivityApplicant() { Activity = activity, ApplyID = accountID, IsCancel = "0" };
                applicant.ActivityApplicantInformation = informationList;
                db.ActivityApplicant.Add(applicant);
            }

            db.SaveChanges();

            return RedirectToAction("ActivityCalendar");
        }

        // POST: Activities/CancelApplyActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelApplyActivity([Bind(Include = "ActivityID,Attend")] ActivityCalendarDetailViewModel activityView)
        {
            string accountID = Session["AccountID"] == null ? null : Session["AccountID"].ToString();
            if (accountID == null) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            if (activityView.Attend.Equals("0")) return RedirectToAction("ActivityCalendar");

            var systemLevel = (from u in db.UserAccount
                               where u.AccountID == accountID
                               select new { systemLevel = u.SystemLevel });
            if (systemLevel.Count() < 1) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            if (systemLevel.FirstOrDefault().systemLevel != null) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            var query = (from a in db.ActivityApplicant
                         where a.ActivityID == activityView.ActivityID && a.ApplyID == accountID
                         select new { a });
            if (query.Count() < 1) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            ActivityApplicant applicant = query.FirstOrDefault().a;
            applicant.IsCancel = "1";

            db.Entry(applicant).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ActivityCalendar");
        }

        // GET: Activities/AddActivity
        public ActionResult AddActivity()
        {
            var activityTypeDetails = (from typeList in db.ActivityType
                                       select new { typeList.ActivityTypeID, typeList.TypeName });
            SelectList activityTypeList = new SelectList(activityTypeDetails, "ActivityTypeID", "TypeName");
            ViewBag.activityTypeList = activityTypeList;
            return View();
        }

        // POST: Activities/AddActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddActivity([Bind(Include = "ActivityName,Description,ActivityType,ApplicantLimit,Meal,StudyProof,StudyHours,ActivityStartTime,ActivityEndTime,ApplyStartTime,ApplyEndTime,ActivityOrganizer,ActivityCoOrganizer,ActivitySponsor")] ActivitiesViewModel activityView)
        {
            if (ModelState.IsValid)
            {
                Activity activity = new Activity()
                {
                    ActivityName = activityView.ActivityName,
                    Description = activityView.Description,
                    ActivityType = activityView.ActivityType,
                    ApplicantLimit = activityView.ApplicantLimit,
                    Meal = activityView.Meal,
                    StudyProof = activityView.StudyProof,
                    StudyHours = activityView.StudyHours,
                    ActivityStartTime = activityView.ActivityStartTime,
                    ActivityEndTime = activityView.ActivityEndTime,
                    ApplyStartTime = activityView.ApplyStartTime,
                    ApplyEndTime = activityView.ApplyEndTime
                };

                if (!string.IsNullOrWhiteSpace(activityView.ActivityOrganizer))
                {
                    List<ActivityOrganizer> organizerList = new List<ActivityOrganizer>();
                    string[] organizer = StringUtility.SplitAndTrim(activityView.ActivityOrganizer, ',');
                    foreach (string s in organizer)
                    {
                        ActivityOrganizer ao = new ActivityOrganizer { Name = s };
                        organizerList.Add(ao);
                    }
                    activity.ActivityOrganizer = organizerList;
                }

                if (!string.IsNullOrWhiteSpace(activityView.ActivityCoOrganizer))
                {
                    List<ActivityCoOrganizer> coOrganizerList = new List<ActivityCoOrganizer>();
                    string[] coOrganizer = StringUtility.SplitAndTrim(activityView.ActivityCoOrganizer, ',');
                    foreach (string s in coOrganizer)
                    {
                        ActivityCoOrganizer ao = new ActivityCoOrganizer { Name = s };
                        coOrganizerList.Add(ao);
                    }
                    activity.ActivityCoOrganizer = coOrganizerList;
                }

                if (!string.IsNullOrWhiteSpace(activityView.ActivitySponsor))
                {
                    List<ActivitySponsor> sponsorList = new List<ActivitySponsor>();
                    string[] sponsor = StringUtility.SplitAndTrim(activityView.ActivitySponsor, ',');
                    foreach (string s in sponsor)
                    {
                        ActivitySponsor ao = new ActivitySponsor { Name = s };
                        sponsorList.Add(ao);
                    }
                    activity.ActivitySponsor = sponsorList;
                }

                string accountID = Session["AccountID"] == null ? null : Session["AccountID"].ToString();
                DateTime time = DateTime.Now;
                activity.CreateID = accountID;
                activity.ModifyID = accountID;
                activity.CreateTime = time;
                activity.ModifyTime = time;
                activity.Status = "0";//0 申請中 1 申請通過 2 申請失敗
                db.Activity.Add(activity);
                db.SaveChanges();
                return RedirectToAction("ActivityManagement");
            }

            return View(activityView);
        }

        // GET: Activities/CheckActivity/id
        public ActionResult CheckActivity(int? id)
        {
            string accountID = Session["AccountID"] == null ? null : Session["AccountID"].ToString();
            if (accountID == null) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activity.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            string organizer = "";
            string coOrganizer = "";
            string sponsor = "";
            List<ActivityOrganizer> organizerList = activity.ActivityOrganizer.ToList();
            List<ActivityCoOrganizer> coOrganizerList = activity.ActivityCoOrganizer.ToList();
            List<ActivitySponsor> sponsorList = activity.ActivitySponsor.ToList();

            foreach (ActivityOrganizer a in organizerList)
            {
                organizer += a.Name + ",";
            }
            foreach (ActivityCoOrganizer a in coOrganizerList)
            {
                coOrganizer += a.Name + ",";
            }
            foreach (ActivitySponsor a in sponsorList)
            {
                sponsor += a.Name + ",";
            }
            if (organizer.EndsWith(",")) organizer = organizer.Substring(0, organizer.Length - 1);
            if (coOrganizer.EndsWith(",")) coOrganizer = coOrganizer.Substring(0, coOrganizer.Length - 1);
            if (sponsor.EndsWith(",")) sponsor = sponsor.Substring(0, sponsor.Length - 1);

            string creator = db.UserAccount.Where(c => c.AccountID == activity.CreateID).Select(c => c.UserName).FirstOrDefault();

            int? systemLevel = db.UserAccount.Where(c => c.AccountID == accountID).Select(c => c.SystemLevel).FirstOrDefault();

            ActivitiesCheckViewModel viewModel = new ActivitiesCheckViewModel
            {
                ActivityID = activity.ActivityID,
                ActivityName = activity.ActivityName,
                Description = activity.Description,
                ActivityType = activity.ActivityType,
                ApplicantLimit = activity.ApplicantLimit,
                Meal = activity.Meal,
                StudyProof = activity.StudyProof,
                StudyHours = activity.StudyHours,
                ActivityStartTime = activity.ActivityStartTime,
                ActivityEndTime = activity.ActivityEndTime,
                ApplyStartTime = activity.ApplyStartTime,
                ApplyEndTime = activity.ApplyEndTime,
                ActivityOrganizer = organizer,
                ActivityCoOrganizer = coOrganizer,
                ActivitySponsor = sponsor,
                Creator = creator,
                CreateTime = activity.CreateTime,
                Status = activity.Status,
                SystemLevel = systemLevel
            };

            if (systemLevel == null) ViewBag.contentTitle = "活動內容";
            else ViewBag.contentTitle = "審核活動";
            return View(viewModel);
        }

        // POST: Activities/CheckActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckActivity([Bind(Include = "ActivityID,Status")] ActivitiesCheckViewModel activitiesCheckView)
        {
            string accountID = Session["AccountID"] == null ? null : Session["AccountID"].ToString();
            DateTime time = DateTime.Now;

            Activity activity = db.Activity.Find(activitiesCheckView.ActivityID);
            if (activity == null)
            {
                return HttpNotFound();
            }

            activity.Status = activitiesCheckView.Status;
            activity.ModifyID = accountID;
            activity.ModifyTime = time;
            db.Entry(activity).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ActivityManagement");
        }

        // GET: Activities/AddActivityType
        public ActionResult AddActivityType()
        {
            return View();
        }

        // POST: Activities/AddActivityType        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddActivityType([Bind(Include = "TypeName,Description")] ActivitiesTypeViewModel activityTypeView)
        {
            if (ModelState.IsValid)
            {
                string accountID = Session["AccountID"] == null ? null : Session["AccountID"].ToString();
                DateTime time = DateTime.Now;
                ActivityType activityType = new ActivityType
                {
                    TypeName = activityTypeView.TypeName,
                    Description = activityTypeView.Description,
                    CreateID = accountID,
                    ModifyID = accountID,
                    CreateTime = time,
                    ModifyTime = time
                };

                db.ActivityType.Add(activityType);
                db.SaveChanges();
                return RedirectToAction("ActivityManagement");
            }
            return View(activityTypeView);
        }

        /*
        // GET: Activities/Edit/5
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

        // POST: Activities/Edit/id
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

        // GET: Activities/Delete/5
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

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activity.Find(id);
            db.Activity.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
