using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Roomies.DAL.Tests
{
    [TestFixture]
    public class CategoryGatewayTests
    {
        readonly Random _random = new Random();

        [Test]
        public async Task can_create_find_update_delete_category()
        {
            CategoryGateway sut = new CategoryGateway( TestHelpers.ConnectionString );

            string categoryName = TestHelpers.RandomTestName();
            string icon = TestHelpers.RandomTestName();
            int collocId = 0;

            Result<int> categoryResult = await sut.CreateCategory( categoryName, icon, collocId );
            Assert.That( categoryResult.Status, Is.EqualTo( Status.Created ) );

            int categoryId = categoryResult.Content;

            Result<CategoryData> categoryData;
            {
                categoryData = await sut.FindCategoryId( categoryId );
                CheckCategory( categoryData, categoryName, icon, collocId );
            }
            {
                categoryName = TestHelpers.RandomTestName();
                icon = TestHelpers.RandomTestName();
                CollocGateway coGateway = new CollocGateway( TestHelpers.ConnectionString );
                var colloc = await coGateway.CreateColloc( TestHelpers.RandomTestName(), 0 );
                collocId = colloc.Content;
                await sut.UpdateCategory( categoryId, categoryName, icon, collocId );

                categoryData = await sut.FindCategoryId( categoryId );
                CheckCategory( categoryData, categoryName, icon, collocId );
            }

            {
                Result r = await sut.DeleteCategory( categoryId );
                Assert.That( r.Status, Is.EqualTo( Status.Ok ) );

                categoryData = await sut.FindCategoryId( categoryId );
                Assert.That( categoryData.Status, Is.EqualTo( Status.NotFound ) );
            }
        }

        private void CheckCategory( Result<CategoryData> category, string categoryName, string icon, int collocId )
        {
            Assert.That( category.Status, Is.EqualTo( Status.Ok ) );
            Assert.That( category.Content.CategoryName, Is.EqualTo( categoryName ) );
            Assert.That( category.Content.IconName, Is.EqualTo( icon ) );
            Assert.That( category.Content.CollocId, Is.EqualTo( collocId) );

        }
    }
}
