// <com.woyouqiu.Copyright>
// --------------------------------------------------------------
// <copyright>上海有求网络科技有限公司 2015</copyright>
// <Solution>Abp.Web.Api.SwaggerTool</Solution>
// <Project>Abp.Web.Api.SwaggerTool</Project>
// <FileName>ILog.cs</FileName>
// <CreateTime>2016-12-16 18:06</CreateTime>
// <Author>何苗</Author>
// <Email>hemiao@woyouqiu.com</Email>
// <log date="2016-12-16 18:06" version="00001">创建</log>
// --------------------------------------------------------------
// </com.woyouqiu.Copyright>

using System;

namespace Abp.Web.Api.SwaggerTool.Logging {
    internal interface ILog
    {
        bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters);
    }
}