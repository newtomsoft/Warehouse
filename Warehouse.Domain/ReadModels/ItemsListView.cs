﻿using System.Collections.Generic;
using Warehouse.Domain.ReadModels.Repositories;

namespace Warehouse.Domain.ReadModels
{
    public class ItemsListView
    {
        public ItemsListView()
            : this(Bootstrapper.ReadModelRepository)
        { 
        }

        internal ItemsListView(IReadModelReadOnlyRepository readOnlyModelRepository)
        {
            this.Items = readOnlyModelRepository.Get<ItemView>();
        }

        public IEnumerable<ItemView> Items { get; }
    }
}
