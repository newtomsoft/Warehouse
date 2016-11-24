﻿using System;
using Moq;
using NFluent;
using NUnit.Framework;
using Warehouse.Domain.Events;
using Warehouse.Domain.ReadModels;
using Warehouse.Domain.ReadModels.Base;
using Warehouse.Domain.ReadModels.Repositories;

namespace Warehouse.Domain.Tests.ReadModels.Repositories
{
    [TestFixture]
    public class ItemsListRepositoryShould
    {
        private Mock<IRepository> repositoryMock;

        private ItemsListRepository itemsListRepository;

        [SetUp]
        public void Init()
        {
            this.repositoryMock = new Mock<IRepository>();
            this.itemsListRepository = new ItemsListRepository(this.repositoryMock.Object);
        }

        [Test]
        public void RetriveItemsWhenGetItems()
        {
            var items = new[] { new ItemView(Guid.NewGuid(), "item1"), new ItemView(Guid.NewGuid(), "item2") };
            this.repositoryMock.Setup(x => x.Get<ItemView>()).Returns(items);

            Check.That(this.itemsListRepository.GetItems()).Equals(items);
        }

        [Test]
        public void InsertNewItemViewWhenHandleItemCreatedEvent()
        {
            var @event = new ItemCreated("item name");
            this.itemsListRepository.Handle(@event);
            this.repositoryMock.Verify(x => x.Insert(new ItemView(@event.Id, @event.Name)));
        }

        [Test]
        public void ChangeItemViewNameAndUpdateWhenHandleItemRenamed()
        {
            var item = new ItemView(Guid.NewGuid(), "first name");
            this.repositoryMock.Setup(x => x.Get<ItemView>()).Returns(new[] { item });

            this.itemsListRepository.Handle(new ItemRenamed(item.Id.Value, "new name"));

            Check.That(item.Name).Equals("new name");
            this.repositoryMock.Verify(x => x.Update(item));
        }
    }
}
