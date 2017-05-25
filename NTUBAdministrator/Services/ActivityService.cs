using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NTUBAdministrator.Models;
using NTUBAdministrator.Models.Interface;
using NTUBAdministrator.Models.Repository;
using NTUBAdministrator.Services.Interface;
using NTUBAdministrator.Utils;
using NTUBAdministrator.ViewModels;

namespace NTUBAdministrator.Services
{
    public class ActivityService : IActivityService
    {
        private IUserAccountRepository userAccountRepository = new UserAccountRepository();
        private IActivityRepository activityRepository = new ActivityRepository();
        private IActivityTypeRepository activityTypeRepository = new ActivityTypeRepository();
        private IActivityApplicantRepository activityApplicantRepository = new ActivityApplicantRepository();

        public JsonResult GetActivityCalendarEvent()
        {
            JsonResult json = new JsonResult()
            {
                Data = activityRepository.GetActivityCalendarEvent(),
                ContentEncoding = Encoding.UTF8,
                ContentType = "application/json"
            };
            return json;
        }

        public ActivityCalendarDetailViewModel GetActivityCalendarDetail(string accountID, int id)
        {
            Activity activity = activityRepository.GetActivityByID(id);
            if (activity == null) return null;

            string organizer = "";
            string coOrganizer = "";
            string sponsor = "";

            foreach (ActivityOrganizer a in activity.ActivityOrganizer) organizer += a.Name + ",";
            foreach (ActivityCoOrganizer a in activity.ActivityCoOrganizer) coOrganizer += a.Name + ",";
            foreach (ActivitySponsor a in activity.ActivitySponsor) sponsor += a.Name + ",";

            if (organizer.EndsWith(",")) organizer = organizer.Substring(0, organizer.Length - 1);
            if (coOrganizer.EndsWith(",")) coOrganizer = coOrganizer.Substring(0, coOrganizer.Length - 1);
            if (sponsor.EndsWith(",")) sponsor = sponsor.Substring(0, sponsor.Length - 1);

            UserAccount user = userAccountRepository.getUserAccountByID(accountID);
            int? systemLevel = user.SystemLevel;

            ActivityApplicant applicant = activityApplicantRepository.GetActivityApplicant(accountID, id);

            string isCancel = null;
            if (applicant != null) isCancel = applicant.IsCancel;

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
            return viewModel;
        }

        public IResult ApplyActivity(string accountID, ActivityCalendarDetailViewModel viewModel)
        {
            IResult result = new Result();
            try
            {
                UserAccount user = userAccountRepository.getUserAccountByID(accountID);
                int? systemLevel = user.SystemLevel;

                if (user == null || systemLevel != null) return result;

                if (!viewModel.Meal.Equals("1")) viewModel.NeedMeal = null;

                ActivityApplicant applicant = activityApplicantRepository.GetActivityApplicant(accountID, viewModel.ActivityID);
                if (applicant != null)
                {
                    applicant.IsCancel = "0";
                    applicant.ActivityApplicantInformation.FirstOrDefault().Meal = viewModel.NeedMeal;
                    activityApplicantRepository.Update(applicant);
                }
                else
                {
                    Activity activity = activityRepository.GetActivityByID(viewModel.ActivityID);
                    ActivityApplicantInformation information = new ActivityApplicantInformation() { Name = user.UserName, Meal = viewModel.NeedMeal, Phone = "0912345678", Sex = 1 };
                    List<ActivityApplicantInformation> informationList = new List<ActivityApplicantInformation>();
                    informationList.Add(information);

                    applicant = new ActivityApplicant() { ActivityID = activity.ActivityID, ApplyID = accountID, IsCancel = "0", ActivityApplicantInformation = informationList };
                    activityApplicantRepository.Insert(applicant);
                }
                result.Success = true;
            }
            catch (Exception e) { result.Exception = e; }
            return result;
        }

        public IResult CancelApplyActivity(string accountID, ActivityCalendarDetailViewModel viewModel)
        {
            IResult result = new Result();
            try
            {
                UserAccount user = userAccountRepository.getUserAccountByID(accountID);
                int? systemLevel = user.SystemLevel;

                if (user == null || systemLevel != null) return result;

                ActivityApplicant applicant = activityApplicantRepository.GetActivityApplicant(accountID, viewModel.ActivityID);
                if (applicant == null) return result;
                applicant.IsCancel = "1";
                activityApplicantRepository.Update(applicant);
                result.Success = true;
            }
            catch (Exception e) { result.Exception = e; }
            return result;
        }

        public IEnumerable<ActivityManagementViewModel> GetActivityManagement(string accountID)
        {
            UserAccount user = userAccountRepository.getUserAccountByID(accountID);
            int? systemLevel = user.SystemLevel;

            if (systemLevel == null)//學生 教職員
            {
                return activityRepository.GetActivityManagementViewByCreateID(accountID);
            }
            else//管理者
            {
                return activityRepository.GetAllActivityManagementView();
            }
        }

