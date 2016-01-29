using System;
using Lending.Cqrs.Query;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.ReadModels.Relational.SearchForLibrary;
using NUnit.Framework;
using static Tests.DefaultTestData;

namespace Tests.Queries
{
    [TestFixture]
    public class SearchForLibraryTests: FixtureWithEventStoreAndNHibernate
    {

        /// <summary>
        /// GIVEN Libraries with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have been Opened 
        /// WHEN I Search for Libraries with the search string 'Lew' 
        /// THEN Lbrary 'Joshua Lewis' gets returned
        /// </summary>
        [Test]
        public void SearchingForLibraryWithSingleMatchShouldReturnThatUser()
        {
            Given(JoshuaLewisOpensLibrary, SuzaanHepburnOpensLibrary, JosieDoeOpensLibrary, AudreyHepburnOpensLibrary);
            WhenGetEndpoint("libraries/Lew");
            Then<Result<OpenedLibrary[]>>((new Result<OpenedLibrary[]>(new[]
            {
                new OpenedLibrary(Library1Id, JoshuaLewisLibraryOpened.Name), 
            })));
            AndEventsSavedForAggregate<Library>(Library1Id, JoshuaLewisLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library2Id, SuzaanHepburnLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library3Id, JosieDoeLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library4Id, AudreyHepburnLibraryOpened);
        }

        /// <summary>
        /// GIVEN Libraries with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have been Opened 
        /// WHEN I Search for Libraries with the search string 'lEw' 
        /// THEN Library 'Joshua Lewis' gets returned
        /// </summary>
        [Test]
        public void SearchingForLibraryWithSingleMatchWithWrongCaseShouldReturnThatUser()
        {
            Given(JoshuaLewisOpensLibrary, SuzaanHepburnOpensLibrary, JosieDoeOpensLibrary, AudreyHepburnOpensLibrary);
            WhenGetEndpoint("libraries/lEw");
            Then<Result<OpenedLibrary[]>>((new Result<OpenedLibrary[]>(new[]
            {
                new OpenedLibrary(Library1Id, JoshuaLewisLibraryOpened.Name),
            })));
            AndEventsSavedForAggregate<Library>(Library1Id, JoshuaLewisLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library2Id, SuzaanHepburnLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library3Id, JosieDoeLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library4Id, AudreyHepburnLibraryOpened);
        }

        /// <summary>
        /// GIVEN Libraries with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have been Opened 
        /// WHEN I Search for Libraries with the search string 'Pet'
        /// THEN no Libraries get returned
        /// </summary>
        [Test]
        public void SearchingForLibraryWithNoMatchesShouldReturnEmptyList()
        {
            Given(JoshuaLewisOpensLibrary, SuzaanHepburnOpensLibrary, JosieDoeOpensLibrary, AudreyHepburnOpensLibrary);
            WhenGetEndpoint("libraries/Pet");
            Then<Result<OpenedLibrary[]>>(new Result<OpenedLibrary[]>(new OpenedLibrary[]
            {
            }));
            AndEventsSavedForAggregate<Library>(Library1Id, JoshuaLewisLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library2Id, SuzaanHepburnLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library3Id, JosieDoeLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library4Id, AudreyHepburnLibraryOpened);

        }

        /// <summary>
        /// GIVEN Libraries with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have been Opened 
        /// WHEN I Search for Libraries with the search string 'Jos'
        /// THEN Libraries 'Joshua Lewis' and 'Josie Doe' get returned
        /// </summary>
        [Test]
        public void SearchingForLibraryWithTwoMatchsShouldReturnTwoLibraries()
        {
            Given(JoshuaLewisOpensLibrary, SuzaanHepburnOpensLibrary, JosieDoeOpensLibrary, AudreyHepburnOpensLibrary);
            WhenGetEndpoint("libraries/Jos");
            Then<Result<OpenedLibrary[]>>((new Result<OpenedLibrary[]>(new[]
            {
                new OpenedLibrary(Library1Id, JoshuaLewisLibraryOpened.Name),
                new OpenedLibrary(Library3Id, JosieDoeLibraryOpened.Name),
            })));
            AndEventsSavedForAggregate<Library>(Library1Id, JoshuaLewisLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library2Id, SuzaanHepburnLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library3Id, JosieDoeLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library4Id, AudreyHepburnLibraryOpened);
        }

    }
}
