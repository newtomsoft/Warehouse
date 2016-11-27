﻿using System;
using System.Collections.Generic;
using Warehouse.Domain.Events;
using Warehouse.Domain.Events.Base;

namespace Warehouse.Domain.Domain
{
    public class Item : Aggregate
    {
        public Item(IEnumerable<Event> events) 
            : base(events)
        {
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public uint Units { get; private set; }

        protected override IReadOnlyDictionary<Type, Action<Event>> GetAggregatedEvents()
        {
            return new Dictionary<Type, Action<Event>>
            {
                { typeof(ItemCreated), e => this.ApplyItemCreated((ItemCreated)e) },
                { typeof(ItemRenamed), e => this.ApplyItemRenamed((ItemRenamed)e) },
                { typeof(UnitsAdded), e => this.ApplyUnitsAdded((UnitsAdded)e) }
            };
        }

        private void ApplyItemCreated(ItemCreated itemCreated)
        {
            this.Id = itemCreated.Id;
            this.Name = itemCreated.Name;
        }

        private void ApplyItemRenamed(ItemRenamed itemRenamed)
        {
            this.Name = itemRenamed.NewName;
        }

        private void ApplyUnitsAdded(UnitsAdded unitsAdded)
        {
            this.Units += unitsAdded.Units;
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                throw new DomainException("You can't set an empty name to an item.");
            }

            this.Name = newName;
            this.UncommitedEventsList.Add(new ItemRenamed(this.Id, newName));
        }

        public void AddUnits(uint units)
        {
            this.Units += units;
            this.UncommitedEventsList.Add(new UnitsAdded(this.Id, units));
        }
    }
}