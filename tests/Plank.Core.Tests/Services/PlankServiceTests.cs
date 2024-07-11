using System.Data;
using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Plank.Core.Data;
using Plank.Core.Services;
using Plank.Core.Tests.Helpers;
using Plank.Core.Tests.Helpers.Entities;
using X.PagedList;

namespace Plank.Core.Tests.Services
{
    public class PlankServiceTests
    {
        private readonly Mock<ILogger> _logger;

        public PlankServiceTests()
        {
            _logger = new Mock<ILogger>();
        }

        [Fact]
        public async Task Add_ChildEntityNotValid_NotCreated()
        {
            // Arrange
            var invalidChild = TestHelper.GetChildOne();
            invalidChild.Address = string.Empty;

            var item = TestHelper.GetParentEntity();
            item.ChildOne =
            [
                TestHelper.GetChildOne(),
                invalidChild
            ];

            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.AddAsync(item);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            repo.Verify(m => m.AddAsync(item), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Add_EntityNotValid_NotCreated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            item.FirstName = string.Empty;

            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.AddAsync(item);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            repo.Verify(m => m.AddAsync(item), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Add_EntityNull_NotCreated()
        {
            // Arrange
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.AddAsync(null);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("ParentEntity cannot be null.");
            repo.Verify(m => m.AddAsync(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Add_EntityValid_Created()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.AddAsync(item)).Returns(Task.FromResult(item));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.AddAsync(item);

            // Assert
            got.ValidationResults.IsValid.Should().BeTrue();
            got.Item.Id.Should().Be(item.Id);
            repo.Verify(m => m.AddAsync(item), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Add_FluentValidatorHasFailResult_NotCreated()
        {
            // Arrange
            var entity = new ChildThreeEntity();
            var repo = new Mock<IRepository<ChildThreeEntity>>();
            var logger = new Mock<ILogger>();

            // Act
            var manager = new PlankService<ChildThreeEntity>(repo.Object, logger.Object);
            var got = await manager.AddAsync(entity);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            got.ValidationResults.ElementAt(0).Message.Should().Be("'Id' must be greater than '0'.");
            repo.Verify(m => m.AddAsync(It.IsAny<ChildThreeEntity>()), Times.Never);
            logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Add_FluentValidatorHasPassResult_Created()
        {
            // Arrange
            var entity = new ChildThreeEntity
            {
                Id = 1
            };
            var repo = new Mock<IRepository<ChildThreeEntity>>();
            var logger = new Mock<ILogger>();

            // Act
            var manager = new PlankService<ChildThreeEntity>(repo.Object, logger.Object);
            var got = await manager.AddAsync(entity);

            // Assert
            got.ValidationResults.IsValid.Should().BeTrue();
            repo.Verify(m => m.AddAsync(It.IsAny<ChildThreeEntity>()), Times.Once());
            logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Add_RepositoryThrowException_NotCreated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.AddAsync(item)).Throws(new DataException("Error"));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.AddAsync(item);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            got.ValidationResults.ElementAt(0).Message.Should().Be("There was an issue processing the request, see the plank logs for details");
            got.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.ErrorMessage(It.IsAny<DataException>()), Times.Once);
        }

        [Fact]
        public async Task Add_ValidatorHasFailResult_NotCreated()
        {
            // Arrange
            var entity = new GrandParentEntity();
            var repo = new Mock<IRepository<GrandParentEntity>>();
            var logger = new Mock<ILogger>();

            // Act
            var manager = new PlankService<GrandParentEntity>(repo.Object, logger.Object);
            var got = await manager.AddAsync(entity);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            got.ValidationResults.ElementAt(0).Message.Should().Be("There was a problem");
            repo.Verify(m => m.AddAsync(It.IsAny<GrandParentEntity>()), Times.Never());
            logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Add_ValidatorHasFailResultOnChildEntity_NotCreated()
        {
            // Arrange
            var entity = TestHelper.GetParentEntity();
            entity.ChildOne = [TestHelper.GetChildOne()];
            entity.ChildTwo = [TestHelper.GetChildTwo()];
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.AddAsync(entity);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            got.ValidationResults.ElementAt(0).Message.Should().Be("There was a problem");
            repo.Verify(m => m.AddAsync(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task BulkAdd_EntitiesValid_Created()
        {
            // Arrange
            var entities = new List<ParentEntity>
            {
                TestHelper.GetParentEntity(1),
                TestHelper.GetParentEntity(2)
            };
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.BulkAddAsync(entities)).Returns(Task.FromResult(entities));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.BulkAddAsync(entities);

            // Assert
            got.Items.Should().HaveCount(2);
            foreach (var item in got.Items)
            {
                item.ValidationResults.IsValid.Should().BeTrue();
                entities.Where(i => i.Id == item.Item.Id).Should().HaveCount(1);
            }

            repo.Verify(m => m.BulkAddAsync(entities), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task BulkAdd_OneEntityNotValid_PartialCreate()
        {
            // Arrange
            var entities = new List<ParentEntity>
            {
                TestHelper.GetParentEntity(1),
                TestHelper.GetParentEntity(2)
            };
            entities[1].FirstName = string.Empty;

            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.BulkAddAsync(entities)).Returns(Task.FromResult(entities[0]));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.BulkAddAsync(entities);

            // Assert
            got.Items.Should().HaveCount(2);
            got.Items.Where(i => i.ValidationResults.IsValid).Should().HaveCount(1);
            got.Items.Where(i => i.ValidationResults.IsValid == false).Should().HaveCount(1);

            repo.Verify(m => m.BulkAddAsync(It.IsAny<IEnumerable<ParentEntity>>()), Times.Once());
            repo.Verify(m => m.BulkAddAsync(entities), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task BulkAdd_RepositoryThrowException_NotCreated()
        {
            // Arrange
            var entities = new List<ParentEntity>
            {
                TestHelper.GetParentEntity(),
                TestHelper.GetParentEntity()
            };
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.BulkAddAsync(entities)).Throws(new DataException("Error"));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.BulkAddAsync(entities);

            // Assert
            got.Items.Should().HaveCount(2);
            got.Items.Where(i => i.ValidationResults.IsValid == false).Should().HaveCount(2);
            got.Items.Where(i => i.ValidationResults.ElementAt(0).Message == "There was an issue processing the request, see the plank logs for details").Should().HaveCount(2);
            got.Items.Where(i => i.ValidationResults.ElementAt(0).Key == "Error").Should().HaveCount(2);
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.ErrorMessage(It.IsAny<DataException>()), Times.Once);
        }

        [Fact]
        public void Constructor_RepositoryNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Action act = () =>
            {
                var manager = new PlankService<ParentEntity>(null, _logger.Object);
            };

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'repository')");
        }

        [Fact]
        public void Constructor_LoggerNull_ArgumentNullException()
        {
            // Arrange
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            Action act = () =>
            {
                var manager = new PlankService<ParentEntity>(repo.Object, null);
            };

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'logger')");
        }

        [Fact]
        public async Task Delete_EntityExists_Deleted()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var id = 1;
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.DeleteAsync(id)).Returns(Task.FromResult(id));
            repo.Setup(m => m.GetAsync(id)).Returns(Task.FromResult(item));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.DeleteAsync(id);

            // Assert
            got.ValidationResults.IsValid.Should().BeTrue();
            got.Id.Should().Be(id);
            repo.Verify(m => m.GetAsync(id), Times.Once());
            repo.Verify(m => m.DeleteAsync(id), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Delete_EntityNotFound_NotDeleted()
        {
            // Arrange
            ParentEntity item = null;
            var id = 1;
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(id)).Returns(Task.FromResult(item));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.DeleteAsync(id);

            // Assert
            got.ValidationResults.IsValid.Should().BeTrue();
            repo.Verify(m => m.GetAsync(id), Times.Once());
            repo.Verify(m => m.DeleteAsync(id), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Delete_RepositoryThrowsException_NotDeleted()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var id = 1;
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(id)).Returns(Task.FromResult(item));
            repo.Setup(m => m.DeleteAsync(id)).Throws(new DataException("Error"));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.DeleteAsync(id);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            got.ValidationResults.ElementAt(0).Message.Should().Be("There was an issue processing the request, see the plank logs for details");
            got.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            repo.Verify(m => m.GetAsync(id), Times.Once());
            repo.Verify(m => m.DeleteAsync(id), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Once());
            _logger.Verify(m => m.ErrorMessage(It.IsAny<DataException>()), Times.Once());
        }

        [Fact]
        public async Task Get_EntityFoundById_ItemReturned()
        {
            // Arrange
            var id = 1;
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(id)).Returns(Task.FromResult(item));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.GetAsync(id);

            // Assert
            got.Should().NotBeNull();
            got.IsValid.Should().BeTrue();
            repo.Verify(m => m.GetAsync(id), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Get_RepositoryThrowsException_IsValidFalse()
        {
            // Arrange
            var id = 1;
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(id)).Throws(new DataException("Error"));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.GetAsync(id);

            // Assert
            got.Should().NotBeNull();
            got.IsValid.Should().BeFalse();
            got.Message.Should().Be("There was an issue processing the request, see the plank logs for details");
            repo.Verify(m => m.GetAsync(id), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Once());
            _logger.Verify(m => m.ErrorMessage(It.IsAny<DataException>()), Times.Once());
        }

        [Fact]
        public async Task Search_NoCriteria_PageReturned()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var item = TestHelper.GetParentEntity();
            var list = new List<ParentEntity>() { item, item }.ToPagedList(1, 10);
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.SearchAsync(It.IsAny<Expression<Func<ParentEntity, bool>>>(), null, pageNumber, pageSize)).Returns(Task.FromResult(list));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.SearchAsync(null, null, pageNumber, pageSize);

            // Assert
            got.IsValid.Should().BeTrue();
            got.IsFirstPage.Should().BeTrue();
            got.IsLastPage.Should().BeTrue();
            got.HasNextPage.Should().BeFalse();
            got.HasPreviousPage.Should().BeFalse();
            got.Items.Should().HaveCount(2);
            got.PageNumber.Should().Be(1);
            got.PageSize.Should().Be(10);
            got.TotalItemCount.Should().Be(2);
            repo.Verify(m => m.SearchAsync(It.IsAny<Expression<Func<ParentEntity, bool>>>(), null, pageNumber, pageSize), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Search_RepositoryThrowsException_IsValidFalse()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.SearchAsync(It.IsAny<Expression<Func<ParentEntity, bool>>>(), null, pageNumber, pageSize)).Throws(new DataException("Error"));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.SearchAsync(null, null, pageNumber, pageSize);

            // Assert
            got.Should().NotBeNull();
            got.IsValid.Should().BeFalse();
            got.Message.Should().Be("There was an issue processing the request, see the plank logs for details");
            repo.Verify(m => m.SearchAsync(It.IsAny<Expression<Func<ParentEntity, bool>>>(), null, pageNumber, pageSize), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.ErrorMessage(It.IsAny<DataException>()), Times.Once());
        }

        [Fact]
        public async Task Update_EntityNotFound_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            ParentEntity rItem = null;
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(rItem));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.UpdateAsync(item);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            got.ValidationResults.ElementAt(0).Message.Should().Be("Item could not be found");
            got.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_EntityNotValid_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            item.FirstName = null;

            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.UpdateAsync(item);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            repo.Verify(m => m.UpdateAsync(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_EntityNull_NotUpdated()
        {
            // Arrange
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.UpdateAsync(null);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            got.ValidationResults.ElementAt(0).Message.Should().Be("ParentEntity cannot be null.");
            repo.Verify(m => m.UpdateAsync(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_EntityValid_Updated()
        {
            // Arrange
            var existing = TestHelper.GetParentEntity();
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(existing));
            repo.Setup(m => m.UpdateAsync(existing)).Returns(Task.FromResult(existing));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.UpdateAsync(item);

            // Assert
            got.ValidationResults.IsValid.Should().BeTrue();
            got.Item.Id.Should().Be(item.Id);
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            repo.Verify(m => m.UpdateAsync(existing), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_PartialUpdateEntityNotFound_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            ParentEntity rItem = null;
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(rItem));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.UpdateAsync(item, p => p.FirstName);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            got.ValidationResults.ElementAt(0).Message.Should().Be("Item could not be found");
            got.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_PartialUpdateEntityNull_NotUpdated()
        {
            // Arrange
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.UpdateAsync(null, p => p.FirstName);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            got.ValidationResults.ElementAt(0).Message.Should().Be("Value cannot be null or empty.\r\nParameter name: item");
            got.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_PartialUpdateEntityValid_Updated()
        {
            // Arrange
            var existing = TestHelper.GetParentEntity();
            var item = new ParentEntity { Id = 0, IsActive = false };
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(existing));
            repo.Setup(m => m.UpdateAsync(existing)).Returns(Task.FromResult(existing));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.UpdateAsync(item, p => p.IsActive);

            // Assert
            got.ValidationResults.IsValid.Should().BeTrue();
            got.Item.Id.Should().Be(item.Id);
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            repo.Verify(m => m.UpdateAsync(existing), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_PartialUpdateInvalidProperty_NotUpdated()
        {
            // Arrange
            var existing = TestHelper.GetParentEntity();
            var item = new ParentEntity { Id = 0, IsActive = false };
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.UpdateAsync(item, p => p, p => p.IsActive);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            got.ValidationResults.ElementAt(0).Message.Should().Be("Value cannot be null or empty.\r\nParameter name: properties");
            got.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_PartialUpdatePropertiesNull_NotUpdated()
        {
            // Arrange
            var existing = TestHelper.GetParentEntity();
            var item = new ParentEntity { Id = 0, IsActive = false };
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.UpdateAsync(item, null);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            got.ValidationResults.ElementAt(0).Message.Should().Be("Value cannot be null or empty.\r\nParameter name: properties");
            got.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_PartialUpdateRepositoryThrowsException_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IRepository<ParentEntity>>();

            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(item));
            repo.Setup(m => m.UpdateAsync(item)).Throws(new DataException("Error"));

            // Act
            //
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.UpdateAsync(item, p => p.FirstName);

            // Assert
            //
            got.ValidationResults.IsValid.Should().BeFalse();
            got.ValidationResults.ElementAt(0).Message.Should().Be("There was an issue processing the request, see the plank logs for details");
            got.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            repo.Verify(m => m.UpdateAsync(item), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.ErrorMessage(It.IsAny<DataException>()), Times.Once());
        }

        [Fact]
        public async Task Update_RepositoryThrowsException_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IRepository<ParentEntity>>();

            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(item));
            repo.Setup(m => m.UpdateAsync(item)).Throws(new DataException("Error"));

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.UpdateAsync(item);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            got.ValidationResults.ElementAt(0).Message.Should().Be("There was an issue processing the request, see the plank logs for details");
            got.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            repo.Verify(m => m.UpdateAsync(item), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.ErrorMessage(It.IsAny<DataException>()), Times.Once());
        }

        [Fact]
        public async Task Update_ValidatorHasFailResult_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            item.ChildOne = new List<ChildOneEntity> { TestHelper.GetChildOne() };
            item.ChildTwo = new List<ChildTwoEntity> { TestHelper.GetChildTwo() };
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankService<ParentEntity>(repo.Object, _logger.Object);
            var got = await manager.UpdateAsync(item);

            // Assert
            got.ValidationResults.IsValid.Should().BeFalse();
            got.ValidationResults.ElementAt(0).Message.Should().Be("There was a problem");
            repo.Verify(m => m.UpdateAsync(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}