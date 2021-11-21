using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Network10Lib.Tests
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

        public class House
        {
            public string Name { get; set; } = "";
        }

        public class Person3<T>
        {
            public string Name { get; set; } = "";
            public int Age { get; set; }
            public float Position { get; set; }
            public object StringObject { get; set; } = "";
            public object BoolObject { get; set; } = "";
            public object ObjectObject { get; set; } = "";
            public T? TObject { get; set; }
        }

        [Fact]
        public void SerializeDeserialize_ObjectInsideObjectTest()
        {
            Person3<House> p = new Person3<House> { Name = "Max Müsert^^\"\"", Age = 55, Position = 12.5f, StringObject = "teststring", BoolObject = true, ObjectObject = new House {Name = "myHouse" }, TObject = new House { Name = "myHouse2" } };
            JsonSerializerOptions options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            

            var s = JsonSerializer.Serialize(p, options);
            Console.WriteLine(s);

            options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            options.Converters.Add(new ObjectBoolConverter());
            Person3<object>? p2 = JsonSerializer.Deserialize<Person3<object>>(s, options);
            Assert.NotNull(p2);
            if (p2 is not null)
            {
                Assert.True(p2 is Person3<object>);
                Assert.Equal("Max Müsert^^\"\"", p2.Name);
                Assert.Equal(55, p2.Age);
                Assert.Equal(12.5f, p2.Position);
                Assert.Equal(true, p2.BoolObject);
                Assert.Equal("teststring", p2.StringObject);
                JsonElement je = (JsonElement?)p2.TObject ?? new JsonElement();
                House? h2 = JsonSerializer.Deserialize<House>(je, options);
                Assert.Equal("myHouse2", h2?.Name);

                /*
                JsonElement js = (JsonElement)p2.StringObject;
                Assert.Equal(JsonValueKind.String, js.ValueKind);
                JsonElement js2 = (JsonElement)p2.BoolObject;
                Assert.Equal(JsonValueKind.True, js.ValueKind);
                //Nullable<string> test = p2.MoreData as Nullable<string>;
                //Assert.Equal("someData", (p2.MoreData as Nullable<string>).Value);*/
            }
        }



        public class ObjectBoolConverter : JsonConverter<object>
        {
            public override object Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.True)
                {
                    return true;
                }

                if (reader.TokenType == JsonTokenType.False)
                {
                    return false;
                }

                if (reader.TokenType == JsonTokenType.String)
                {
                    return reader.GetString() ?? "";
                }


                // Forward to the JsonElement converter
                var converter = options.GetConverter(typeof(JsonElement)) as JsonConverter<JsonElement>;
                if (converter != null)
                {
                    return converter.Read(ref reader, type, options);
                }

                throw new JsonException();

                // or for best performance, copy-paste the code from that converter:
                //using (JsonDocument document = JsonDocument.ParseValue(ref reader))
                //{
                //    return document.RootElement.Clone();
                //}
            }

            public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
            {
                throw new InvalidOperationException("Directly writing object not supported");
            }
        }
    }
}
