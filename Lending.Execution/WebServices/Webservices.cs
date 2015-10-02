﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using Lending.Core.ConnectionRequest;
using Lending.Core.Model;
using Lending.Execution.UnitOfWork;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Lending.Execution.WebServices
{
    public class ConnectWebservice : WebserviceBase<ConnectionRequest, BaseResponse>, IWebserviceBase<ConnectionRequest, BaseResponse>
    {
        public ConnectWebservice(IUnitOfWork unitOfWork,
            IRequestHandler<ConnectionRequest, BaseResponse> requestHandler)
            : base(unitOfWork, requestHandler)
        { }

    }


}
