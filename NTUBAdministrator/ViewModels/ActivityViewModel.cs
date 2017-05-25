using System;
using System.ComponentModel.DataAnnotations;

namespace NTUBAdministrator.ViewModels
{
    public class ActivityManagementViewModel
    {
        [Required]
        [Display(Name = "活動ID")]
        public int ActivityID { get; set; }

        [Required]
        [Display(Name = "活動名稱")]
        public string ActivityName { get; set; }

        [Required]
        [Display(Name = "活動狀態")]
        public string Status { get; set; }
    }

    public class ActivityCalendarViewModel
    {
        public string ActivityName { get; set; }
        public DateTime? ActivityStartTime { get; set; }
        public DateTime? ActivityEndTime { get; set; }
        public string Url { get; set; }
    }

    public class ActivityCalendarDetailViewModel
    {
        [Display(Name = "活動ID")]
        public int ActivityID { get; set; }

        [Display(Name = "活動名稱")]
        public string ActivityName { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "活動敘述")]
        public string Description { get; set; }

        [Display(Name = "活動種類")]
        public short ActivityType { get; set; }

        [Display(Name = "人數限制")]
        public int? ApplicantLimit { get; set; }

        [Display(Name = "是否供餐")]
        public string Meal { get; set; }

        [Display(Name = "是否提供研習證明")]
        public string StudyProof { get; set; }

        [Display(Name = "研習時數")]
        public double? StudyHours { get; set; }

        [Display(Name = "活動開始時間")]
        [DataType(DataType.DateTime)]
        public DateTime? ActivityStartTime { get; set; }

        [Display(Name = "活動結束時間")]
        [DataType(DataType.DateTime)]
        public DateTime? ActivityEndTime { get; set; }

        [Display(Name = "活動報名開始時間")]
        [DataType(DataType.DateTime)]
        public DateTime? ApplyStartTime { get; set; }

        [Display(Name = "活動報名結束時間")]
        [DataType(DataType.DateTime)]
        public DateTime? ApplyEndTime { get; set; }

        [Display(Name = "主辦單位")]
        [DataType(DataType.MultilineText)]
        public string ActivityOrganizer { get; set; }

        [Display(Name = "協辦單位")]
        [DataType(DataType.MultilineText)]
        public string ActivityCoOrganizer { get; set; }

        [Display(Name = "贊助單位")]
        [DataType(DataType.MultilineText)]
        public string ActivitySponsor { get; set; }
       
        public int? SystemLevel { get; set; }

        [Display(Name = "是否需要餐點")]
        public string NeedMeal { get; set; }

        public string Attend { get; set; }

        public string IsCancel { get; set; }

    }

    public class ActivityCheckViewModel
    {
        [Display(Name = "活動ID")]
        public int ActivityID { get; set; }

        [Display(Name = "活動名稱")]
        public string ActivityName { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "活動敘述")]
        public string Description { get; set; }

        [Display(Name = "活動種類")]
        public short ActivityType { get; set; }

        [Display(Name = "人數限制")]
        public int? ApplicantLimit { get; set; }

        [Display(Name = "是否供餐")]
        public string Meal { get; set; }

        [Display(Name = "是否提供研習證明")]
        public string StudyProof { get; set; }

        [Display(Name = "研習時數")]
        public double? StudyHours { get; set; }

        [Display(Name = "申請狀態")]
        public string Status { get; set; }

        [Display(Name = "活動開始時間")]
        [DataType(DataType.DateTime)]
        public DateTime? ActivityStartTime { get; set; }

        [Display(Name = "活動結束時間")]
        [DataType(DataType.DateTime)]
        public DateTime? ActivityEndTime { get; set; }

        [Display(Name = "活動報名開始時間")]
        [DataType(DataType.DateTime)]
        public DateTime? ApplyStartTime { get; set; }

        [Display(Name = "活動報名結束時間")]
        [DataType(DataType.DateTime)]
        public DateTime? ApplyEndTime { get; set; }

        [Display(Name = "主辦單位")]
        [DataType(DataType.MultilineText)]
        public string ActivityOrganizer { get; set; }

        [Display(Name = "協辦單位")]
        [DataType(DataType.MultilineText)]
        public string ActivityCoOrganizer { get; set; }

        [Display(Name = "贊助單位")]
        [DataType(DataType.MultilineText)]
        public string ActivitySponsor { get; set; }

        [Display(Name = "建立人")]
        public string Creator { get; set; }

        [Display(Name = "建立時間")]
        [DataType(DataType.DateTime)]
        public DateTime? CreateTime { get; set; }

        public int? SystemLevel { get; set; }

    }

    public class ActivityViewModel
    {
        [Required]
        [Display(Name = "活動名稱")]
        public string ActivityName { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "活動敘述")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "活動種類")]
        public short ActivityType { get; set; }

        [Display(Name = "人數限制")]
        public int? ApplicantLimit { get; set; }

        [Display(Name = "是否供餐")]
        public string Meal { get; set; }

        [Display(Name = "是否提供研習證明")]
        public string StudyProof { get; set; }

        [Display(Name = "研習時數")]
        public double? StudyHours { get; set; }

        [Display(Name = "活動開始時間")]
        [DataType(DataType.DateTime)]
        public DateTime? ActivityStartTime { get; set; }

        [Display(Name = "活動結束時間")]
        [DataType(DataType.DateTime)]
        public DateTime? ActivityEndTime { get; set; }

        [Display(Name = "活動報名開始時間")]
        [DataType(DataType.DateTime)]
        public DateTime? ApplyStartTime { get; set; }

        [Display(Name = "活動報名結束時間")]
        [DataType(DataType.DateTime)]
        public DateTime? ApplyEndTime { get; set; }

        [Display(Name = "主辦單位")]
        [DataType(DataType.MultilineText)]
        public string ActivityOrganizer { get; set; }

        [Display(Name = "協辦單位")]
        [DataType(DataType.MultilineText)]
        public string ActivityCoOrganizer { get; set; }

        [Display(Name = "贊助單位")]
        [DataType(DataType.MultilineText)]
        public string ActivitySponsor { get; set; }
    }

    public class ActivityTypeViewModel
    {

        public short ActivityTypeID { get; set; }

        [Required]
        [Display(Name = "活動種類")]
        public string TypeName { get; set; }

        [Required]
        [Display(Name = ("敘述"))]
        public string Description { get; set; }
    }
}