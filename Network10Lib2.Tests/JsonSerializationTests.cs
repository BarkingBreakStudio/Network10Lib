using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using System.Text.Json;

namespace Network10Lib2.Tests
{
    public  class JsonSerializationTests
    {

        public class Person
        {
            public string Name { get; set; } = "";
            public int Age { get; set; }
            public float Position { get; set; }
        }

        public class Person2
        {
            public string Name { get; set; } = "";
            public int Age { get; set; }
            public float Position { get; set; }
        }


        [Fact]
        public void SerializeDeserialize_SimpleTest()
        {
            Person p = new Person { Name = "Max Müsert^^\"\"" , Age = 55, Position = 12.5f};

            var s = JsonSerializer.Serialize(p, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            Console.WriteLine(s);

            Person? p2 = JsonSerializer.Deserialize<Person>(s, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            Assert.NotNull(p2);
            if (p2 is not null)
            {
                Assert.True(p2 is Person);
                Assert.Equal("Max Müsert^^\"\"", p2.Name);
                Assert.Equal(55, p2.Age);
                Assert.Equal(12.5f, p2.Position);
            }
        }

        [Fact]
        public void SerializeDeserialize_MemoryStreamTest()
        {
            Person p = new Person { Name = "Max Müsert^^\"\"$", Age = 55, Position = 12.5f };
            JsonSerializerOptions JsonSerializerOptions = new(){ PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = false };

            MemoryStream stream = new MemoryStream();
            JsonSerializer.Serialize(stream, p, JsonSerializerOptions);

            stream.Seek(0, SeekOrigin.Begin);
            Person? p2 = JsonSerializer.Deserialize<Person>(stream, JsonSerializerOptions);
            Assert.NotNull(p2);
            if (p2 is not null)
            {
                Assert.True(p2 is Person);
                Assert.Equal("Max Müsert^^\"\"$", p2.Name);
                Assert.Equal(55, p2.Age);
                Assert.Equal(12.5f, p2.Position);
            }
        }

    }
}
