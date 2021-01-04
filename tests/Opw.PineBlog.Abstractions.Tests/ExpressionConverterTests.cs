//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using Xunit;

//namespace Opw.PineBlog
//{
//    public class ExpressionConverterTests
//    {
//        private readonly IEnumerable<ObjectB> _listB;

//        public ExpressionConverterTests()
//        {
//            _listB = new List<ObjectB>
//            {
//                new ObjectB{ Id = 1, Name = "A" },
//                new ObjectB{ Id = 2, Name = "A" },
//                new ObjectB{ Id = 3, Name = "B" },
//                new ObjectB{ Id = 4, Name = "B" },
//                new ObjectB{ Id = 5, Name = "B" },
//            };
//        }

//        [Fact]
//        public void Convert_Should_Convert()
//        {
//            var expressionConverter = new ExpressionConverter<ObjectA, ObjectB>();

//            Expression<Func<ObjectA, bool>> expA = o => o.Id == 1;

//            var convertedExp = expressionConverter.Convert(expA);

//            var query = _listB.AsQueryable();
//            var results = query.Where(convertedExp);
//        }

//        private class ObjectA
//        {
//            public int Id { get; set; }
//            public string Name { get; set; }
//        }

//        private class ObjectB
//        {
//            public int Id { get; set; }
//            public string Name { get; set; }
//        }
//    }
//}
