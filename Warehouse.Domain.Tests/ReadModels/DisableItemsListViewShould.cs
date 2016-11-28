﻿using System;
using Moq;
using NFluent;
using NUnit.Framework;
using Warehouse.Domain.ReadModels;
using Warehouse.Domain.ReadModels.Repositories;

namespace Warehouse.Domain.Tests.ReadModels
{
    [TestFixture]
    public class DisableItemsListViewShould
    {
        [Test]
        public void ReturnDisableItemsFromItemsListRepository()
        {
            var itemId = Guid.NewGuid();
            var repositoryMock = new Mock<IItemsListRepository>();
            repositoryMock.Setup(x => x.GetDisableItems()).Returns(new[] { new DisableItemView(itemId, "chair") });

            var disableItemsListView = new DisableItemsListView(repositoryMock.Object);
            Check.That(disableItemsListView.Items).ContainsExactly(new DisableItemView(itemId, "chair"));
        }
    }
}