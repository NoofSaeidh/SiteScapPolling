using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public record struct Command(string Name, string Description)
{
    public string FullCommand => '/' + Name;

    public readonly bool Equals(Command other)
    {
        return string.Equals(Name, other.Name, StringComparison.InvariantCulture);
    }

    public override string ToString()
    {
        return $"{FullCommand} - {Description}";
    }

    public readonly override int GetHashCode()
    {
        return StringComparer.InvariantCulture.GetHashCode(Name);
    }

}
