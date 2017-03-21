using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http.Description;

namespace Abp.Web.Api.SwaggerTool
{
    public class ApplyDocumentVendorExtensions : IDocumentFilter
    {
        private string hackcontrollername(string name) {
            if (name.Contains('/'))
            {
                return name.Replace("/", "_");
            }
            else {
                return name;
            }
        }
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            //添加Tag
            swaggerDoc.tags = new List<Tag>();
            var controllers = apiExplorer.ApiDescriptions.Select(p => p.ActionDescriptor.ControllerDescriptor).Distinct();
            foreach (var item in controllers)
            {
                var desc = item.GetCustomAttributes<DisplayNameAttribute>();
                if (desc != null && desc.Count > 0)
                {
                    //hack
                    swaggerDoc.tags.Add(new Tag() { name = hackcontrollername(item.ControllerName), description = desc[0].DisplayName });
                }
                else
                {
                    var desc2 = item.GetCustomAttributes<DescriptionAttribute>();
                    if (desc2 != null && desc2.Count > 0)
                    {
                        swaggerDoc.tags.Add(new Tag() { name = hackcontrollername(item.ControllerName), description = desc2[0].Description });
                    }
                }

                
            }


            //优化枚举显示
            foreach (var schemaDictionaryItem in swaggerDoc.definitions)
            {
                var schema = schemaDictionaryItem.Value;
                foreach (var propertyDictionaryItem in schema.properties)
                {
                    var property = propertyDictionaryItem.Value;
                    var propertyEnums = property.@enum;
                    if (propertyEnums != null && propertyEnums.Count > 0)
                    {
                        var enumDescriptions = new List<string>();
                        for (int i = 0; i < propertyEnums.Count; i++)
                        {
                            var enumOption = propertyEnums[i];
                            var desc =(DisplayAttribute) enumOption.GetType().GetField(enumOption.ToString()).GetCustomAttributes(true).Where(p => p is DisplayAttribute).FirstOrDefault();
                            if (desc==null)
                            {
                                enumDescriptions.Add(string.Format("{0} = {1} ", Convert.ToInt32(enumOption), Enum.GetName(enumOption.GetType(), enumOption)));
                            }
                            else
                            {
                                enumDescriptions.Add(string.Format("{0} = {1} ", Convert.ToInt32(enumOption), desc.Name));
                            }
                            
                        }
                        property.description += string.Format(" ({0})", string.Join(", ", enumDescriptions.ToArray()));
                    }
                }
            }
        }
    }
}
