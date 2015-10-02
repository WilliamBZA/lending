﻿using System;
using Lending.Domain;
using NHibernate;

namespace Lending.Execution.Auth
{
    public abstract class BaseAuthenticatedRequestHandler<TRequest, TResponse> : IAuthenticatedRequestHandler<TRequest, TResponse>
    {
        private readonly Func<ISession> getSession;

        protected BaseAuthenticatedRequestHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected BaseAuthenticatedRequestHandler() { }

        protected ISession Session
        {
            get { return getSession(); }
        }

        public abstract TResponse HandleRequest(TRequest request, int userId);
    }
}