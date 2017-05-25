using AutoMapper;
using NTUBAdministrator.Models;
using NTUBAdministrator.ViewModels;
//沒用到
namespace NTUBAdministrator.Utils
{
    public class ActivityProfile : Profile
    {
        public override string ProfileName
        {
            get { return "Test"; }
        }

        public ActivityProfile()
        {
            CreateMap<Activity, ActivityViewModel>();
            // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
        }

    }
}