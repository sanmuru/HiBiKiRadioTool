// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xml.Serialization;

namespace Qtyi.HiBiKiRadio.Generators.Model;

public sealed class Object
{
    public string Name { get; set; }

    [XmlElement(ElementName = "Enum", Type = typeof(EnumProperty))]
    [XmlElement(ElementName = "Integer", Type = typeof(IntegerProperty))]
    [XmlElement(ElementName = "Object", Type = typeof(ObjectProperty))]
    [XmlElement(ElementName = "String", Type = typeof(StringProperty))]
    public List<PropertyBase> Properties { get; set; }
}
