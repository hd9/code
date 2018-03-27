using System;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace HildenCo
{

    public class UserIndex : AbstractMultiMapIndexCreationTask<UserIndex.Result>
    {
        public class Result
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string MyCustomProperty { get; set; }
            public DateTime? LastLogin { get; set; }
            public DateTime CreatedOn { get; set; }
        }

        public UserIndex()
        {
            AddMap<User>( users =>
                  from user in users
                  select new
                  {
                      user.Id,
                      user.FirstName,
                      user.LastName,
                      user.PhoneNumber,
                      user.Email,
                      user.LastLogin,
                      user.CreatedOn,
                      CrlType = MetadataFor(user)["MyCustomProperty"]
                  });

            Index(x => x.Id, FieldIndexing.Analyzed);
            Index(x => x.FirstName, FieldIndexing.NotAnalyzed);
            Index(x => x.LastName, FieldIndexing.NotAnalyzed);
            Index(x => x.PhoneNumber, FieldIndexing.Analyzed);
            Index(x => x.Email, FieldIndexing.Analyzed);
            Index(x => x.LastLogin, FieldIndexing.NotAnalyzed);
            Index(x => x.CreatedOn, FieldIndexing.Analyzed);

            Store(x => x.PhoneNumber, FieldStorage.Yes);
            Store(x => x.Email, FieldStorage.Yes);
            Store(x => x.CreatedOn, FieldStorage.Yes);
            Store(x => x.MyCustomProperty, FieldStorage.Yes);
        }
    }
}