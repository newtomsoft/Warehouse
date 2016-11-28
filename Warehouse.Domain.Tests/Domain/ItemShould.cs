﻿using System;
using System.Linq;
using Moq;
using NFluent;
using NUnit.Framework;
using Warehouse.Domain.Domain;
using Warehouse.Domain.Events;
using Warehouse.Domain.Events.Base;
using Warehouse.Domain.Tests.Events;

namespace Warehouse.Domain.Tests.Domain
{
    [TestFixture]
    public class ItemShould
    {
        [Test]
        public void HaveCorrectIdAndNameWhenProvideItemCreated()
        {
            var itemCreated = new ItemCreated("item name");
            var item = new Item(new[] { itemCreated });

            Check.That(item.Id).Equals(itemCreated.Id);
            Check.That(item.Name).Equals("item name");
        }

        [Test]
        public void BeEnableWhenProvideItemCreated()
        {

            var itemCreated = new ItemCreated("item name");
            var item = new Item(new[] { itemCreated });

            Check.That(item.IsEnable).IsTrue();
        }

        [Test]
        public void HaveCorrectIdAndNameWhenProvideItemCreatedAndItemRenamed()
        {
            var itemCreated = new ItemCreated("item name");

            var events = new Event[] {itemCreated, new ItemRenamed(It.IsAny<Guid>(), "new item name") };
            var item = new Item(events);

            Check.That(item.Id).Equals(itemCreated.Id);
            Check.That(item.Name).Equals("new item name");
        }

        [Test]
        public void BeDisableWhenProvideItemCreatedAndItemDiabled()
        {
            var itemCreated = new ItemCreated("item name");

            var events = new Event[] { itemCreated, new ItemDisabled(itemCreated.Id) };
            var item = new Item(events);

            Check.That(item.IsEnable).IsFalse();
        }

        [Test]
        public void IgnoreUnexpectedEvents()
        {
            var itemCreated = new ItemCreated("item name");
            var events = new Event[] { itemCreated, new ItemRenamed(It.IsAny<Guid>(), "new item name"), new Event1Fake() };
            Check.ThatCode(() => new Item(events)).DoesNotThrow();
        }

        [Test]
        public void AddToUncommitedEventsItemRenamedWhenRename()
        {
            var itemCreated = new ItemCreated("item name");
            var item = new Item(new[] { itemCreated });

            item.Rename("new name");

            Check.That(item.Name).Equals("new name");
            Check.That(item.UncommitedEvents.Single()).HasFieldsWithSameValues(new ItemRenamed(itemCreated.Id, "new name"));
        }

        [Test]
        [TestCase("")]
        [TestCase((string)null)]
        public void ThrowsDomainExceptionWhenRenameWithAnName(string newName)
        {
            var itemCreated = new ItemCreated("item name");
            var item = new Item(new[] { itemCreated });

            Check.ThatCode(() => item.Rename(newName)).Throws<DomainException>();
        }

        [Test]
        public void AddToUncommitedEventsItemDisabledWhenDiasble()
        {
            var itemCreated = new ItemCreated("item name");
            var item = new Item(new[] {itemCreated});

            item.Disable();

            Check.That(item.UncommitedEvents.Single()).HasFieldsWithSameValues(new ItemDisabled(itemCreated.Id));
        }

        [Test]
        public void HaveCorrectUnitsQuantityWhenProvideUnitsAdded()
        {
            var itemCreated = new ItemCreated("item name");

            var events = new Event[] { itemCreated, new UnitsAdded(It.IsAny<Guid>(), 6), new UnitsAdded(It.IsAny<Guid>(), 3) };
            var item = new Item(events);

            Check.That(item.Id).Equals(itemCreated.Id);
            Check.That(item.Units).Equals((uint)9);
        }

        [Test]
        public void AddToUncommitedEventsUnitsAddedWhenAddUnits()
        {
            const uint units = 6;
            var itemCreated = new ItemCreated("item name");
            var item = new Item(new Event[] { itemCreated, new UnitsAdded(itemCreated.Id, 3) });

            item.AddUnits(units);

            Check.That(item.Units).Equals((uint)9);
            Check.That(item.UncommitedEvents.Single()).HasFieldsWithSameValues(new UnitsAdded(itemCreated.Id, units));
        }

        [Test]
        public void HaveCorrectUnitsQuantityWhenProvideUnitsRemoved()
        {
            var itemCreated = new ItemCreated("item name");

            var events = new Event[] { itemCreated, new UnitsAdded(It.IsAny<Guid>(), 6), new UnitsRemoved(It.IsAny<Guid>(), 2) };
            var item = new Item(events);

            Check.That(item.Id).Equals(itemCreated.Id);
            Check.That(item.Units).Equals((uint)4);
        }

        [Test]
        public void AddToUncommitedEventsUnitsRemovedWhenAddUnits()
        {
            const uint units = 3;
            var itemCreated = new ItemCreated("item name");
            var item = new Item(new Event[] { itemCreated, new UnitsAdded(itemCreated.Id, 6) });

            item.RemoveUnits(units);

            Check.That(item.Units).Equals((uint)3);
            Check.That(item.UncommitedEvents.Single()).HasFieldsWithSameValues(new UnitsRemoved(itemCreated.Id, units));
        }

        [Test]
        public void ThrowsDomainExceptionWhenRemoveMoreUnitsThanExisting()
        {
            var itemCreated = new ItemCreated("item name");
            var item = new Item(new Event[] { itemCreated, new UnitsAdded(itemCreated.Id, 2) });

            Check.ThatCode(() => item.RemoveUnits(6)).Throws<DomainException>();
        }
    }
}
