﻿using Warehouse.Domain.Commands.Base;
using Warehouse.Domain.Domain;
using Warehouse.Domain.Events.Base;

namespace Warehouse.Domain.Commands.RenameItem
{
    internal class RenameItemHandler : ICommandHandler<RenameItemCommand>
    {
        private readonly IEventStore eventStore;

        public RenameItemHandler(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public void Handle(RenameItemCommand command)
        {
            var item = new Item(this.eventStore.GetEventsById(command.ItemId.Value));
            item.Rename(command.NewName);
            this.eventStore.Save(item.UncommitedEvents);
        }
    }
}