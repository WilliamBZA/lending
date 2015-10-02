﻿using System;
using System.Collections.Generic;

namespace Lending.Domain
{
    public interface IRepository
    {
        TAggregate GetById<TAggregate>(Guid id) where TAggregate : Aggregate;
        TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : Aggregate;
        void Save(Aggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
        void EmitEvent(string stream, Event @event);
    }
}