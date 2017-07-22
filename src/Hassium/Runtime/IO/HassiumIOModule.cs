namespace Hassium.Runtime.IO
{
    public class HassiumIOModule : InternalModule
    {
        public HassiumIOModule() : base("IO")
        {
            AddAttribute("DirectoryNotFoundException", HassiumDirectoryNotFoundException.TypeDefinition);
            AddAttribute("File", HassiumFile.TypeDefinition);
            AddAttribute("FileNotFoundException", HassiumFileNotFoundException.TypeDefinition);
            AddAttribute("FS", new HassiumFS());
            AddAttribute("Path", new HassiumPath());
            AddAttribute("StreamClosedException", HassiumStreamClosedException.TypeDefinition);
        }
    }
}
