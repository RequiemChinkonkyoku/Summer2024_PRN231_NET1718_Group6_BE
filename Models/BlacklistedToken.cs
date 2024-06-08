using System;
using System.Collections.Generic;

namespace Models;

public partial class BlacklistedToken
{
    public int TokenId { get; set; }

    public string? TokenString { get; set; }

    public DateTime? BlacklistTime { get; set; }
}
