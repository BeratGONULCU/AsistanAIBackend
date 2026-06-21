using GeminiAsistanBackend.Domain.Entities;

namespace GeminiAsistanBackend.UnitTests;

public class CihazKomutuTests
{
    [Fact]
    public void CihazKomutu_ShouldBeCreated_WithValidValues()
    {
        var komut = new CihazKomutu
        {
            type = "command",
            domain = "system",
            target = "terminal",
            operation = "open",
            CalisacakKod = "cmd.exe",
            Aciklama = "Terminal açar"
        };

        Assert.Equal("command", komut.type);
        Assert.Equal("system", komut.domain);
        Assert.Equal("terminal", komut.target);
        Assert.Equal("open", komut.operation);
        Assert.Equal("cmd.exe", komut.CalisacakKod);
    }
}