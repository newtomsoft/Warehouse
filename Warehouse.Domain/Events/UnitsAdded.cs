﻿using System;
using Warehouse.Domain.Events.Base;

namespace Warehouse.Domain.Events
{
    internal class UnitsAdded : Event
    {
        public UnitsAdded(Guid id, uint units)
            : base(id)
        {
            this.Units = units;
        }

        public uint Units { get; }
    }
}
