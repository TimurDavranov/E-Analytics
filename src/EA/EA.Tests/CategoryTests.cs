using AutoFixture;
using EA.Application.Aggregates;
using EA.Domain.Events;
using EAnalytics.Common.Dtos;
using Shouldly;
using Xunit;

namespace EA.Tests;

public class CategoryTests
{
    private readonly CategoryAggregateRoot _categoryRoot;

    private readonly Fixture _fixture;
    public CategoryTests()
    {
        _fixture = new Fixture();
        var name1 = _fixture.Create<string>();
        var name2 = _fixture.Create<string>();
        var id = _fixture.Create<Guid>();
        
        _categoryRoot = new CategoryAggregateRoot(id, new 
            List<TranslationDto>()
            {
                new() {LanguageCode = "uz", Description = "uz", Title = name1},
                new() {LanguageCode = "ru", Description = "ru", Title = name2},
            }
        );

    }
    

    [Fact]
    public void Adding_Category_Test()
    {
        var events = _categoryRoot.GetUncommittedChanges();
        events.Count().ShouldBe(1);
        var outEvent = events.Single();
        outEvent.ShouldBeOfType<AddCategoryEvent>();
    }
    
    // [Fact]
    // public void Adding_Category_Product_Test()
    // {
    //     var events = _categoryRoot.GetUncommittedChanges();
    //     
    //     
    //     events.Count().ShouldBe(1);
    //     var outEvent = events.Single();
    //     outEvent.ShouldBeOfType<AddCategoryEvent>();
    // }
}