﻿using System;
using Warehouse.Domain.Events.Base;

namespace Warehouse.Domain.Events
{
    internal class ItemDisabled : Event
    {
        public ItemDisabled(Guid itemId)
            : base(itemId)
        {
        }
    }
}
