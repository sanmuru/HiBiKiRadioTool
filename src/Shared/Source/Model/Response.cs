// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xml.Serialization;

namespace Qtyi.HiBiKiRadio.Generators.Model;

public sealed class Response
{
    [XmlElement(ElementName = "Object", Type = typeof(Object))]
    public List<Object> Models { get; set; }
}
