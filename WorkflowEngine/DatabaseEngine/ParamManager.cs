namespace WorkflowEngine.DatabaseEngine
{
    public class ParamManager
    {
        public static ConfigSettings ConfigSettings {get;set;}
    }

    public class ConfigSettings
    {
        public string SystemConnectionString { get; set; }
        public string ApplicationConnectionString { get; set; }
    }
}
