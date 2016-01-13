﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Execution.Auth;
using Microsoft.Owin.Extensions;
using Owin;
using Owin.StatelessAuth;

namespace Lending.Execution.Owin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var pathsToIgnore = new[]
            {
                "/",
                "/App/*.*",
                "/App/",
                "/authentication/*.*",
                "/authentication/**",
                "/authentication",
                "/content",
                "/content/*.*",
                "/fonts/*.*",
                "/scripts/*.js",
            };

            app.RequiresStatelessAuth(new SecureTokenValidator(), new StatelessAuthOptions
            {
                IgnorePaths = pathsToIgnore,
            });
            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}