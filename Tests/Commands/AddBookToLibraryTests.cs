﻿using System;
using Lending.Cqrs.Query;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.Model;
using Lending.Domain.RemoveBookFromLibrary;
using NUnit.Framework;
using static Tests.DefaultTestData;

namespace Tests.Commands
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/9
    /// As a User I want to Add Books to my Library so that my Connections can see what Books I own.
    /// </summary>
    public class AddBookToLibraryTests : FixtureWithEventStoreAndNHibernate
    {

        /// <summary>
        /// GIVEN User1 is a Registered User
        /// WHEN User1 Adds Book1 to her Library
        /// THEN Book1 is Added to User1's Library
        /// </summary>
        [Test]
        public void AddingNewBookToLibraryShouldSucceed()
        {
            Given(Library1Opens);
            When(User1AddsBookToLibrary);
            Then(succeed);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, Book1AddedToUser1Library);
        }

        /// <summary>
        /// GIVEN User1 is a Registered User AND Book1 is in User1's Library
        /// WHEN User1 Adds Book1 to her Library
        /// THEN User1 is notified that Book1 is already in her Library
        /// </summary>
        [Test]
        public void AddingDuplicateBookToLibraryShouldFail()
        {
            Given(Library1Opens, User1AddsBookToLibrary);
            When(User1AddsBookToLibrary);
            Then(failBecauseBookAlreadyInLibrary);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, Book1AddedToUser1Library);
        }

        /// <summary>
        /// GIVEN User1 is a Registered User AND User1 has Added and Removed Book1 from her Library
        /// WHEN User1 Adds Book1 to her Library
        /// THEN Book1 is Added to User1's Library
        /// </summary>
        [Test]
        public void AddingPreviouslyRemovedBookToLibraryShouldSucceed()
        {
            Given(Library1Opens, User1AddsBookToLibrary, user1RemovesBookFromLibrary);
            When(User1AddsBookToLibrary);
            Then(succeed);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, Book1AddedToUser1Library, book1RemovedFromLibrary, Book1AddedToUser1Library);
        }

    }
}