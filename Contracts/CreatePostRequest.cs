using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts;

public record CreatePostRequest(
    string title, string body, bool isPrivate);
