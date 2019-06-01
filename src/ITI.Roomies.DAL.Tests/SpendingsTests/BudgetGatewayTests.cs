using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Roomies.DAL.Tests
{
    [TestFixture]
    public class BudgetGatewayTests
    {
        [Test]
        public async Task can_create_find_update_delete_a_budget()
        {
            BudgetGateway sut = new BudgetGateway( TestHelpers.ConnectionString );

            int categoryId = 0;
            DateTime date1 = TestHelpers.RandomBirthDate( 1 );
            DateTime date2 = TestHelpers.RandomBirthDate( 2 );
            int amount = 50;
            int collocId = 0;

            Result<int> budgetResult = await sut.CreateBudget( categoryId, date1, date2, amount, collocId );
            Assert.That( budgetResult.Status, Is.EqualTo( Status.Created ) );

            int budgetId = budgetResult.Content;

            Result<BudgetData> budgetData;
            {
                budgetData = await sut.FindBudgetById( budgetId );
                checkBudget( budgetData, categoryId, date1, date2, amount, collocId );
            }
            {
                CategoryGateway categoryGateway = new CategoryGateway( TestHelpers.ConnectionString );
                Result<int> result = await categoryGateway.CreateCategory( TestHelpers.RandomTestName(), TestHelpers.RandomTestName(), 0 );
                categoryId = result.Content;
                CollocGateway collocGateway = new CollocGateway( TestHelpers.ConnectionString );
                Result<int> result1 = await collocGateway.CreateColloc( TestHelpers.RandomTestName(), 0 );
                collocId = result.Content;
                amount = 100;
                date1 = TestHelpers.RandomBirthDate( 2 );
                date2 = TestHelpers.RandomBirthDate( 3 );
                await sut.UpdateBudget( budgetId, categoryId, date1, date2, amount, collocId );

                budgetData = await sut.FindBudgetById( budgetId );
                checkBudget(budgetData, categoryId, date1, date2, amount, collocId);

            }
            {
                Result r = await sut.DeleteBudget( budgetId );
                Assert.That( r.Status, Is.EqualTo( Status.Ok ) );

                budgetData = await sut.FindBudgetById( budgetId );
                Assert.That( budgetData.Status, Is.EqualTo( Status.NotFound ) );
            }
        }

        private void checkBudget( Result<BudgetData> budget, int categoryId, DateTime date1, DateTime date2, int amount, int collocId )
        {
            Assert.That( budget.Status, Is.EqualTo( Status.Ok));
            Assert.That( budget.Content.Date1, Is.EqualTo( date1 ) );
            Assert.That( budget.Content.Date2, Is.EqualTo( date2 ) );
            Assert.That( budget.Content.Amount, Is.EqualTo( amount) );
            Assert.That( budget.Content.CategoryId, Is.EqualTo( categoryId ) );
            Assert.That( budget.Content.CollocId, Is.EqualTo( collocId) );
        }
    }
}