        public IResult AddActivityType(string accountID, ActivityTypeViewModel viewModel)
        {
            IResult result = new Result();
            try
            {
                DateTime time = DateTime.Now;
                ActivityType activityType = new ActivityType
                {
                    TypeName = viewModel.TypeName,
                    Description = viewModel.Description,
                    CreateID = accountID,
                    ModifyID = accountID,
                    CreateTime = time,
                    ModifyTime = time
                };
                activityTypeRepository.Insert(activityType);
                result.Success = true;
            }
            catch (Exception e) { result.Exception = e; }
            return result;
        }

        public IResult AddActivity(string accountID, ActivityViewModel viewModel)
        {
            IResult result = new Result();
            try
            {
                Activity activity = new Activity()
                {
                    ActivityName = viewModel.ActivityName,
                    Description = viewModel.Description,
                    ActivityType = viewModel.ActivityType,
                    ApplicantLimit = viewModel.ApplicantLimit,
                    Meal = viewModel.Meal,
                    StudyProof = viewModel.StudyProof,
                    StudyHours = viewModel.StudyHours,
                    ActivityStartTime = viewModel.ActivityStartTime,
                    ActivityEndTime = viewModel.ActivityEndTime,
                    ApplyStartTime = viewModel.ApplyStartTime,
                    ApplyEndTime = viewModel.ApplyEndTime
                };

                if (!string.IsNullOrWhiteSpace(viewModel.ActivityOrganizer))
                {
                    List<ActivityOrganizer> organizerList = new List<ActivityOrganizer>();
                    string[] organizer = StringUtility.SplitAndTrim(viewModel.ActivityOrganizer, ',');
                    foreach (string s in organizer)
                    {
                        ActivityOrganizer ao = new ActivityOrganizer { Name = s };
                        organizerList.Add(ao);
                    }
                    activity.ActivityOrganizer = organizerList;
                }

                if (!string.IsNullOrWhiteSpace(viewModel.ActivityCoOrganizer))
                {
                    List<ActivityCoOrganizer> coOrganizerList = new List<ActivityCoOrganizer>();
                    string[] coOrganizer = StringUtility.SplitAndTrim(viewModel.ActivityCoOrganizer, ',');
                    foreach (string s in coOrganizer)
                    {
                        ActivityCoOrganizer ao = new ActivityCoOrganizer { Name = s };
                        coOrganizerList.Add(ao);
                    }
                    activity.ActivityCoOrganizer = coOrganizerList;
                }

                if (!string.IsNullOrWhiteSpace(viewModel.ActivitySponsor))
                {
                    List<ActivitySponsor> sponsorList = new List<ActivitySponsor>();
                    string[] sponsor = StringUtility.SplitAndTrim(viewModel.ActivitySponsor, ',');
                    foreach (string s in sponsor)
                    {
                        ActivitySponsor ao = new ActivitySponsor { Name = s };
                        sponsorList.Add(ao);
                    }
                    activity.ActivitySponsor = sponsorList;
                }

                DateTime time = DateTime.Now;
                activity.CreateID = accountID;
                activity.ModifyID = accountID;
                activity.CreateTime = time;
                activity.ModifyTime = time;
                activity.Status = "0";//0 申請中 1 申請通過 2 申請失敗
                activityRepository.Insert(activity);
                result.Success = true;
            }
            catch (Exception e) { result.Exception = e; }
            return result;
        }

        public IQueryable GetActivityTypeList()
        {
            return activityTypeRepository.GetActivityTypeList();
        }

        public ActivityCheckViewModel GetActivityCheckView(string accountID, int id)
        {
            Activity activity = activityRepository.GetActivityByID(id);
            if (activity == null) return null;

            string organizer = "";
            string coOrganizer = "";
            string sponsor = "";

            foreach (ActivityOrganizer a in activity.ActivityOrganizer) organizer += a.Name + ",";
            foreach (ActivityCoOrganizer a in activity.ActivityCoOrganizer) coOrganizer += a.Name + ",";
            foreach (ActivitySponsor a in activity.ActivitySponsor) sponsor += a.Name + ",";

            if (organizer.EndsWith(",")) organizer = organizer.Substring(0, organizer.Length - 1);
            if (coOrganizer.EndsWith(",")) coOrganizer = coOrganizer.Substring(0, coOrganizer.Length - 1);
            if (sponsor.EndsWith(",")) sponsor = sponsor.Substring(0, sponsor.Length - 1);

            UserAccount user = userAccountRepository.getUserAccountByID(accountID);
            string creator = user.UserName;
            int? systemLevel = user.SystemLevel;

            ActivityCheckViewModel viewModel = new ActivityCheckViewModel
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
            return viewModel;
        }

        public IResult CheckActivity(string accountID, ActivityCheckViewModel viewModel)
        {
            IResult result = new Result();
            try
            {
                DateTime time = DateTime.Now;

                Activity activity = activityRepository.GetActivityByID(viewModel.ActivityID);
                if (activity == null) return null;

                activity.Status = viewModel.Status;
                activity.ModifyID = accountID;
                activity.ModifyTime = time;
                activityRepository.Update(activity);
                result.Success = true;
            }
            catch (Exception e) { result.Exception = e; }
            return result;
        }
    }
}