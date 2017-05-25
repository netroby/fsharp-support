﻿using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.FSharp.Impl.Cache2.Declarations;
using JetBrains.ReSharper.Psi.FSharp.Tree;

namespace JetBrains.ReSharper.Psi.FSharp.Impl.Cache2
{
  /// <summary>
  /// Used for type abbreviations and abstract types
  /// </summary>
  internal class HiddenTypePart : FSharpClassLikePart<IFSharpTypeDeclaration>, Class.IClassPart
  {
    public HiddenTypePart(IFSharpTypeDeclaration declaration, ICacheBuilder cacheBuilder)
      : base(declaration, ModifiersUtil.GetDecoration(declaration.AccessModifiers, declaration.AttributesEnumerable),
        declaration.TypeParameters, cacheBuilder)
    {
    }

    public HiddenTypePart(IReader reader) : base(reader)
    {
    }

    public override TypeElement CreateTypeElement()
    {
      return new Class(this);
    }

    public MemberPresenceFlag GetMemberPresenceFlag()
    {
      return MemberPresenceFlag.NONE;
    }

    public override MemberDecoration Modifiers =>
      MemberDecoration.FromModifiers(Psi.Modifiers.INTERNAL); // should not be accessible from other languages

    protected override byte SerializationTag => (byte) FSharpPartKind.HiddenType;
  }
}