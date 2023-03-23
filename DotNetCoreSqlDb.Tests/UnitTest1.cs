using Castle.Core.Logging;
using DotNetCoreSqlDb.Controllers;
using DotNetCoreSqlDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using Moq.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Reflection;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace DotNetCoreSqlDb.Tests
{
    public interface IEntityFrameworkCoreWrapper
    {
        Task<TSource?> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default);
        Task<TSource> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate);
    }
    public class StaticWrapper : IEntityFrameworkCoreWrapper
    {
        IEntityFrameworkCoreWrapper _wrapper;
        public StaticWrapper(IEntityFrameworkCoreWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        public Task<TSource?> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(source, cancellationToken);
        }

        public Task<TSource> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return QueryableExtensions.FirstOrDefaultAsync<TSource>(source, predicate);
        }
    }

    public class ControllerTests
    {
        private class TestWrapper : IEntityFrameworkCoreWrapper
        {
            public Task<TSource?> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
            {
                return EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(source, cancellationToken);
            }

            public Task<TSource> FirstOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
            {
                return QueryableExtensions.FirstOrDefaultAsync(source, predicate);
            }          
        }

        [Fact]
        public async void Test1()
        {
            var options = new DbContextOptionsBuilder<MyDatabaseContext>()
                .Options;

            var dbContextOptionsMock = new Mock<DbContextOptions<MyDatabaseContext>>();
            var myDbContext = new Mock<MyDatabaseContext>(options);
            var myLogger = new Mock<ILogger<TodosController>>();

            var testTodoList = new List<Todo>() {
            new Todo() { ID = 1, ViewCount = 0, Description = "test", CreatedDate = DateTime.Now }
                };

            myDbContext.Setup(x => x.Todo).ReturnsDbSet(testTodoList);

            var sut = new TodosController(myDbContext.Object, myLogger.Object);
            var results = await sut.Index();

            var viewResult = Assert.IsType<ViewResult>(results);
            var model = Assert.IsAssignableFrom<List<Todo>>(viewResult.ViewData.Model);
            Assert.Equal(testTodoList, model);
        }

        [Fact]
        public async Task Test2()
        {

            var id = 123;
            var options = new DbContextOptionsBuilder<MyDatabaseContext>()
                .Options;

            var dbContextOptionsMock = new Mock<DbContextOptions<MyDatabaseContext>>();
            var myDbContext = new Mock<MyDatabaseContext>(options);
            var myLogger = new Mock<ILogger<TodosController>>();
            var myTodo = new Todo() { ID = 123, ViewCount = 0, Description = "test", CreatedDate = DateTime.Now };
            var testTodoList = new List<Todo>(){
                myTodo
            };

            myDbContext.Setup(x => x.Todo).ReturnsDbSet(testTodoList);

            var entityFrameworkCoreWrapperMock = new Mock<IEntityFrameworkCoreWrapper>();
            entityFrameworkCoreWrapperMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<IQueryable<Todo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(myTodo);

            var sut = new TodosController(myDbContext.Object, myLogger.Object);
            var results = await sut.Details(id);

            var viewResult = Assert.IsType<ViewResult>(results);
            var model = Assert.IsAssignableFrom<Todo>(viewResult.ViewData.Model);
        }

    }
}