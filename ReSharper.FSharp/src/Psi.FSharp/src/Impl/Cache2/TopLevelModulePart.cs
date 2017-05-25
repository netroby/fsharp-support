﻿using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.FSharp.Impl.Cache2.Declarations;
using JetBrains.ReSharper.Psi.FSharp.Tree;

namespace JetBrains.ReSharper.Psi.FSharp.Impl.Cache2
{
  internal class TopLevelModulePart : ModulePartBase<ITopLevelModuleDeclaration>
  {
    public TopLevelModulePart(ITopLevelModuleDeclaration declaration, ICacheBuilder cacheBuilder)
      : base(declaration, ModifiersUtil.GetDecoration(declaration.AccessModifiers, declaration.AttributesEnumerable),
        cacheBuilder)
    {
    }

    public TopLevelModulePart(IReader reader) : base(reader)
    {
    }

    public override TypeElement CreateTypeElement()
    {
      return new FSharpTopLevelModule(this);
    }

    protected override byte SerializationTag => (byte) FSharpPartKind.TopLevelModule;
  }
}