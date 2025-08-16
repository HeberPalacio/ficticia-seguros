using FluentAssertions;
using FicticiaSA.Backend.Security;

namespace FicticiaSA.Backend.Tests.Security
{
	public class PasswordHasherTests
	{
		[Fact]
		public void Hash_SameInput_ShouldProduceSameHash()
		{
			var hasher = new PasswordHasher(); 
			var h1 = hasher.Hash("1234");
			var h2 = hasher.Hash("1234");
			h1.Should().Be(h2);
		}

		[Fact]
		public void Verify_ShouldReturnTrue_ForCorrectPassword()
		{
			var hasher = new PasswordHasher();
			var hash = hasher.Hash("1234");
			hasher.Verify("1234", hash).Should().BeTrue();
		}
	}
}
