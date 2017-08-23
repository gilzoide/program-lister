using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RequestArguments : Dictionary<string, string> {
	public override string ToString() {
		return string.Join("&", this.Select(a => WWW.EscapeURL(a.Key) + "=" + WWW.EscapeURL(a.Value)).ToArray());
	}
}
