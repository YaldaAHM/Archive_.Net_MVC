using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model.ReportTitle;
using YA.Archive.Service.ReportTitleMdl.VMs;

namespace YA.Archive.Service.ReportTitleMdl.Mapping
{
    public static class ReportTitleMapper
    { 
     public static ReportTitleModel ToModel(this ReportTitleVM ReportTitle)
    {
        return Mapper.Map<ReportTitleVM, ReportTitleModel>(ReportTitle);
    }
    public static ReportTitleVM ToViewModel(this ReportTitleModel ReportTitle)
    {
        return Mapper.Map<ReportTitleModel, ReportTitleVM>(ReportTitle);
    }
    public static IEnumerable<ReportTitleModel> ToModelList(this IEnumerable<ReportTitleVM> ReportTitles)
    {
        return Mapper.Map<IEnumerable<ReportTitleVM>, List<ReportTitleModel>>(ReportTitles);
    }
    public static IEnumerable<ReportTitleVM> ToViewModelList(this IEnumerable<ReportTitleModel> ReportTitle)
    {
        return Mapper.Map<IEnumerable<ReportTitleModel>, List<ReportTitleVM>>(ReportTitle);
    }
       
    }
}
