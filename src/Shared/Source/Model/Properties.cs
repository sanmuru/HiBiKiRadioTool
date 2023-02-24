// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Qtyi.HiBiKiRadio.Generators.Model;

public abstract class PropertyBase
{
    public string Name { get; set; }
    public bool Optional { get; set; }
}

public sealed class EnumProperty : PropertyBase
{
    public Type EnumType { get; set; }
}

public sealed class IntegerProperty : PropertyBase
{
    public int? Max { get; set; }
    public int? Min { get; set; }
}

public sealed class ObjectProperty : PropertyBase
{
    public string Type { get; set; }
}

public sealed class StringProperty : PropertyBase
{
    public bool CanBeEmpty { get; set; }
}
