
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;

namespace cfg
{
public partial class Tables
{
    public language.TbLang TbLang {get; }

    public Tables(System.Func<string, JSONNode> loader)
    {
        TbLang = new language.TbLang(loader("language_tblang"));
        ResolveRef();
    }
    
    private void ResolveRef()
    {
        TbLang.ResolveRef(this);
    }
}

}
