using System;
using System.Reflection;

namespace Abp.Web.Api.SwaggerTool.SampleSite.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}