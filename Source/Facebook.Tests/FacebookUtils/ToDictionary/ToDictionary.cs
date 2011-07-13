namespace Facebook.Utils.Tests.ToDictionary
{
    using System.Runtime.Serialization;
    using Xunit;

    public class GivenAObjectThen
    {
        public Post post;

        public GivenAObjectThen()
        {
            this.post = new Post { Message = "dummy message", Privacy = "ALL_FRIENDS" };
        }

        [Fact]
        public void ResultIsNotNull()
        {
            var parameters = FacebookUtils.ToDictionary(this.post);

            Assert.NotNull(parameters);
        }

        [Fact]
        public void ThereAreExactlyTwoKeys()
        {
            var parameters = FacebookUtils.ToDictionary(this.post);

            Assert.Equal(2, parameters.Count);
        }

        [Fact]
        public void ContainsMessageKey()
        {
            var parameters = FacebookUtils.ToDictionary(this.post);

            Assert.True(parameters.ContainsKey("message"));
        }

        [Fact]
        public void ContainsPrivacyKey()
        {
            var parameters = FacebookUtils.ToDictionary(this.post);

            Assert.True(parameters.ContainsKey("privacy"));
        }

        [Fact]
        public void MessageShouldBeSetCorrectly()
        {
            var parameters = FacebookUtils.ToDictionary(this.post);

            Assert.Equal(this.post.Message, parameters["message"]);
        }

        [Fact]
        public void PrivacyShouldBeSetCorrectly()
        {
            var parameters = FacebookUtils.ToDictionary(this.post);

            Assert.Equal(this.post.Privacy, parameters["privacy"]);
        }

        [DataContract]
        public class Post
        {
            [DataMember(Name = "message")]
            public string Message { get; set; }

            [DataMember(Name = "privacy")]
            public string Privacy { get; set; }
        }
    }
}