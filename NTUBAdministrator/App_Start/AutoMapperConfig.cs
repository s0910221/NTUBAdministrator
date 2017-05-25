using AutoMapper;
using NTUBAdministrator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//沒用到
namespace NTUBAdministrator.App_Start
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile<ActivityProfile>();
            });
        }
    }
}