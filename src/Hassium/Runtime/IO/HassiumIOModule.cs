namespace Hassium.Runtime.IO
{
    public class HassiumIOModule : InternalModule
    {
        public HassiumIOModule() : base("IO")
        {
            AddAttribute("File", HassiumFile.TypeDefinition);
            AddAttribute("FileNotFoundException", HassiumFileNotFoundException.TypeDefinition);
            AddAttribute("FS", new HassiumFS());
        }
    }
}
